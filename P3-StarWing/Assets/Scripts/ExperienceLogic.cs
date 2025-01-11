using UnityEngine;
using UnityEngine.UI;

public class ExperienceLogic : MonoBehaviour
{
    [SerializeField] private Image experienceFillImage;
    private float currentExperience = 0;
    private float totalExperience = 0;

    public void AddExperience(float experience)
    {
        currentExperience += experience;
        Debug.Log($"Experience: {currentExperience}/{totalExperience}");
        UpdateExperienceUI();
    }

    public void AddTotalExperience(float experience)
    {
        totalExperience += experience;
        Debug.Log($"Total Experience: {currentExperience}/{totalExperience}");
        UpdateExperienceUI();
    }

    private void UpdateExperienceUI(){
        float experiencePercentage = currentExperience / totalExperience;

        // Asignamos ese porcentaje al fillAmount de la imagen
        experienceFillImage.fillAmount = experiencePercentage;
    }

    public float getTotalExperience()
    {
        return currentExperience/totalExperience;
    }
}
