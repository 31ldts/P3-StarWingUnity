using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    // Referencia al script que maneja la vida
    //private LifeComponent lifeComponent;
    [SerializeField] private LifeComponent lifeComponent;

    // Referencia a la imagen de la barra de vida
    //private Image healthFillImage;
    [SerializeField] private Image healthFillImage;

    private void Start()
    {
        //lifeComponent = GetComponent<LifeComponent>();
        //healthFillImage = GetComponent<Image>();
    }

    private void Update()
    {
        // Calculamos el porcentaje de vida
        float healthPercentage = lifeComponent.currentHealth / lifeComponent.maxHealth;

        // Asignamos ese porcentaje al fillAmount de la imagen
        healthFillImage.fillAmount = healthPercentage;
    }
}
