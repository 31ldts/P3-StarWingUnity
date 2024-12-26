using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.IO;

public class RawImageManager : MonoBehaviour
{
    // Array para almacenar las referencias a las imágenes RawImage
    public RawImage[] rawImages;

    // Lista de texturas asociadas a cada número
    public List<Texture> textures; // Lista de 5 texturas (índices 0-4)

    // Botón que se habilitará al pulsar un número
    public Button actionButton;

    // Lista generada desde el archivo
    private List<int> imageConfig;

    // Referencia a las imágenes RawImage
    public RawImage shipImage;

    private int shipTextureIndex = 0;

    // Lista de texturas asociadas a cada número
    public List<Texture> shipTextures; 

    void Start()
    {
        // Deshabilitar el botón al inicio
        if (actionButton != null)
        {
            actionButton.interactable = false;
        }

        // Inicializar la lista de configuración leyendo el archivo
        string filePath = "Assets/Resources/LevelConfig.txt"; // Ruta del archivo
        imageConfig = LoadConfiguration(filePath);

        // Validar y actualizar las imágenes
        if (imageConfig != null)
        {
            UpdateImages(imageConfig);
            if (shipImage != null && shipTextures != null)
            {
                shipImage.texture = shipTextures[shipTextureIndex];
            }
            else
            {
                Debug.LogError("Error al cargar las imágenes de la nave.");
            }
        }
        else
        {
            Debug.LogError("Error al cargar la configuración. Revisa el archivo.");
        }
    }

    void Update()
    {
        // Detectar teclas del 1 al 9 para cambiar dinámicamente las imágenes
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i) && imageConfig[i-1] != 0)
            {
                Debug.Log($"Tecla {i} pulsada.");
                UpdateImages(imageConfig);
                ChangeImage(i - 1, textures.Count - 1); // Cambia al siguiente índice circularmente
                // Habilitar el botón si no está ya habilitado
                if (actionButton != null && !actionButton.interactable)
                {
                    actionButton.interactable = true;
                    Debug.Log("Botón habilitado.");
                }
            }
        }

        // Detectar flecha izquierda
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Flecha izquierda pulsada");
            if (shipImage != null && shipTextures != null)
            {
                shipTextureIndex = shipTextureIndex != 0 ? (shipTextureIndex - 1) : (shipTextures.Count - 1);
                shipImage.texture = shipTextures[shipTextureIndex];
            }
            else
            {
                Debug.LogError("Error al cargar las imágenes de la nave.");
            }
        }

        // Detectar flecha derecha
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Flecha derecha pulsada");
            if (shipImage != null && shipTextures != null)
            {
                shipTextureIndex = (shipTextureIndex + 1) % shipTextures.Count;
                shipImage.texture = shipTextures[shipTextureIndex];
            }
            else
            {
                Debug.LogError("Error al cargar las imágenes de la nave.");
            }
        }

        // Detectar Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (actionButton != null && !actionButton.interactable)
            {
                Debug.LogError("Botón no habilitado.");
            }
            else
            {
                Debug.Log("Tecla Enter pulsada");
            }
        }
    }

    // Función para cargar y validar la configuración desde un archivo
    private List<int> LoadConfiguration(string filePath)
    {
        List<int> config = new List<int>();
        try
        {
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length != rawImages.Length)
            {
                throw new Exception("El archivo debe contener exactamente 9 líneas.");
            }

            foreach (string line in lines)
            {
                if (int.TryParse(line, out int value) && value >= 0 && value <= 4)
                {
                    config.Add(value);
                }
                else
                {
                    throw new Exception($"Valor inválido en el archivo: {line}. Debe ser un número entre 0 y 4.");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al leer el archivo de configuración: {e.Message}");
            return null;
        }

        return config;
    }

    // Función para actualizar las imágenes según una lista de configuración
    private void UpdateImages(List<int> config)
    {
        for (int i = 0; i < rawImages.Length; i++)
        {
            if (i < config.Count && config[i] >= 0 && config[i] < textures.Count)
            {
                rawImages[i].texture = textures[config[i]];
                Debug.Log($"Imagen {i + 1} actualizada a la textura {config[i]}.");
            }
            else
            {
                Debug.LogWarning($"Índice de configuración fuera de rango para la imagen {i + 1}.");
            }
        }
    }

    // Función para cambiar una imagen específica
    private void ChangeImage(int index, int newTextureIndex)
    {
        if (index >= 0 && index < rawImages.Length && newTextureIndex >= 0 && newTextureIndex < textures.Count)
        {
            rawImages[index].texture = textures[newTextureIndex];
            Debug.Log($"Imagen {index + 1} cambiada a la textura {newTextureIndex}.");
        }
        else
        {
            Debug.LogWarning($"Índice fuera de rango: {index} o textura {newTextureIndex}.");
        }
    }
}
