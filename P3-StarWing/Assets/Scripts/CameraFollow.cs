using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // L'objecte que la c�mera ha de seguir (la nau)
    public float distance = 3f; // Dist�ncia des de la nau
    public float height = 1f; // Al�ada de la c�mera sobre la nau
    public float smoothSpeed = 0.125f; // Velocitat de suavitzat per al moviment de la c�mera

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

            transform.LookAt(target); // Fa que la c�mera sempre miri cap a la nau
        }
    }
}

