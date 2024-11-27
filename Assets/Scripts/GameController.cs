using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool isPlayerTurn = true; // Determina se � o turno do jogador
    private EnemyController[] enemies; // Lista de inimigos
    private int currentEnemyIndex = 0; // �ndice do inimigo atual

    void Start()
    {
        // Obt�m a lista de inimigos no in�cio do jogo
        enemies = FindObjectsOfType<EnemyController>();
    }

    // Chamado no fim do turno do jogador
    public void EndPlayerTurn()
    {
        if (!isPlayerTurn) return; // Evita duplicidade
        isPlayerTurn = false; // Fim do turno do jogador
        currentEnemyIndex = 0; // Reinicia o �ndice para o primeiro inimigo
        ProcessNextEnemy(); // Come�a o turno dos inimigos
    }

    // Processa o pr�ximo inimigo na sequ�ncia
    private void ProcessNextEnemy()
    {
        if (currentEnemyIndex < enemies.Length)
        {
            Debug.Log($"Processando turno do inimigo {currentEnemyIndex + 1}/{enemies.Length}");
            enemies[currentEnemyIndex].TakeTurn(); // Inimigo atual age
        }
        else
        {
            EndEnemyTurn(); // Todos os inimigos j� agiram, volta para o jogador
        }
    }

    // Chamado pelo inimigo quando ele termina sua a��o
    public void EndEnemyAction()
    {
        currentEnemyIndex++; // Passa para o pr�ximo inimigo
        ProcessNextEnemy(); // Processa o pr�ximo
    }

    // Chamado no fim do turno dos inimigos
    private void EndEnemyTurn()
    {
        isPlayerTurn = true; // Agora � o turno do jogador
        Debug.Log("Turno do inimigo acabou. � o turno do jogador.");
    }
}
