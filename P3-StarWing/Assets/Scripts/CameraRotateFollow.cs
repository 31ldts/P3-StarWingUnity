using UnityEngine;

public class CameraRotateFollow : MonoBehaviour
{
    public Transform turret;           // Referencia a la torreta
    public Transform cameraTransform;  // Referencia a la cámara
    public float rotationSpeed = 5f;   // Velocidad de rotación de la torreta
    public float followSpeed = 2f;     // Velocidad de seguimiento de la cámara

    void Update()
    {
        // Controlar la rotación de la torreta (ejemplo con input del mouse)
        float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed;
        turret.Rotate(0f, horizontalInput, 0f);

        // Hacer que la cámara siga la rotación de la torreta suavemente
        FollowTurretRotation();
    }

    void FollowTurretRotation()
    {
        // Obtener la rotación objetivo de la cámara basada en la rotación de la torreta
        Quaternion targetRotation = Quaternion.Euler(0f, turret.eulerAngles.y, 0f);

        // Interpolar suavemente hacia la rotación objetivo
        cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, targetRotation, followSpeed * Time.deltaTime);
    }
}
