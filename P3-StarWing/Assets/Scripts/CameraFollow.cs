using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // L'objecte que la càmera ha de seguir (la nau)
    public float distance = 3f; // Distància des de la nau
    public float height = 1f; // Alçada de la càmera sobre la nau
    public float smoothSpeed = 0.125f; // Velocitat de suavitzat per al moviment de la càmera

    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    private void LateUpdate()
    {
        if (target != null)
        {

            Vector3 desiredPosition = target.position + Vector3.up * height - target.forward * distance;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(target); // Fa que la càmera sempre miri cap a la nau
        }
    }
}

