using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image[] hearts;
    private int currentHealth;

    void Start()
    {
        currentHealth = hearts.Length;
    }

    public void TakeDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth--;
            hearts[currentHealth].enabled = false;
        }

        if (currentHealth == 0)
        {
            Debug.Log("Game Over!");
            gameObject.SetActive(false);
        }
    }

    public void Heal(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (currentHealth < hearts.Length)
            {
                hearts[currentHealth].enabled = true;
                currentHealth++;
            }
        }
    }
}