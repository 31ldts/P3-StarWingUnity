using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    // Atributos para el movimiento y rotación
    public float movementSpeed = 8f;
    public float initialSpeed = 0f;
    public float acceleration = 3f;
    public float deceleration = 2f;
    public float maxSpeed = 20f;
    public float minSpeed = 2.0f;

    public float rotationSpeed = 10f;
    public float followSpeed = 7f; // Velocidad de seguimiento de la cámara

    public float lerpSpeed = 2f;

    public ParticleSystem accelerationParticles;
    private Quaternion targetRotation;

    // Atributos para el disparo de proyectiles
    private ProjectileShooter projectileShooter;
    public Transform singleFirePoint;
    public Transform cameraTransform;  // Referencia a la cámara

    public Transform[] doubleFirePoint;
    private bool isDoubleShot = false;

    // Atributos para el lanzamiento de bombas
    public GameObject bombPrefab;
    public Transform bombSpawnPoint;

    // Atributos para el cooldown
    private CooldownLogic cooldownLogic;

    void Start()
    {
        targetRotation = transform.rotation; // Inicializa la rotación objetivo

        projectileShooter = GetComponent<ProjectileShooter>();
        cooldownLogic = Object.FindFirstObjectByType<CooldownLogic>(); // Busca la lógica de cooldown en la escena
    }

    void Update()
    {
        // Movilidad horizontal (Yaw - rotación sobre el eje Y)
        float rotateHorizontal = 0;
        if (Input.GetKey(KeyCode.A)) // Girar a la izquierda
        {
            rotateHorizontal = -1;
        }
        if (Input.GetKey(KeyCode.D)) // Girar a la derecha
        {
            rotateHorizontal = 1;
        }

        // Movilidad vertical (Pitch - rotación sobre el eje X)
        float rotateVertical = 0;
        if (Input.GetKey(KeyCode.W)) // Girar hacia arriba
        {
            rotateVertical = -1;
        }
        if (Input.GetKey(KeyCode.S)) // Girar hacia abajo
        {
            rotateVertical = 1;
        }

        // Obtener las rotaciones actuales de X e Y en el rango de -180 a 180
        float currentRotationX = Mathf.DeltaAngle(0, transform.rotation.eulerAngles.x); // Normaliza X entre -180 y 180
        float currentRotationY = Mathf.DeltaAngle(0, transform.rotation.eulerAngles.y); // Normaliza Y entre -180 y 180

        // Limitar la rotación de X e Y en los rangos deseados
        float newRotationX = Mathf.Clamp(currentRotationX + rotateVertical * rotationSpeed * Time.deltaTime, -15, 15); // Limitar rotación vertical (pitch)
        float newRotationY = Mathf.Clamp(currentRotationY + rotateHorizontal * rotationSpeed * Time.deltaTime, -30, 30); // Limitar rotación horizontal (yaw)

        // Recalcular la rotación objetivo con las nuevas rotaciones limitadas
        targetRotation = Quaternion.Euler(newRotationX, newRotationY, transform.rotation.eulerAngles.z); // Mantener Z igual

        // Interpolación para suavizar la rotación de la nave
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);

        // La cámara sigue la rotación de la nave con interpolación suave
        cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, transform.rotation, followSpeed * Time.deltaTime);

        // Disparo de proyectiles con el botón izquierdo del ratón
        if (Input.GetMouseButtonDown(0))
        {
            if (!isDoubleShot)
            {
                projectileShooter.firePoint = singleFirePoint;
                projectileShooter.ShootProjectile(true);
            }
            else
            {
                foreach (Transform firePoint in doubleFirePoint)
                {
                    projectileShooter.firePoint = firePoint;
                    projectileShooter.ShootProjectile(true);
                }
            }
        }
    }
}
