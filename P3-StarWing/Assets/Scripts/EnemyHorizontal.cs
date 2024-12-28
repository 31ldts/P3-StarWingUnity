using UnityEngine;

public class EnemySideToSide : MonoBehaviour
{
    public float movementRange = 5f; // Amplada m�xima del moviment
    public float speed = 2f; // Velocitat del moviment

    private Vector3 startPosition; // Posici� inicial de l'enemic
    private bool movingRight = true; // Direcci� inicial del moviment

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Mou l'objecte cap a un costat o cap a l'altre
        float movement = speed * Time.deltaTime;

        if (movingRight)
        {
            transform.position += Vector3.right * movement;
            if (transform.position.x >= startPosition.x + movementRange)
            {
                movingRight = false; // Canvia la direcci� a l'esquerra
            }
        }
        else
        {
            transform.position += Vector3.left * movement;
            if (transform.position.x <= startPosition.x - movementRange)
            {
                movingRight = true; // Canvia la direcci� a la dreta
            }
        }
    }
}

