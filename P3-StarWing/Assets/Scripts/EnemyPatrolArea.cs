using UnityEngine;

public class EnemyPatrolArea : MonoBehaviour
{
    public Vector3 areaCenter; // Centre de l'àrea de patrulla
    public Vector3 areaSize;   // Dimensions de l'àrea
    public float speed = 2f;   // Velocitat de moviment
    public float rotationSpeed = 5f; // Velocitat de rotació

    private Vector3 targetPosition;
    private Vector3 moveDirection; // Direcció del moviment

    private void Start()
    {
        // Establir la direcció inicial de moviment (pot ser qualsevol direcció)
        moveDirection = Vector3.right; // Mou de l'esquerra a la dreta per exemple
        SetTargetPosition();
    }

    private void Update()
    {
        // Mou l'enemic en la direcció actual
        transform.position += moveDirection * speed * Time.deltaTime;

        // Comprovar si l'enemic ha arribat al final de l'àrea
        if (!IsWithinArea(transform.position))
        {
            // Canvia la direcció
            moveDirection = -moveDirection;
            SetTargetPosition(); // Recalcula la nova posició del target quan canvia la direcció
        }

        // Fer que l'enemic giri cap a la direcció del moviment
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void SetTargetPosition()
    {
        // Establir la nova posició de target segons el moviment en línia recta dins l'àrea
        float targetX = moveDirection.x > 0 ? areaCenter.x + areaSize.x / 2 : areaCenter.x - areaSize.x / 2;
        float targetZ = areaCenter.z; // Fer que l'enemic es desplaci només en X (també podries fer-ho en Z si vols un altre eix)
        targetPosition = new Vector3(targetX, transform.position.y, targetZ);
    }

    private bool IsWithinArea(Vector3 position)
    {
        // Comprovar si l'enemic està dins l'àrea del collider
        return position.x >= areaCenter.x - areaSize.x / 2 &&
               position.x <= areaCenter.x + areaSize.x / 2 &&
               position.z >= areaCenter.z - areaSize.z / 2 &&
               position.z <= areaCenter.z + areaSize.z / 2;
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuixar l'àrea de patrullatge al editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}
