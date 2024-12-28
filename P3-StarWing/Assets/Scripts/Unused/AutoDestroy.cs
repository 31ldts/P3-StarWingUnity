using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 5f; // Tiempo de vida del objeto

    void Start()
    {
        // Destruir el objeto después del tiempo especificado
        Destroy(gameObject, lifetime);
    }
}