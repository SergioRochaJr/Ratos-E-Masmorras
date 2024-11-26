using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour
{
    public float detectionRange = 6f; // Alcance de detecção do jogador
    public float gridSize = 1f; // Tamanho do movimento por turno
    public Transform player; // Referência ao jogador
    public Tilemap tilemap; // Referência ao Tilemap
    public LayerMask paredeLayer; // Camada das paredes (usada para verificação de colisão)

    private bool isMoving = false; // Indica se o inimigo está se movendo
    private Vector3 targetPosition; // Próxima posição do inimigo

    private void Start()
    {
        targetPosition = transform.position; // Posição inicial
    }

    public void TakeTurn()
    {
        if (isMoving) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3Int gridDirection = Vector3Int.RoundToInt(new Vector3(direction.x, direction.y, 0));

            if (CanMove(gridDirection))
            {
                Move(gridDirection);
            }
            else
            {
                TryRandomMove();
            }
        }
    }

    private void Move(Vector3Int direction)
    {
        targetPosition = transform.position + new Vector3(direction.x, direction.y, 0) * gridSize;
        StartCoroutine(MoveToTarget());
    }

    private bool CanMove(Vector3Int direction)
    {
        Vector3 newPosition = transform.position + new Vector3(direction.x, direction.y, 0) * gridSize;
        Vector3Int tilePosition = tilemap.WorldToCell(newPosition);
        TileBase tileAtPosition = tilemap.GetTile(tilePosition);

        if (tileAtPosition != null && tilemap.GetColliderType(tilePosition) != Tile.ColliderType.None)
        {
            return false;
        }

        return true;
    }

    private System.Collections.IEnumerator MoveToTarget()
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5f);
            yield return null;
        }
        transform.position = targetPosition;
        isMoving = false;
    }

    private void TryRandomMove()
    {
        Vector3Int[] directions = {
            Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
        };

        foreach (var direction in directions)
        {
            if (CanMove(direction))
            {
                Move(direction);
                return;
            }
        }
    }
}
