using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 4f;
    public float initialSpeed = 0f;
    public float acceleration = 3f;
    public float deceleration = 2f;
    public float maxSpeed = 10f;
    public float minSpeed = 2.0f;

    public float rotationSpeed = 10f;
    public float lerpSpeed = 2f;

    private Quaternion targetRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetRotation = transform.rotation; // Inicialitza la rotació objectiu
        initialSpeed = movementSpeed; // Guardem la velocitat inicial
    }

    // Update is called once per frame
    void Update()
    {
        // Moviment cap endavant per defecte
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

        // Acceleració
        if (Input.GetKey(KeyCode.W))
        {
            movementSpeed += acceleration * Time.deltaTime; // Augmenta la velocitat
            movementSpeed = Mathf.Clamp(movementSpeed, minSpeed, maxSpeed); // Limita la velocitat dins dels marges
        }

        // Frenada/desacceleració
        else if (Input.GetKey(KeyCode.S))
        {
            movementSpeed -= deceleration * Time.deltaTime; // Redueix la velocitat
            movementSpeed = Mathf.Clamp(movementSpeed, minSpeed, maxSpeed); // Limita perquè no es mogui enrere ni s'aturi per complet
        }

        else // En cas de deixar d'accelerar/frenar hem de tornar a la velocitat inicial
        {
            if (movementSpeed > initialSpeed)
            {
                movementSpeed -= 4f * Time.deltaTime;
            }
            else if (movementSpeed < initialSpeed)
            {
                movementSpeed += 1f * Time.deltaTime;
            }
        }


        // Moviment cap a l'esquerra/dreta (rotació horitzontal - yaw)
        float rotateHorizontal = 0;
        if (Input.GetKey(KeyCode.A)) // Gir a l'esquerra (yaw)
        {
            rotateHorizontal = -1;
            transform.Translate(Vector3.left * movementSpeed * 0.1f * Time.deltaTime); // lleuger desplaçament cap a l'esquerra
            targetRotation *= Quaternion.Euler(0, 0, rotationSpeed * 0.5f * Time.deltaTime); // lleugera rotació roll per donar efecte
        }
        if (Input.GetKey(KeyCode.D)) // Gir a la dreta (yaw)
        {
            rotateHorizontal = 1;
            transform.Translate(Vector3.right * movementSpeed * 0.1f * Time.deltaTime); // lleuger desplaçament cap a la dreta
            targetRotation *= Quaternion.Euler(0, 0, -rotationSpeed * 0.5f * Time.deltaTime); // lleugera rotació roll per donar efecte
        }
        if (rotateHorizontal != 0)
        {
            targetRotation *= Quaternion.Euler(0, rotateHorizontal * rotationSpeed * Time.deltaTime, 0);
        }

        // Rotació vertical - pitch (pujar/baixar)
        if (Input.GetKey(KeyCode.UpArrow))
        {
            targetRotation *= Quaternion.Euler(-rotationSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            targetRotation *= Quaternion.Euler(rotationSpeed * Time.deltaTime, 0, 0);
        }

        // Rotació de roll ràpida ("barrell roll")
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            targetRotation *= Quaternion.Euler(0, 0, rotationSpeed * 20 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            targetRotation *= Quaternion.Euler(0, 0, -rotationSpeed * 20 * Time.deltaTime);
        }

        // Apliquem rotació suavitzada amb Lerp
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
    }
}
