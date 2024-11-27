using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gridSize = 1f;
    public float hopHeight = 0.2f;
    public float attackHopHeight = 0.5f;

    public AudioClip attackSound;
    private AudioSource audioSource;

    private Vector2 targetPosition;
    private bool isMoving = false;
    private Vector3 originalPosition;

    public Tilemap tilemap;
    public Tilemap itemTilemap;
    public TileBase itemTile;
    public Canvas itemCanvas;

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
        audioSource = GetComponent<AudioSource>();

        if (itemCanvas != null)
        {
            itemCanvas.gameObject.SetActive(false);
        }
    }

    void Update()
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

        CheckItemTileTrigger();
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
        float attackRange = 1.0f;
        float attackWidth = 0.5f;

        Vector2 attackOrigin = (Vector2)transform.position + direction.normalized * 0.5f;
        RaycastHit2D hit = Physics2D.BoxCast(attackOrigin, new Vector2(attackRange, attackWidth), 0, direction, attackRange, enemyLayer);

        if (hit.collider != null)
        {
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1);
                Debug.Log("Inimigo atingido!");
            }
        }

        StartCoroutine(AttackHopEffect());
    }

    private System.Collections.IEnumerator AttackHopEffect()
    {
        Vector3 preAttackPosition = transform.position;
        transform.position += Vector3.up * attackHopHeight;
        yield return new WaitForSeconds(0.2f);

        transform.position = preAttackPosition;
    }

    private void CheckItemTileTrigger()
    {
        if (itemTilemap == null || itemTile == null || itemCanvas == null) return;

        Vector3Int playerCellPosition = itemTilemap.WorldToCell(transform.position);

        if (itemTilemap.GetTile(playerCellPosition) == itemTile)
        {
            ShowItemCanvas();
        }
    }

    private void ShowItemCanvas()
    {
        if (itemCanvas != null)
        {
            itemCanvas.gameObject.SetActive(true);

            Invoke(nameof(HideItemCanvas), 3f);
        }
    }

    private void HideItemCanvas()
    {
        if (itemCanvas != null)
        {
            itemCanvas.gameObject.SetActive(false);
        }
    }
}
