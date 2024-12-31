using UnityEngine;

public class EnemyPatrolArea : MonoBehaviour
{
    public Vector3 areaCenter; // Centre de l'�rea de patrulla
    public Vector3 areaSize;   // Dimensions de l'�rea
    public float speed = 2f;   // Velocitat de moviment
    public float rotationSpeed = 5f; // Velocitat de rotaci�

    private Vector3 targetPosition;
    private Vector3 moveDirection; // Direcci� del moviment

    private void Start()
    {
        // Establir la direcci� inicial de moviment (pot ser qualsevol direcci�)
        moveDirection = Vector3.right; // Mou de l'esquerra a la dreta per exemple
        SetTargetPosition();
    }

    private void Update()
    {
        // Mou l'enemic en la direcci� actual
        transform.position += moveDirection * speed * Time.deltaTime;

        // Comprovar si l'enemic ha arribat al final de l'�rea
        if (!IsWithinArea(transform.position))
        {
            // Canvia la direcci�
            moveDirection = -moveDirection;
            SetTargetPosition(); // Recalcula la nova posici� del target quan canvia la direcci�
        }

        // Fer que l'enemic giri cap a la direcci� del moviment
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void SetTargetPosition()
    {
        // Establir la nova posici� de target segons el moviment en l�nia recta dins l'�rea
        float targetX = moveDirection.x > 0 ? areaCenter.x + areaSize.x / 2 : areaCenter.x - areaSize.x / 2;
        float targetZ = areaCenter.z; // Fer que l'enemic es desplaci nom�s en X (tamb� podries fer-ho en Z si vols un altre eix)
        targetPosition = new Vector3(targetX, transform.position.y, targetZ);
    }

    private bool IsWithinArea(Vector3 position)
    {
        // Comprovar si l'enemic est� dins l'�rea del collider
        return position.x >= areaCenter.x - areaSize.x / 2 &&
               position.x <= areaCenter.x + areaSize.x / 2 &&
               position.z >= areaCenter.z - areaSize.z / 2 &&
               position.z <= areaCenter.z + areaSize.z / 2;
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuixar l'�rea de patrullatge al editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}
