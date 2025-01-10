using UnityEngine;
using System.Collections.Generic; // Para la clase List<T>
using System.IO; // Para trabajar con archivos y rutas

public class _Captures : MonoBehaviour
{
    public List<GameObject> shipPrefabs; // Lista de prefabs FBX
    public Camera captureCamera; // Cámara configurada con fondo transparente
    public string savePath = "Assets/Screnshots"; // Ruta donde se guardarán las imágenes

    void Start()
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        for (int i = 0; i < shipPrefabs.Count; i++)
        {
            CaptureImage(shipPrefabs[i], i);
        }
    }

    void CaptureImage(GameObject shipPrefab, int index)
    {
        // Instanciar la nave en la escena
        GameObject shipInstance = Instantiate(shipPrefab);
        // Aplicar posición y rotación deseadas
        shipInstance.transform.position = new Vector3(0, 1, -5);
        shipInstance.transform.rotation = Quaternion.Euler(0, 180, 0);

        // Renderizar la imagen
        RenderTexture renderTexture = new RenderTexture(1024, 1024, 24);
        captureCamera.targetTexture = renderTexture;
        Texture2D screenshot = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);

        captureCamera.Render();
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, 1024, 1024), 0, 0);
        screenshot.Apply();

        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // Guardar la imagen como PNG
        byte[] bytes = screenshot.EncodeToPNG();
        string filename = Path.Combine(savePath, $"Ship_{index}.png");
        File.WriteAllBytes(filename, bytes);
        Debug.Log($"Imagen guardada en: {filename}");

        // Destruir la instancia
        Destroy(shipInstance);
    }
}
