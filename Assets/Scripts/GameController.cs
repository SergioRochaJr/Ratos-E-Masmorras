using System.Linq;
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
            if (enemies[currentEnemyIndex] != null) 
            {
                Debug.Log($"Processando turno do inimigo {currentEnemyIndex + 1}/{enemies.Length}");
                enemies[currentEnemyIndex].TakeTurn();
            }
            else
            {
                Debug.Log($"Inimigo {currentEnemyIndex + 1} foi destruído. Passando para o próximo.");
                currentEnemyIndex++;
                ProcessNextEnemy();
            }
        }
        else
        {
            EndEnemyTurn();
        }
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        enemies = enemies.Where(e => e != enemy).ToArray();  // Remove o inimigo da lista
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
