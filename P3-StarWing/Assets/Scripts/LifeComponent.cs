using UnityEngine;

public class LifeComponent : MonoBehaviour
{
    public GameObject explosionEffect;

    public float maxHealth = 100f; // Salud máxima
    public float currentHealth = 100f; // Salud actual

    public float collisionDamage = 20f;

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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

         
        if (!collision.gameObject.CompareTag("Projectile"))
        {
            if (collision.gameObject.CompareTag("Asteroid"))
            {
                if (gameObject.CompareTag("Player"))
                {
                    Debug.Log("Entroo.");
                    // Si es el jugador, resta 30 puntos de vida y hace explotar el asteroide
                    doDamage(30f); // Daño de 30 al jugador
                    ExplodeAsteroid(collision.gameObject); // Hacer explotar el asteroide
                }
                // Si es un enemigo, no hacer nada
                else if (gameObject.CompareTag("Enemy"))
                {
                    // No hacer nada
                    return;
                }
            }
            else if (!(gameObject.CompareTag("Enemy") && collision.gameObject.CompareTag("Ground")))
            {
                doDamage(collisionDamage);
            }
        }
    }

    // Función para aplicar daño o curación
    public void doDamage(float amount)
    {
        if (amount > maxHealth) {
            amount = maxHealth;
        }

        // Aplica daño o curación y restringe el resultado entre el valor mínimo y máximo
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);

        if (gameObject.CompareTag("Enemy") || gameObject.CompareTag("Asteroid")) {
            experienceLogic.AddExperience(amount);
        }

        // Para depuración
        Debug.Log($"{gameObject.name} ha rebut {amount} dany. Vida restant: {currentHealth}");
        Debug.Log($"Current Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
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
            }
            else
            {
                CanvasHandler.ActivateCanvas("NoCompleted");
            }
        }
        else
        {
            CanvasHandler.ActivateCanvas("Defeat");
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

    // Función para hacer explotar el asteroide
    private void ExplodeAsteroid(GameObject asteroid)
    {
        // Si el asteroide tiene una explosión asignada, creamos el efecto
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, asteroid.transform.position, asteroid.transform.rotation);
        }
        
        // Destruimos el asteroide
        Destroy(asteroid);
    }
}
