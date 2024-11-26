using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float detectionRange = 6f; // Distância de detecção do jogador
    public float gridSize = 1f; // Tamanho do movimento em cada turno
    public Transform player; // Referência ao jogador
    private bool isMoving = false; // Verifica se o inimigo está se movendo
    private Vector3 targetPosition; // Posição alvo do inimigo

    private void Start()
    {
        targetPosition = transform.position; // Define a posição inicial
    }

    public void TakeTurn()
    {
        if (isMoving) return; // Não realiza outra ação se já estiver se movendo

        // Verifica a distância ao jogador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            // Determina a direção para o jogador
            Vector3 direction = (player.position - transform.position).normalized;

            // Ajusta a direção para o grid
            Vector3Int gridDirection = Vector3Int.RoundToInt(new Vector3(direction.x, direction.y, 0));
            Move(gridDirection);
        }
    }

    private void Move(Vector3Int direction)
    {
        // Calcula a nova posição
        targetPosition = transform.position + new Vector3(direction.x, direction.y, 0) * gridSize;

        // Inicia o movimento
        StartCoroutine(MoveToTarget());
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
}
