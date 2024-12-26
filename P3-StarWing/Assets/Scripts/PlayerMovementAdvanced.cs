using UnityEngine;

public class PlayerMovementAdvanced : MonoBehaviour
{
    public float movementSpeed = 1f;
    public float rotationSpeed = 10f;
    public float lerpSpeed = 2f;

    private Quaternion targetRotation;  // Rotació desitjada

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        targetRotation = transform.rotation; // Inicialitza la rotació objectiu
    }

    // Update is called once per frame
    void Update()
    {
        // Moviment cap endavant/enrere
        //float moveForward = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        //transform.Translate(Vector3.forward * moveForward);

        // Moviment cap endavant/enrere
        if (Input.GetKey(KeyCode.W)) // Moure endavant
        {
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S)) // Moure enrere
        {
            transform.Translate(Vector3.back * movementSpeed * Time.deltaTime);
        }

        // Moviment cap a l'esquerra/dreta (rotació horitzontal - yaw)
        /*float rotateHorizontal = Input.GetAxis("Horizontal");
        if (rotateHorizontal != 0)
        {
            // Actualitza la rotació objectiu per al yaw (gir horitzontal)
            targetRotation *= Quaternion.Euler(0, rotateHorizontal * rotationSpeed * Time.deltaTime, 0);
        }*/

        // Moviment cap a l'esquerra/dreta (rotació horitzontal - yaw)
        float rotateHorizontal = 0;
        if (Input.GetKey(KeyCode.A)) // Gir a l'esquerra (yaw)
        {
            rotateHorizontal = -1;
        }
        if (Input.GetKey(KeyCode.D)) // Gir a la dreta (yaw)
        {
            rotateHorizontal = 1;
        }
        // Aplicar la rotació horitzontal amb `targetRotation` (Yaw)
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

        // Rotació de roll (inclinació esquerra/dreta)
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            targetRotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            targetRotation *= Quaternion.Euler(0, 0, -rotationSpeed * Time.deltaTime);
        }

        // Aplica la rotació suavitzada amb Lerp
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
    }
}
