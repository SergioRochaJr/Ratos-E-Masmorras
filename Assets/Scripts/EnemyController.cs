using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;       // Velocidade do movimento
    public float detectionRange = 5f; // Distância de detecção
    public float attackRange = 1f;    // Distância para atacar

    public Transform player;          // Referência ao jogador
    public GameController gameController; // Referência ao GameController
    public Tilemap tilemap;           // Referência ao Tilemap para checar paredes
    public LayerMask collisionLayers; // Camadas para checar colisões (Player e Inimigos)

    private bool isMoving = false;    // Indica se o inimigo está se movendo
    private Vector3 targetPosition;   // Próxima posição do inimigo
    private SpriteRenderer spriteRenderer; // Para espelhar o sprite ao mudar de direção

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeTurn()
    {
        if (isMoving)
        {
            Debug.Log($"{gameObject.name} ainda está se movendo. Ignorando turno.");
            return;
        }

        Debug.Log($"{gameObject.name} está tomando seu turno.");

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            AttackPlayer();
        }
        else if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            EndTurn(); // Nada a fazer, termina o turno
        }
    }

private void MoveTowardsPlayer()
{
    Vector3 direction = Vector3.zero;

    // Calcula a diferença de posição em X e Y
    float diffX = player.position.x - transform.position.x;
    float diffY = player.position.y - transform.position.y;

    if (Mathf.Abs(diffX) > Mathf.Abs(diffY))
    {
        direction = new Vector3(Mathf.Sign(diffX), 0, 0); // Movimento horizontal
    }
    else
    {
        direction = new Vector3(0, Mathf.Sign(diffY), 0); // Movimento vertical
    }

    Vector3 potentialPosition = transform.position + direction;

    if (CanMove(potentialPosition))
    {
        UpdateSpriteDirection(direction); // Atualiza a direção do sprite
        targetPosition = potentialPosition;
        isMoving = true;
        StartCoroutine(MoveCoroutine(targetPosition));
    }
    else
    {
        Debug.Log($"{gameObject.name} não pode se mover na direção inicial. Tentando outra.");
        direction = (direction.x != 0) ? new Vector3(0, Mathf.Sign(diffY), 0) : new Vector3(Mathf.Sign(diffX), 0, 0);
        potentialPosition = transform.position + direction;

        if (CanMove(potentialPosition))
        {
            UpdateSpriteDirection(direction); // Atualiza a direção do sprite
            targetPosition = potentialPosition;
            isMoving = true;
            StartCoroutine(MoveCoroutine(targetPosition));
        }
        else
        {
            Debug.Log($"{gameObject.name} está preso. Terminando turno.");
            EndTurn();
        }
    }
}

    private IEnumerator MoveCoroutine(Vector3 target)
    {
        float step = moveSpeed * Time.deltaTime;

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            yield return null; // Espera um frame antes de continuar o movimento
        }

        transform.position = target;
        StopMoving();
    }

    private void AttackPlayer()
    {
        Debug.Log($"{gameObject.name} atacou o jogador!");
        EndTurn();
    }

    private void StopMoving()
    {
        isMoving = false;
        EndTurn();
    }

    private void EndTurn()
    {
        gameController.EndEnemyAction();
    }

    private bool CanMove(Vector3 target)
    {
        // Verifica se o Tilemap permite o movimento
        Vector3Int cellPosition = tilemap.WorldToCell(target);
        TileBase tile = tilemap.GetTile(cellPosition);

        bool tileIsWalkable = tile == null || tilemap.GetColliderType(cellPosition) == Tile.ColliderType.None;

        // Verifica colisões com objetos nas camadas configuradas
        bool hasCollision = Physics2D.OverlapCircle(target, 0.1f, collisionLayers);

        return tileIsWalkable && !hasCollision;
    }

    private void UpdateSpriteDirection(Vector3 direction)
{
    // Atualiza a direção do sprite baseada na direção do movimento
    if (direction.x != 0)
    {
        // Se o inimigo está se movendo para a direita (x positivo), não espelha
        // Se está indo para a esquerda (x negativo), espelha
        spriteRenderer.flipX = direction.x > 0;
    }
}
}


