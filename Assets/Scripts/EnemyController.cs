using System.Collections;  // Necessário para usar IEnumerator e corrotinas
using UnityEngine;         // Necessário para usar MonoBehaviour

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;  // Velocidade do movimento
    public float detectionRange = 5f; // Distância de detecção
    public float attackRange = 1f;  // Distância para atacar

    public Transform player; // Referência ao jogador
    public GameController gameController; // Referência ao GameController

    private bool isMoving = false;  // Inicia com o inimigo sem se mover
    private Vector3 targetPosition; // Posição alvo para onde o inimigo vai se mover
    private float waitTime = 1f; // Tempo que o inimigo aguarda entre os movimentos
    private float currentWaitTime = 0f;  // Contador de espera

    void Update()
    {
        // Impede o movimento enquanto é o turno do jogador ou enquanto o inimigo está se movendo
        if (gameController.isPlayerTurn || isMoving) return; // Retorna sem fazer nada se for o turno do jogador ou se o inimigo já está se movendo

        // Se o jogador está dentro do alcance de detecção
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            // Calcula a direção para o jogador
            Vector3 direction = (player.position - transform.position).normalized;

            // Atualiza a posição alvo para se mover em direção ao jogador
            targetPosition = transform.position + direction;

            // Inicia o movimento do inimigo
            isMoving = true;
            StartCoroutine(MoveTowardsPlayer(direction)); // Inicia a corrotina para mover o inimigo
        }
        else
        {
            // Caso o jogador não esteja no alcance, o inimigo termina o turno sem fazer nada
            gameController.EndEnemyTurn();
        }
    }

    // Corrotina para mover o inimigo
    private IEnumerator MoveTowardsPlayer(Vector3 direction)
    {
        float step = moveSpeed * Time.deltaTime; // Calcula o passo de movimento

        // Move o inimigo na direção do jogador até alcançar a posição alvo
        while (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            yield return null; // Espera um frame antes de continuar o movimento
        }

        // Chegou ao jogador, pronto para atacar ou ficar parado
        StopMoving();
    }

    // Interrompe o movimento do inimigo e passa o turno para o jogador
    private void StopMoving()
    {
        isMoving = false;
        gameController.EndEnemyTurn();  // Fim do turno do inimigo, passa para o turno do jogador
    }

    // Função chamada para o inimigo agir no seu turno
    public void TakeTurn()
    {
        // O inimigo pode atacar ou se mover, mas só faz isso se o jogador estiver dentro do alcance
        Debug.Log("Inimigo tomou seu turno!");
    }
}
