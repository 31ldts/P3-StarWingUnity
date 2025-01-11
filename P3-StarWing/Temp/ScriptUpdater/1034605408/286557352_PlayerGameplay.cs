using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGameplay : MonoBehaviour
{
    public float movementSpeed = 8f;
    public float initialSpeed = 0f;
    public float acceleration = 3f;
    public float deceleration = 2f;
    public float maxSpeed = 20f;
    public float minSpeed = 2.0f;

    public float rotationSpeed = 10f;
    public float lerpSpeed = 2f;

    public ParticleSystem accelerationParticles;
    private Quaternion targetRotation;

    private ProjectileShooter projectileShooter;
    public Transform singleFirePoint;
    public Transform[] doubleFirePoint;
    private bool isDoubleShot = false;

    public GameObject bombPrefab;
    public Transform bombSpawnPoint;

    private CooldownLogic cooldownLogic;

    void Start()
    {
        targetRotation = transform.rotation;
        initialSpeed = movementSpeed;

        projectileShooter = GetComponent<ProjectileShooter>();
        cooldownLogic = Object.FindFirstObjectByType<CooldownLogic>();
    }

    void Update()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "XaviLevel2")
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
        }
        else
        {
            // Control de velocidad con teclas Q y E
            if (Input.GetKey(KeyCode.Q))
            {
                movementSpeed = Mathf.Max(minSpeed, movementSpeed - acceleration * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.E))
            {
                movementSpeed = Mathf.Min(maxSpeed, movementSpeed + acceleration * Time.deltaTime);
            }

            // Movimiento hacia adelante
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

            // Movimiento vertical y rotación de pitch (W y S)
            float rotateVertical = 0;
            if (Input.GetKey(KeyCode.W))
            {
                rotateVertical = -1;
                transform.Translate(Vector3.up * movementSpeed * 0.5f * Time.deltaTime); // Movimiento hacia arriba
            }
            if (Input.GetKey(KeyCode.S))
            {
                rotateVertical = 1;
                transform.Translate(Vector3.down * movementSpeed * 0.5f * Time.deltaTime); // Movimiento hacia abajo
            }

            // Movimiento horizontal y rotación de yaw (A y D)
            float rotateHorizontal = 0;
            if (Input.GetKey(KeyCode.A))
            {
                rotateHorizontal = -1;
                transform.Translate(Vector3.left * movementSpeed * 0.5f * Time.deltaTime); // Movimiento hacia la izquierda
            }
            if (Input.GetKey(KeyCode.D))
            {
                rotateHorizontal = 1;
                transform.Translate(Vector3.right * movementSpeed * 0.5f * Time.deltaTime); // Movimiento hacia la derecha
            }

            // Rotación de pitch y yaw combinada
            targetRotation *= Quaternion.Euler(rotateVertical * rotationSpeed * Time.deltaTime, 
                                               rotateHorizontal * rotationSpeed * Time.deltaTime, 0);

            // Aplicar rotación suavizada
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
        }

        // DISPAR DEL PROJECTIL - BOTÓ ESQUERRA DEL RATOLÍ
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

        // LLANÇAR BOMBA - BOTÓ ESPAI
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropBomb();
        }
    }

    private void DropBomb()
    {
        if (cooldownLogic != null && cooldownLogic.IsCooldownReady())
        {
            if (bombPrefab != null && bombSpawnPoint != null)
            {
                GameObject bomb = Instantiate(bombPrefab, bombSpawnPoint.position, Quaternion.identity);
                Rigidbody rb = bomb.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.down; // Aplicar fuerza hacia abajo
                }
            }
        }
        else
        {
            Debug.Log("No puedes lanzar una bomba, el cooldown no está listo.");
        }
    }

    public void SetDoubleShotMode(bool enable)
    {
        isDoubleShot = enable;
        Debug.Log(isDoubleShot ? "Dispar doble activat!" : "Dispar únic restaurat.");
    }
}
