using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade do movimento
    public float gridSize = 1f;  // Tamanho do tile (ex: 1 unidade)

    private Vector2 targetPosition; // Posi��o alvo para onde o personagem vai se mover
    private bool isMoving = false;  // Indica se o personagem est� se movendo

    void Start()
    {
        targetPosition = transform.position; // Define a posi��o inicial como alvo
    }

    void Update()
    {
        // Somente permite o input se n�o estiver se movendo
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

        // Move o personagem em dire��o ao alvo
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Checa se o personagem chegou ao destino
            if ((Vector2)transform.position == targetPosition)
                isMoving = false;
        }
    }

    // Inicia o movimento em uma dire��o espec�fica
    private void StartMovement(Vector2 direction)
    {
        targetPosition = (Vector2)transform.position + direction * gridSize; // Define o pr�ximo tile como alvo
        isMoving = true;
    }
}
