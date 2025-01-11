using UnityEngine;

public class CanvasHandler : MonoBehaviour
{
    public static void ActivateCanvas(string tag)
    {
        GetCanvas(tag).gameObject.SetActive(true);
    }

    public static void DeactivateCanvas(string tag)
    {
        GetCanvas(tag).gameObject.SetActive(false);
    }

    private static Canvas GetCanvas(string tag)
    {
        Canvas[] allCanvases = Resources.FindObjectsOfTypeAll<Canvas>();

        foreach (Canvas canvas in allCanvases)
        {
            if (canvas.gameObject.CompareTag(tag))
            {
                return canvas;
            }
        }
        return null;
    }
}
