using UnityEngine;

public class LifeComponent : MonoBehaviour
{
    public GameObject explosionEffect;

    public float maxHealth = 100f; // Salud máxima
    public float currentHealth = 100f; // Salud actual

    // Función para aplicar daño o curación
    public void doDamage(float amount)
    {
        // Aplica daño o curación y restringe el resultado entre el valor mínimo y máximo
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        // Para depuración
        Debug.Log($"{gameObject.name} ha rebut {amount} dany. Vida restant: {currentHealth}");
        Debug.Log($"Current Health: {currentHealth}/{maxHealth}");

        if ( currentHealth <= 0 )
        {
            Die();
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
}
