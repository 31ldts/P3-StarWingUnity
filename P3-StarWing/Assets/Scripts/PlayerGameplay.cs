using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Necesario para IEnumerator y coroutines


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
    Rigidbody rb;

    private ProjectileShooter projectileShooter;
    public Transform singleFirePoint;
    public Transform[] doubleFirePoint;
    private bool isDoubleShot = false;

    public GameObject bombPrefab;
    public Transform bombSpawnPoint;

    private CooldownLogic cooldownLogic;
    public Transform cameraTransform;  // Referencia a la cámara
    public Vector3 cameraOffset; // Offset de la cámara con respecto a la nave

    public float followSpeed = 7f; // Velocidad de seguimiento de la cámara
    public float returnToZeroSpeed = 1.5f; // Velocidad de retorno a 0°
    public float verticalRotationLimit = 15f; // Límite de rotación vertical (pitch)
    public float horizontalRotationLimit = 45f; // Aumentar límite de rotación horizontal
    float rotateHorizontal;
    float rotateVertical;
    bool isRolling = false;
    string lastKey = ""; // Variable para registrar la última tecla pulsada ('A' o 'D')
    public float transportBackDistance = 15f; // Distancia hacia atrás para transportar la nave
    public float resetDuration = 0.5f; // Duración del ajuste de la orientación



    void Start()
    {
        targetRotation = transform.rotation;
        initialSpeed = movementSpeed;
        rb = GetComponent<Rigidbody>();
        projectileShooter = GetComponent<ProjectileShooter>();
        cooldownLogic = Object.FindFirstObjectByType<CooldownLogic>();
        cameraOffset = cameraTransform.position - transform.position;
    }

    void Update()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "XaviLevel2")
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            // Activar isKinematic
            rb.isKinematic = true;  // Esto desactiva la física y permite moverlo manualmente
            rotationSpeed = 1600f;
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
            float returnToNeutralSpeed = 2f; // Velocidad de retorno a la rotación neutral

            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.Q))
            {
                movementSpeed = Mathf.Max(minSpeed, movementSpeed - acceleration * Time.deltaTime);
                accelerationParticles.Stop();
            }
            else if (Input.GetKey(KeyCode.E))
            {
                movementSpeed = Mathf.Min(maxSpeed, movementSpeed + acceleration * Time.deltaTime);
                accelerationParticles.Play();
            }
            else
            {
                if (movementSpeed > initialSpeed)
                {
                    movementSpeed -= 4f * Time.deltaTime;
                }
                else
                {
                    movementSpeed += 1f * Time.deltaTime;
                }
                accelerationParticles.Stop();
            }

            float rotateVertical = 0;
            if (Input.GetKey(KeyCode.W))
            {
                rotateVertical = -1;
                transform.Translate(Vector3.up * movementSpeed * 0.5f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rotateVertical = 1;
                transform.Translate(Vector3.down * movementSpeed * 0.5f * Time.deltaTime);
            }

            float rotateHorizontal = 0;
            if (Input.GetKey(KeyCode.A))
            {
                lastKey = "A";
                rotateHorizontal = -1;
                transform.Translate(Vector3.left * movementSpeed * 0.5f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                lastKey = "D";
                rotateHorizontal = 1;
                transform.Translate(Vector3.right * movementSpeed * 0.5f * Time.deltaTime);
            }

            // Variables para controlar la inclinación
            float rollAngle = 15f; // Ángulo de inclinación máxima
            float rollSpeed = 5f;  // Velocidad de interpolación para la inclinación

            // Inclinación en el eje Z según el movimiento horizontal
            float targetRoll = 0f;
            if (rotateHorizontal != 0)
            {
                targetRoll = rotateHorizontal * -rollAngle;
            }

            Quaternion targetZRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, targetRoll);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetZRotation, rollSpeed * Time.deltaTime);

            // Rotación en el eje Y (yaw)
            if (rotateHorizontal != 0)
            {
                targetRotation *= Quaternion.Euler(0, rotateHorizontal * rotationSpeed * Time.deltaTime, 0);
                float yaw = targetRotation.eulerAngles.y;
                yaw = Mathf.Clamp(Mathf.DeltaAngle(0, yaw), -horizontalRotationLimit, horizontalRotationLimit);
                targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, yaw, targetRotation.eulerAngles.z);
            }
            else
            {
                Quaternion zeroYawRotation = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z);
                targetRotation = Quaternion.Lerp(transform.rotation, zeroYawRotation, returnToZeroSpeed * Time.deltaTime);
            }

            // Rotación en el eje X (pitch)
            if (rotateVertical != 0)
            {
                targetRotation *= Quaternion.Euler(rotateVertical * rotationSpeed * Time.deltaTime, 0, 0);
                float pitch = targetRotation.eulerAngles.x;
                pitch = Mathf.Clamp(Mathf.DeltaAngle(0, pitch), -verticalRotationLimit, verticalRotationLimit);
                targetRotation = Quaternion.Euler(pitch, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
            if (Input.GetKeyDown(KeyCode.Space) && !isRolling)
            {
                StartCoroutine(PerformBarrelRoll());
            }

            // Verificar si no se presionan teclas de movimiento para retornar a la orientación adecuada en X y Z
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                float currentYRotation = transform.eulerAngles.y; // Conservar la rotación en Y
                Quaternion targetRotation = Quaternion.Euler(0, currentYRotation, 0); // Rotación neutra en X y Z

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * returnToNeutralSpeed);
            }

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
    IEnumerator PerformBarrelRoll()
    {
        isRolling = true;

        // Determina la dirección del barrel roll basado en la última tecla pulsada
        float direction = (lastKey == "D") ? -1f : 1f;
        yield return StartCoroutine(BarrelRoll(1.0f, direction));

        isRolling = false;
    }

    IEnumerator BarrelRoll(float duration, float direction)
    {
        float startRotation = transform.eulerAngles.z;
        float endRotation = startRotation + (360.0f * direction);
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float zRotation = Mathf.Lerp(startRotation, endRotation, elapsed / duration) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startRotation);
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Emtrouu2.");
        if (collision.gameObject.CompareTag("Wall")) // Verifica si el objeto tiene el tag "Wall"
        {
            Debug.Log("Emtrouu.");
            // Transporta la nave hacia atrás
            Vector3 backDirection = -transform.forward;
            transform.position += backDirection * transportBackDistance;
            cameraTransform.position = transform.position + cameraOffset;
            // Restablece la orientación de la nave
            Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            StartCoroutine(ResetRotation(targetRotation, resetDuration));
        }
    }
    IEnumerator ResetRotation(Quaternion targetRotation, float duration)
    {
        float elapsed = 0f;
        Quaternion initialRotation = transform.rotation;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }
    public void SetDoubleShotMode(bool enable)
    {
        isDoubleShot = enable;
        Debug.Log(isDoubleShot ? "Dispar doble activat!" : "Dispar únic restaurat.");
    }
}
