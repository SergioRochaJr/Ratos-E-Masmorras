using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool isPlayerTurn = true;
    private EnemyController[] enemies;
    private int currentEnemyIndex = 0;

    void Start()
    {
        enemies = FindObjectsOfType<EnemyController>();
    }

    public void EndPlayerTurn()
    {
        if (!isPlayerTurn) return;
        isPlayerTurn = false;
        currentEnemyIndex = 0;
        ProcessNextEnemy();
    }

    private void ProcessNextEnemy()
    {
        if (currentEnemyIndex < enemies.Length)
        {
            Debug.Log($"Processando turno do inimigo {currentEnemyIndex + 1}/{enemies.Length}");
            enemies[currentEnemyIndex].TakeTurn();
        }
        else
        {
            EndEnemyTurn();
        }
    }

    public void EndEnemyAction()
    {
        currentEnemyIndex++;
        ProcessNextEnemy();
    }

    private void EndEnemyTurn()
    {
        isPlayerTurn = true;
        Debug.Log("Turno do inimigo acabou. O turno do jogador.");
    }
}
