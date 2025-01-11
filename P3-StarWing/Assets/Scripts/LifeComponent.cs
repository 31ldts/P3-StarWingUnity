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
                PlayerDied(true);
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

    public void PlayerDied(bool killed)
    {
        Debug.Log("Player died.");
        if (heartsLogic.ThereAreHearts())
        {
            if (killed)
            {
                RestartPlayer();
            } else
            {
                //Die();
                Canvas[] allCanvases = Resources.FindObjectsOfTypeAll<Canvas>();

                foreach (Canvas canvas in allCanvases)
                {
                    if (canvas.gameObject.CompareTag("NoCompleted"))
                    {
                        canvas.gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
        else
        {
            Canvas[] allCanvases = Resources.FindObjectsOfTypeAll<Canvas>();

            foreach (Canvas canvas in allCanvases)
            {
                if (canvas.gameObject.CompareTag("Defeat"))
                {
                    canvas.gameObject.SetActive(true);
                    break;
                }
            }
            Debug.Log("No hay más corazones.");
            Die();
        }
    }

    public void RestartPlayer()
    {
        gameObject.transform.position = new Vector3(0, 8, 0);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        Debug.Log("Player restarted.");
        currentHealth = maxHealth;
    }
}
