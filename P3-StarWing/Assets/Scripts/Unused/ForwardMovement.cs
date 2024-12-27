using UnityEngine;

public class ForwardMovement : MonoBehaviour
{
    // Velocidad del movimiento
    public float speed = 3.0f;

    void Update()
    {
        // Mover el objeto hacia adelante (en dirección a su eje local forward)
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}