using UnityEngine;

public class LifeComponent : MonoBehaviour
{
    public float maxHealth = 100f; // Salud máxima
    public float currentHealth = 100f; // Salud actual

    // Función para aplicar daño o curación
    public void doDamage(float amount)
    {
        // Aplica daño o curación y clampa el resultado
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        // Para depuración
        Debug.Log($"Current Health: {currentHealth}/{maxHealth}");
    }
}
