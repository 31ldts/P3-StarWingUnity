using UnityEngine;

public class EnemyUpAndDown : MonoBehaviour
{
    public float movementRange = 3f; // Rang m�xim de moviment vertical
    public float movementSpeed = 2f; // Velocitat de moviment vertical
    public float rotationSpeed = 100f; // Velocitat de rotaci�

    private Vector3 startPosition; // Posici� inicial de l'enemic
    private bool movingUp = true; // Direcci� inicial del moviment

    private void Start()
    {
        // Desa la posici� inicial
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
                movingUp = false; // Canvia la direcci� cap avall
            }
        }
        else
        {
            transform.position += Vector3.down * movement;
            if (transform.position.y <= startPosition.y - movementRange)
            {
                movingUp = true; // Canvia la direcci� cap amunt
            }
        }

        // Rotar sobre s� mateix
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}

