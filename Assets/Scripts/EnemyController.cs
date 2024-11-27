using System.Collections;  // Necessário para usar IEnumerator e corrotinas
using UnityEngine;         // Necessário para usar MonoBehaviour
using UnityEngine.Tilemaps; // Para verificar colisões no Tilemap

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;  // Velocidade do movimento
    public float detectionRange = 5f; // Distância de detecção
    public float attackRange = 1f;  // Distância para atacar

    public Transform player; // Referência ao jogador
    public GameController gameController; // Referência ao GameController
    public Tilemap tilemap; // Referência ao Tilemap para checar paredes

    private bool isMoving = false;  // Indica se o inimigo está se movendo
    private Vector3 targetPosition; // Próxima posição do inimigo

    void Update()
    {
        // O inimigo só age se for seu turno
        if (gameController.isPlayerTurn || isMoving) return;

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // O inimigo ataca o jogador
            AttackPlayer();
        }
        else if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            // O inimigo tenta se mover em direção ao jogador
            MoveTowardsPlayer();
        }
        else
        {
            // Caso o jogador esteja fora do alcance, o inimigo termina o turno
            gameController.EndEnemyTurn();
        }
    }

    // Função para iniciar o turno do inimigo
    public void TakeTurn()
    {
        if (!isMoving) // Apenas age se não estiver em movimento
        {
            Debug.Log($"{gameObject.name} está tomando seu turno.");
            Update(); // Chama o comportamento normal de Update
        }
    }

    // Função para mover o inimigo em direção ao jogador
    private void MoveTowardsPlayer()
    {
        Vector3 direction = Vector3.zero;

        // Calcula a diferença de posição em X e Y
        float diffX = player.position.x - transform.position.x;
        float diffY = player.position.y - transform.position.y;

        // Decide a direção com base na maior diferença
        if (Mathf.Abs(diffX) > Mathf.Abs(diffY))
        {
            // Prioriza movimento na direção horizontal
            direction = new Vector3(Mathf.Sign(diffX), 0, 0);
        }
        else
        {
            // Prioriza movimento na direção vertical
            direction = new Vector3(0, Mathf.Sign(diffY), 0);
        }

        Vector3 potentialPosition = transform.position + direction;

        // Verifica se pode mover na direção escolhida
        if (CanMove(potentialPosition))
        {
            targetPosition = potentialPosition;
        }
        else
        {
            // Se não puder mover na direção escolhida, tenta a outra
            direction = (direction.x != 0) ? new Vector3(0, Mathf.Sign(diffY), 0) : new Vector3(Mathf.Sign(diffX), 0, 0);
            potentialPosition = transform.position + direction;

            if (CanMove(potentialPosition))
            {
                targetPosition = potentialPosition;
            }
            else
            {
                // Se nenhuma direção for válida, termina o turno
                gameController.EndEnemyTurn();
                return;
            }
        }

        // Move o inimigo para a posição alvo
        isMoving = true;
        StartCoroutine(MoveCoroutine(targetPosition));
    }

    // Corrotina para mover o inimigo até a posição alvo
    private IEnumerator MoveCoroutine(Vector3 target)
    {
        float step = moveSpeed * Time.deltaTime;

        // Move o inimigo até a posição alvo
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            yield return null; // Espera um frame antes de continuar o movimento
        }

        transform.position = target;
        StopMoving();
    }

    // Função de ataque ao jogador
    private void AttackPlayer()
    {
        Debug.Log("Inimigo atacou o jogador!");
        StopMoving();  // Depois de atacar, o inimigo termina seu turno
    }

    // Interrompe o movimento do inimigo e passa o turno ao jogador
    private void StopMoving()
    {
        isMoving = false;
        gameController.EndEnemyTurn();
    }

    // Verifica se o inimigo pode se mover para uma posição específica
    private bool CanMove(Vector3 target)
    {
        // Converte a posição de destino para o grid do Tilemap
        Vector3Int cellPosition = tilemap.WorldToCell(target);
        TileBase tile = tilemap.GetTile(cellPosition);

        // Verifica se há um tile bloqueando o caminho
        return tile == null || tilemap.GetColliderType(cellPosition) == Tile.ColliderType.None;
    }
}
