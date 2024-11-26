using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool isPlayerTurn = true; // Determina se é o turno do jogador

    // Chamado no fim do turno do jogador
    public void EndPlayerTurn()
    {
        isPlayerTurn = false; // Fim do turno do jogador
        NotifyEnemies(); // Faz os inimigos agirem
    }

    // Chamado no fim do turno do inimigo
    public void EndEnemyTurn()
    {
        isPlayerTurn = true; // Agora é o turno do jogador
    }

    // Notifica os inimigos para agir (simula o turno do inimigo)
    private void NotifyEnemies()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            enemy.TakeTurn();  // Faz os inimigos tomarem seu turno
        }
    }
}
