using UnityEngine;
using UnityEngine.UI; // Necessário para usar UI

public class PlayerHealth : MonoBehaviour
{
    public Image[] hearts; // Array para armazenar as imagens dos corações
    private int currentHealth;  // Vida atual do jogador

    void Start()
    {
        currentHealth = hearts.Length; // Inicia com o número total de corações
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o player colidiu com um inimigo
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(); // Chama a função de dano
        }
    }

    void TakeDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth--; // Reduz a vida
            hearts[currentHealth].enabled = false; // Desativa o coração da UI
        }

        // Se a vida chegar a 0, pode implementar lógica de "game over"
        if (currentHealth == 0)
        {
            Debug.Log("Game Over!");
            // Exemplo: Desativar o jogador
            gameObject.SetActive(false);
        }
    }
}
