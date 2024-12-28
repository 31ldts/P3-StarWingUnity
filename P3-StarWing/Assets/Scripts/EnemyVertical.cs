using UnityEngine;

public class EnemyUpAndDown : MonoBehaviour
{
    public float movementRange = 3f; // Rang màxim de moviment vertical
    public float movementSpeed = 2f; // Velocitat de moviment vertical
    public float rotationSpeed = 100f; // Velocitat de rotació

    private Vector3 startPosition; // Posició inicial de l'enemic
    private bool movingUp = true; // Direcció inicial del moviment

    private void Start()
    {
        // Desa la posició inicial
        startPosition = transform.position;
    }

    private void Update()
    {
        // Mou cap amunt i cap avall
        float movement = movementSpeed * Time.deltaTime;

        if (movingUp)
        {
            transform.position += Vector3.up * movement;
            if (transform.position.y >= startPosition.y + movementRange)
            {
                movingUp = false; // Canvia la direcció cap avall
            }
        }
        else
        {
            transform.position += Vector3.down * movement;
            if (transform.position.y <= startPosition.y - movementRange)
            {
                movingUp = true; // Canvia la direcció cap amunt
            }
        }

        // Rotar sobre sí mateix
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}

