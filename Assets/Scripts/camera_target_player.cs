using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraTargetMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gridSize = 1f;

    private Vector2 targetPosition;
    private bool isMoving = false;

    public Tilemap tilemap;
    public LayerMask paredeLayer;
    public LayerMask enemyLayer;

    public GameController gameController;
    public MenuController menuController;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (menuController.IsInventoryOpen)
        {
            return;
        }

        if (gameController.isPlayerTurn && !isMoving)
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

        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if ((Vector2)transform.position == targetPosition)
                isMoving = false;
        }
    }

    private void StartMovement(Vector2 direction)
    {
        targetPosition = (Vector2)transform.position + direction * gridSize;
        isMoving = true;
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
}
