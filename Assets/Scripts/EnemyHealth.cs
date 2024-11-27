using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 2;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} foi derrotado!");
        Destroy(gameObject);
    }
}