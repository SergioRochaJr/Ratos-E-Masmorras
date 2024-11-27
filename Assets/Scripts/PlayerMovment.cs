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

    // Referência ao Tilemap para verificar a colisão com as paredes
    public Tilemap tilemap;
    public LayerMask paredeLayer; // Camada onde as paredes estão (ex: camada "Parede")
    public LayerMask enemyLayer;  // Camada onde os inimigos estão

    private SpriteRenderer spriteRenderer; // Para controlar o espelhamento do sprite
    public GameController gameController; // Referência ao GameController (onde gerenciamos o turno)

    public MenuController menuController; // Referência ao MenuController


    void Start()
    {
        targetPosition = transform.position; // Define a posição inicial como alvo
        originalPosition = transform.position; // Salva a posição inicial
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtém o SpriteRenderer
    }

    void Update()
    {
         // Verifica se o menu do inventário está aberto
    if (menuController != null && menuController.IsInventoryOpen)
        return;

    // Verifica se é o turno do jogador e se ele não está em movimento
    if (!isMoving && gameController.isPlayerTurn)
    {
        // Verifica se o menu do inventário está aberto
    if (menuController != null && menuController.IsInventoryOpen)
        return;

    // Verifica se é o turno do jogador e se ele não está em movimento
    if (!isMoving && gameController.isPlayerTurn)
    {
        // Verifica o input para cada direção
        if (Input.GetKeyDown(KeyCode.UpArrow) && CanMove(Vector2.up))
            StartMovement(Vector2.up);
        else if (Input.GetKeyDown(KeyCode.DownArrow) && CanMove(Vector2.down))
            StartMovement(Vector2.down);
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && CanMove(Vector2.left))
            StartMovement(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.RightArrow) && CanMove(Vector2.right))
            StartMovement(Vector2.right);
        else if (Input.GetKeyDown(KeyCode.E))
        {
            PerformAttack();
        }
    }

    // Checa se o movimento terminou
    if (isMoving && (Vector2)transform.position == targetPosition)
    {
        isMoving = false;
        gameController.EndPlayerTurn(); // Termina o turno do jogador
    }

    // Move o personagem em direção ao alvo
    if (isMoving)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
        }

        // Checa se o movimento terminou
        if (isMoving && (Vector2)transform.position == targetPosition)
        {
            isMoving = false;
            gameController.EndPlayerTurn(); // Termina o turno do jogador
        }

        // Move o personagem em direção ao alvo
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    // Inicia o movimento em uma direção específica
    private void StartMovement(Vector2 direction)
    {
        targetPosition = (Vector2)transform.position + direction * gridSize; // Define o próximo tile como alvo

        // Atualiza o sprite para a direção horizontal
        if (direction.x != 0)
        {
            FlipSprite(direction.x < 0); // Espelha o sprite se movendo para a esquerda
        }

        isMoving = true;
        StartCoroutine(HopEffect()); // Adiciona o efeito de "pulo"
    }

    // Simula um pequeno pulo ao andar
    private System.Collections.IEnumerator HopEffect()
    {
        // Sobe um pouco
        transform.position += Vector3.up * hopHeight;
        yield return new WaitForSeconds(0.1f);

        // Retorna à posição original
        transform.position -= Vector3.up * hopHeight;
    }

    // Checa se é possível mover para a direção fornecida (verifica se há uma parede ou inimigo)
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

    // Espelha o sprite horizontalmente
    private void FlipSprite(bool faceLeft)
    {
        spriteRenderer.flipX = faceLeft;
    }

    // Simula o ataque com um pulo
    private void PerformAttack()
    {
        StartCoroutine(AttackHopEffect());
    }

    // Efeito de "pulo" ao atacar
    private System.Collections.IEnumerator AttackHopEffect()
    {
        // Salva a posição atual antes do ataque
        Vector3 preAttackPosition = transform.position;

        // Sobe mais alto (simula o ataque)
        transform.position += Vector3.up * attackHopHeight;
        yield return new WaitForSeconds(0.2f);

        // Retorna à posição anterior ao ataque
        transform.position = preAttackPosition;
    }
}
