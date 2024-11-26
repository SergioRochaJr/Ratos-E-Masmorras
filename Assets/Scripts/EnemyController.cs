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

    void Update()
    {
        // Impede o movimento enquanto é o turno do jogador ou enquanto o inimigo está se movendo
        if (gameController.isPlayerTurn || isMoving) return;

        // Primeiro, verifica se o jogador está dentro do alcance de ataque
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // O inimigo ataca
            AttackPlayer();
        }
        else if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            // Se não estiver no alcance de ataque, mas dentro do alcance de detecção, o inimigo se move
            MoveTowardsPlayer();
        }
        else
        {
            // Caso o jogador não esteja nem no alcance de detecção, o inimigo termina o turno
            gameController.EndEnemyTurn();  // Isso deve passar o turno para o jogador
        }
    }

    // Função para mover o inimigo em direção ao jogador
    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        targetPosition = transform.position + direction;

        isMoving = true;
        StartCoroutine(MoveCoroutine(direction)); // Inicia a corrotina para mover o inimigo
    }

    // Corrotina para mover o inimigo até a posição do jogador
    private IEnumerator MoveCoroutine(Vector3 direction)
    {
        float step = moveSpeed * Time.deltaTime; // Calcula o passo de movimento

        // Move o inimigo na direção do jogador até alcançar a posição alvo
        while (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            yield return null; // Espera um frame antes de continuar o movimento
        }

        // Chegou ao jogador ou ficou perto o suficiente
        StopMoving();
    }

    // Função de ataque ao jogador
    private void AttackPlayer()
    {
        Debug.Log("Inimigo atacou o jogador!");
        // Aqui você pode adicionar a lógica para o ataque
        StopMoving();  // Depois de atacar, o inimigo termina seu turno
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
        // O inimigo pode atacar ou se mover, dependendo da distância do jogador
        Debug.Log("Inimigo tomou seu turno!");
    }
}
