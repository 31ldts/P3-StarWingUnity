using UnityEngine;

public class PlayerMovementPhysic : MonoBehaviour
{
    public float thrust = 1f; // Força de moviment
    public float torque = 1f; // Força de rotació
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Evitar que caigui si no hi ha gravetat
    }

    private void FixedUpdate()
    {
        // Moviment cap endavant/enrere
        float moveForward = Input.GetAxis("Vertical") * thrust;
        rb.AddForce(transform.forward * moveForward, ForceMode.Acceleration);

        // Rotació horitzontal
        float rotateHorizontal = Input.GetAxis("Horizontal") * torque;
        rb.AddTorque(Vector3.up * rotateHorizontal, ForceMode.Acceleration);

        // Inclinació (Roll)
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(Vector3.forward * torque, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(Vector3.back * torque, ForceMode.Acceleration);
        }
    }
}
