using UnityEngine;

public class PlayerMovementSimple : MonoBehaviour
{
    public float movementSpeed = 1f;
    public float rotationSpeed = 10f;

    // Update is called once per frame
    /*void Update()
    {
        Vector3 movement = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        transform.position += movement * movementSpeed * Time.deltaTime;
    }*/

    private void Update()
    {
        // Moviment cap endavant/enrere
        float moveForward = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * moveForward);


        // Moviment cap a l'esquerra/dreat (rotaci� horitzontal - yaw)
        float rotateHorizontal = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotateHorizontal);


        // Rotaci� vertical - pitch (pujar/baixar)
        if (Input.GetKey(KeyCode.UpArrow)) // Rotar amunt
        {
            transform.Rotate(Vector3.left, rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow)) // Rotar avall
        {
            transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        }


        // Rotaci� de roll (inclinaci� esquerra/dreta)
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
        }
    }
}
