using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade do movimento
    public float gridSize = 1f;  // Tamanho do tile (ex: 1 unidade)

    private Vector2 targetPosition; // Posição alvo para onde o personagem vai se mover
    private bool isMoving = false;  // Indica se o personagem está se movendo

    void Start()
    {
        targetPosition = transform.position; // Define a posição inicial como alvo
    }

    void Update()
    {
        // Somente permite o input se não estiver se movendo
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                StartMovement(Vector2.up);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                StartMovement(Vector2.down);
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                StartMovement(Vector2.left);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                StartMovement(Vector2.right);
        }

        // Move o personagem em direção ao alvo
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Checa se o personagem chegou ao destino
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
}
