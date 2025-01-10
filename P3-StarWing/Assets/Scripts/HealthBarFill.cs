using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    // Referencia al script que maneja la vida
    [SerializeField] private LifeComponent lifeComponent;

    // Referencia a la imagen de la barra de vida
    [SerializeField] private Image healthFillImage;

    private void Update()
    {
        // Calculamos el porcentaje de vida
        float healthPercentage = lifeComponent.currentHealth / lifeComponent.maxHealth;

        // Asignamos ese porcentaje al fillAmount de la imagen
        healthFillImage.fillAmount = healthPercentage;
    }
}
