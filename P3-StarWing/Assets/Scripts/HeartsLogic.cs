using UnityEngine;
using UnityEngine.UI;

public class HeartsLogic : MonoBehaviour
{
    [SerializeField] private Image[] heartImages;
    
    void Start()
    {
        // Validar asignaciones
        if (heartImages == null || heartImages.Length != 3)
        {
            //Debug.LogError("Error: Debes asignar exactamente 3 imágenes.");
            return;
        }
        
        // Activar todas las imágenes de boost
        foreach (var image in heartImages)
        {
            image.gameObject.SetActive(true);
        }
    }

    public bool ThereAreHearts()
    {
        Image firstActiveHeart = null;
        int activeHearts = 0;

        // Primera pasada: contamos activos y almacenamos el primero
        foreach (var image in heartImages)
        {
            if (image.gameObject.activeSelf)
            {
                activeHearts++;
                if (firstActiveHeart == null)
                {
                    firstActiveHeart = image;
                }
            }
        }

        // Si tenemos 1 o menos, desactivamos todos y retornamos false
        if (activeHearts <= 1)
        {
            foreach (var image in heartImages)
            {
                if (image.gameObject.activeSelf)
                {
                    image.gameObject.SetActive(false);
                }
            }
            return false;
        }

        // Si hay más de 1, desactivamos el primero que encontramos y retornamos true
        if (firstActiveHeart != null)
        {
            firstActiveHeart.gameObject.SetActive(false);
            Debug.Log("Vida restaurada.");
            return true;
        }

        // Si nada anterior se cumplió, retornamos false
        return false;
    }

}
