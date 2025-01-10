using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownLogic : MonoBehaviour
{
    public Image cooldownBar;         // Barra de cooldown
    public Image[] boostImages;       // Imágenes de boost

    public float currentTime = 5000;  // Tiempo actual (inicia al máximo)
    private float maxTime = 5000;     // Tiempo máximo para el cooldown
    private bool isBoostActive = false; // Estado del boost activo

    void Start()
    {
        // Validar que todos los elementos están asignados
        if (cooldownBar == null || boostImages == null || boostImages.Length != 3)
        {
            Debug.LogError("Error: Debes asignar exactamente 3 imágenes y una barra de cooldown.");
            return;
        }

        // Inicializar las imágenes como interactuables
        foreach (var image in boostImages)
        {
            image.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        // Actualizar la barra de cooldown
        cooldownBar.fillAmount = currentTime / maxTime;

        // Reducir el tiempo si el boost está activo
        if (isBoostActive)
        {
            currentTime += Time.deltaTime * 1000; // Reducir el tiempo en milisegundos
            if (currentTime >= maxTime)
            {
                currentTime = maxTime;
                isBoostActive = false; // Termina el estado de boost
                Debug.Log("Boost terminado.");
            }
        }

        // Activar boost al presionar 'B' si las condiciones se cumplen
        if (Input.GetKeyDown(KeyCode.B) && !isBoostActive && currentTime >= maxTime && AnyImageActive())
        {
            ActivateBoost();
        }

        // Activar imágenes al presionar '+'
        if (Input.GetKeyDown(KeyCode.A)) // Equals es la tecla '+'
        {
            ActivateNextImage();
        }
    }

    // Verifica si alguna imagen está activa
    private bool AnyImageActive()
    {
        foreach (var image in boostImages)
        {
            if (image.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    // Activar el estado de boost
    private void ActivateBoost()
    {
        // Desactivar la primera imagen activa
        foreach (var image in boostImages)
        {
            if (image.gameObject.activeSelf)
            {
                image.gameObject.SetActive(false);
                break;
            }
        }

        // Activar el boost
        isBoostActive = true;
        currentTime = 0; // Resetea el cooldown
        Debug.Log("Boost activado.");
    }

    // Activar la siguiente imagen, hasta un máximo de 3
    private void ActivateNextImage()
    {
        for (int i = boostImages.Length - 1; i >= 0; i--) // Comienza desde el final
        {
            if (!boostImages[i].gameObject.activeSelf)
            {
                boostImages[i].gameObject.SetActive(true);
                Debug.Log("Imagen activada.");
                return;
            }
        }
        Debug.Log("No se pueden activar más imágenes, ya están todas activas.");
    }
}
