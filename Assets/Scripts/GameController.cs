using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool isPlayerTurn = true; // Determina se é o turno do jogador
    private EnemyController[] enemies; // Lista de inimigos
    private int currentEnemyIndex = 0; // Índice do inimigo atual

    void Start()
    {
        // Obtém a lista de inimigos no início do jogo
        enemies = FindObjectsOfType<EnemyController>();
    }

    // Chamado no fim do turno do jogador
    public void EndPlayerTurn()
    {
        if (!isPlayerTurn) return; // Evita duplicidade
        isPlayerTurn = false; // Fim do turno do jogador
        currentEnemyIndex = 0; // Reinicia o índice para o primeiro inimigo
        ProcessNextEnemy(); // Começa o turno dos inimigos
    }

    // Processa o próximo inimigo na sequência
    private void ProcessNextEnemy()
    {
        if (currentEnemyIndex < enemies.Length)
        {
            Debug.Log($"Processando turno do inimigo {currentEnemyIndex + 1}/{enemies.Length}");
            enemies[currentEnemyIndex].TakeTurn(); // Inimigo atual age
        }
        else
        {
            EndEnemyTurn(); // Todos os inimigos já agiram, volta para o jogador
        }
    }

    // Chamado pelo inimigo quando ele termina sua ação
    public void EndEnemyAction()
    {
        currentEnemyIndex++; // Passa para o próximo inimigo
        ProcessNextEnemy(); // Processa o próximo
    }

    // Chamado no fim do turno dos inimigos
    private void EndEnemyTurn()
    {
        isPlayerTurn = true; // Agora é o turno do jogador
        Debug.Log("Turno do inimigo acabou. É o turno do jogador.");
    }
}
