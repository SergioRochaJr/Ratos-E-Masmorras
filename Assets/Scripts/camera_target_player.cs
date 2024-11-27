using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraTargetMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade do movimento
    public float gridSize = 1f;  // Tamanho do tile (ex: 1 unidade)

    private Vector2 targetPosition; // Posição alvo para onde o alvo da câmera vai se mover
    private bool isMoving = false;  // Indica se está se movendo

    // Referência ao Tilemap para verificar a colisão com as paredes
    public Tilemap tilemap;
    public LayerMask paredeLayer; // Camada onde as paredes estão
    public LayerMask enemyLayer;  // Camada onde os inimigos estão

    public GameController gameController; // Referência ao GameController para saber o turno atual

    void Start()
    {
        targetPosition = transform.position; // Define a posição inicial como alvo
    }

    void Update()
    {
        // Só permite mover o alvo da câmera se for o turno do jogador
        if (gameController.isPlayerTurn && !isMoving)
        {
            // Somente permite o input se não estiver se movendo
            if (Input.GetKeyDown(KeyCode.UpArrow) && CanMove(Vector2.up))
                StartMovement(Vector2.up);
            else if (Input.GetKeyDown(KeyCode.DownArrow) && CanMove(Vector2.down))
                StartMovement(Vector2.down);
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && CanMove(Vector2.left))
                StartMovement(Vector2.left);
            else if (Input.GetKeyDown(KeyCode.RightArrow) && CanMove(Vector2.right))
                StartMovement(Vector2.right);
        }

        // Move o alvo da câmera em direção ao destino
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Checa se chegou ao destino
            if ((Vector2)transform.position == targetPosition)
                isMoving = false;
        }
    }

    // Inicia o movimento em uma direção específica
    private void StartMovement(Vector2 direction)
    {
        targetPosition = (Vector2)transform.position + direction * gridSize; // Define o próximo tile como alvo
        isMoving = true;
    }

    // Checa se é possível mover para a direção fornecida
    private bool CanMove(Vector2 direction)
    {
        // Calcula a posição de destino
        Vector2 newPos = (Vector2)transform.position + direction * gridSize;

        // Verifica se o tile de destino está bloqueado (com a camada "Parede")
        Vector3Int tilePosition = tilemap.WorldToCell(newPos);
        TileBase tileAtPosition = tilemap.GetTile(tilePosition);

        if (tileAtPosition != null && tilemap.GetColliderType(tilePosition) != Tile.ColliderType.None)
        {
            return false; // Bloqueado por parede
        }

        // Verifica se há um inimigo na posição de destino
        Collider2D hit = Physics2D.OverlapCircle(newPos, 0.1f, enemyLayer);
        if (hit != null)
        {
            return false; // Bloqueado por inimigo
        }

        return true; // Movimento permitido
    }
}
