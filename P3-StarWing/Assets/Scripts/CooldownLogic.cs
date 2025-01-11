using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownLogic : MonoBehaviour
{
    [SerializeField] private Image cooldownBar;  // Barra de cooldown
    [SerializeField] private Image[] boostImages; // Imágenes de boost

    [SerializeField] private float maxTime = 5000f; // Tiempo máximo de cooldown
    private float currentTime;                     // Contador interno de cooldown
    private bool isBoostActive = false;            // Indica si el boost está activo
    private bool cooldownReady = true;             // Indica que el cooldown ha finalizado y no se ha notificado aún

    void Start()
    {
        // Validar asignaciones
        if (cooldownBar == null || boostImages == null || boostImages.Length != 3)
        {
            Debug.LogError("Error: Debes asignar exactamente 3 imágenes y una barra de cooldown.");
            return;
        }

        // Inicializar valores
        currentTime = maxTime;
        
        // Activar todas las imágenes de boost
        foreach (var image in boostImages)
        {
            image.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        // Manejo principal del cooldown y boost
        HandleCooldown();
        HandleBoost();

        // Manejo de la activación de imágenes con la tecla A
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            ActivateNextImage();
        }*/
    }

    private void HandleCooldown()
    {
        // Actualizar la barra de cooldown
        cooldownBar.fillAmount = currentTime / maxTime;

        // Si el boost está activo, incrementa el tiempo transcurrido
        if (isBoostActive)
        {
            currentTime += Time.deltaTime * 1000f; // Si deseas mantener la escala de ms
            if (currentTime >= maxTime)
            {
                currentTime = maxTime;
                isBoostActive = false;
                cooldownReady = true && AnyImageActive();  // El cooldown se “reactiva” (señal de que terminó)
                Debug.Log("Boost terminado.");
            }
        }
    }

    private void HandleBoost()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool canActivateBoost = !isBoostActive && currentTime >= maxTime && AnyImageActive();
            if (canActivateBoost)
            {
                ActivateBoost();
            }
        }
    }

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

    private void ActivateBoost()
    {
        // Desactivar la primera imagen activa que encontremos
        foreach (var image in boostImages)
        {
            if (image.gameObject.activeSelf)
            {
                image.gameObject.SetActive(false);
                break;
            }
        }

        // Reiniciar el cooldown y activar el boost
        isBoostActive = true;
        currentTime = 0f;
        Debug.Log("Boost activado.");
    }

    private void ActivateNextImage()
    {
        for (int i = boostImages.Length - 1; i >= 0; i--)
        {
            if (!boostImages[i].gameObject.activeSelf)
            {
                boostImages[i].gameObject.SetActive(true);
                Debug.Log("Imagen activada.");
                return;
            }
        }
        Debug.Log("No se pueden activar más imágenes; todas están activas.");
    }

    public bool IsCooldownReady()
    {
        bool wasReady = cooldownReady;
        if (cooldownReady)
        {
            cooldownReady = false; // Consumir la notificación
        }
        return wasReady;
    }
}
