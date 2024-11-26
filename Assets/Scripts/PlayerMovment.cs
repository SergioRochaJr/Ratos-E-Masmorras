using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade do movimento
    public float gridSize = 1f;  // Tamanho do tile (ex: 1 unidade)
    public float hopHeight = 0.2f; // Altura do "pulo" ao andar
    public float attackHopHeight = 0.5f; // Altura do "pulo" ao atacar

    private Vector2 targetPosition; // Posição alvo para onde o personagem vai se mover
    private bool isMoving = false;  // Indica se o personagem está se movendo
    private Vector3 originalPosition; // Posição inicial para restaurar após o "pulo"

    public Tilemap tilemap; // Referência ao Tilemap para verificar colisão
    public LayerMask paredeLayer; // Camada onde as paredes estão (ex: camada "Parede")

    private SpriteRenderer spriteRenderer; // Para controlar o espelhamento do sprite

    void Start()
    {
        targetPosition = transform.position; // Define a posição inicial como alvo
        originalPosition = transform.position; // Salva a posição inicial
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtém o SpriteRenderer
    }

    void Update()
    {
        // Verifica se o jogador não está em movimento
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && CanMove(Vector2.up))
                StartMovement(Vector2.up);
            else if (Input.GetKeyDown(KeyCode.DownArrow) && CanMove(Vector2.down))
                StartMovement(Vector2.down);
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && CanMove(Vector2.left))
            {
                StartMovement(Vector2.left);
                FlipSprite(true);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && CanMove(Vector2.right))
            {
                StartMovement(Vector2.right);
                FlipSprite(false);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                PerformAttack();
            }
        }

        // Move o jogador em direção ao alvo
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if ((Vector2)transform.position == targetPosition)
            {
                isMoving = false;
                NotifyEnemies(); // Notifica os inimigos após o movimento
            }
        }
    }

    private void StartMovement(Vector2 direction)
    {
        targetPosition = (Vector2)transform.position + direction * gridSize;
        isMoving = true;
        StartCoroutine(HopEffect());
    }

    private System.Collections.IEnumerator HopEffect()
    {
        transform.position += Vector3.up * hopHeight;
        yield return new WaitForSeconds(0.1f);
        transform.position -= Vector3.up * hopHeight;
    }

    private bool CanMove(Vector2 direction)
    {
        Vector2 newPos = (Vector2)transform.position + direction * gridSize;
        Vector3Int tilePosition = tilemap.WorldToCell(newPos);
        TileBase tileAtPosition = tilemap.GetTile(tilePosition);

        if (tileAtPosition != null && tilemap.GetColliderType(tilePosition) != Tile.ColliderType.None)
        {
            return false;
        }

        return true;
    }

    private void FlipSprite(bool faceLeft)
    {
        spriteRenderer.flipX = faceLeft;
    }

    private void PerformAttack()
    {
        StartCoroutine(AttackHopEffect());
    }

    private System.Collections.IEnumerator AttackHopEffect()
    {
        Vector3 preAttackPosition = transform.position;
        transform.position += Vector3.up * attackHopHeight;
        yield return new WaitForSeconds(0.2f);
        transform.position = preAttackPosition;
    }

    private void NotifyEnemies()
    {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 10f, LayerMask.GetMask("Enemy"));
        foreach (Collider2D collider in nearbyEnemies)
        {
            EnemyController enemy = collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeTurn();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10f);
    }
}
