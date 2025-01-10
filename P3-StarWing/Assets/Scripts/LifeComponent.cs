using UnityEngine;

public class LifeComponent : MonoBehaviour
{
    public GameObject explosionEffect;

    public float maxHealth = 100f; // Salud máxima
    public float currentHealth = 100f; // Salud actual

    private HeartsLogic heartsLogic;
    private ExperienceLogic experienceLogic;

    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            heartsLogic = Object.FindFirstObjectByType<HeartsLogic>();
        }
        else
        {
            // Se entiende que el resto son enemigos
            experienceLogic = Object.FindFirstObjectByType<ExperienceLogic>();
            experienceLogic.AddTotalExperience(maxHealth);
        }
    }
    // Función para aplicar daño o curación
    public void doDamage(float amount)
    {
        // Aplica daño o curación y restringe el resultado entre el valor mínimo y máximo
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (!gameObject.CompareTag("Player")) {
            experienceLogic.AddExperience(-amount);
        }

        // Para depuración
        Debug.Log($"{gameObject.name} ha rebut {amount} dany. Vida restant: {currentHealth}");
        Debug.Log($"Current Health: {currentHealth}/{maxHealth}");

        if ( currentHealth <= 0 )
        {
            if (gameObject.CompareTag("Player"))
            {
                PlayerDied();
            }
            else
            {
                Debug.Log("Enemy has died.");
                Die();
            }
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} ha mort.");

        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

    private void PlayerDied()
    {
        Debug.Log("Player died.");
        if (heartsLogic.ThereAreHearts())
        {
            RestartPlayer();
        }
        else
        {
            Debug.Log("No hay más corazones.");
            Die();
        }
    }

    private void RestartPlayer()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        }
        gameObject.transform.position = new Vector3(0, 8, 0);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        Debug.Log("Player restarted.");
        currentHealth = maxHealth;
    }
}
