using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gridSize = 1f;
    public float hopHeight = 0.2f;
    public float attackHopHeight = 0.5f;

    private Vector2 targetPosition;
    private bool isMoving = false;
    private Vector3 originalPosition;

    public Tilemap tilemap;
    public LayerMask paredeLayer;
    public LayerMask enemyLayer;

    private SpriteRenderer spriteRenderer;
    public GameController gameController;

    public MenuController menuController;


    void Start()
    {
        targetPosition = transform.position;
        originalPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (menuController != null && menuController.IsInventoryOpen)
            return;

        if (!isMoving && gameController.isPlayerTurn)
        {
            if (menuController != null && menuController.IsInventoryOpen)
                return;

            if (!isMoving && gameController.isPlayerTurn)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && CanMove(Vector2.up))
                    StartMovement(Vector2.up);
                else if (Input.GetKeyDown(KeyCode.DownArrow) && CanMove(Vector2.down))
                    StartMovement(Vector2.down);
                else if (Input.GetKeyDown(KeyCode.LeftArrow) && CanMove(Vector2.left))
                    StartMovement(Vector2.left);
                else if (Input.GetKeyDown(KeyCode.RightArrow) && CanMove(Vector2.right))
                    StartMovement(Vector2.right);
            }

            if (isMoving && (Vector2)transform.position == targetPosition)
            {
                isMoving = false;
                gameController.EndPlayerTurn();
            }

            if (isMoving)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
        }

        if (isMoving && (Vector2)transform.position == targetPosition)
        {
            isMoving = false;
            gameController.EndPlayerTurn();
        }

        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void StartMovement(Vector2 direction)
    {
        targetPosition = (Vector2)transform.position + direction * gridSize;

        if (direction.x != 0)
        {
            FlipSprite(direction.x < 0);
        }

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

        Collider2D hit = Physics2D.OverlapCircle(newPos, 0.1f, enemyLayer);
        if (hit != null)
        {
            return false;
        }

        return true;
    }

    private void FlipSprite(bool faceLeft)
    {
        spriteRenderer.flipX = faceLeft;
    }

    public void PerformAttack(Vector2 direction)
    {
        // Tamanho do raio
        float attackRange = 1.0f; // Considerando que o movimento é em grid 1x1
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction, attackRange, enemyLayer);

        if (hit.collider != null)
        {
            // Verifica se atingiu um inimigo
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1); // Aplica dano ao inimigo
                Debug.Log("Inimigo atingido!");
            }
        }

        StartCoroutine(AttackHopEffect());  // Efetua o "pulo" do ataque
    }

    private System.Collections.IEnumerator AttackHopEffect()
    {
        Vector3 preAttackPosition = transform.position;
        transform.position += Vector3.up * attackHopHeight;
        yield return new WaitForSeconds(0.2f);

        transform.position = preAttackPosition;
    }
}