using UnityEngine;

public class PlayerGameplay : MonoBehaviour
{
    // Atributs per al moviment i rotació
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

    // Atributs per al dispar de projectils
    private ProjectileShooter projectileShooter;
    public Transform singleFirePoint;
    public Transform[] doubleFirePoint;
    private bool isDoubleShot = false;

    // Atributs per al llançament de bombes
    public GameObject bombPrefab;
    public Transform bombSpawnPoint;

    // Atributs per al cooldown
    private CooldownLogic cooldownLogic;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetRotation = transform.rotation; // Inicialitza la rotació objectiu
        initialSpeed = movementSpeed; // Guardem la velocitat inicial

        projectileShooter = GetComponent<ProjectileShooter>();
        //accelerationParticles = GetComponent<ParticleSystem>();
        cooldownLogic = Object.FindFirstObjectByType<CooldownLogic>(); // Busca la lógica de cooldown en la escena
    }

    // Update is called once per frame
    void Update()
    {
        // Moviment cap endavant per defecte
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

        // Acceleració
        if (Input.GetKey(KeyCode.W))
        {
            movementSpeed += acceleration * Time.deltaTime; // Augmenta la velocitat
            movementSpeed = Mathf.Clamp(movementSpeed, minSpeed, maxSpeed); // Limita la velocitat dins dels marges

            accelerationParticles.Play(); // Iniciem el sistema de partícules que representa l'acceleració
        }

        // Frenada/desacceleració
        else if (Input.GetKey(KeyCode.S))
        {
            movementSpeed -= deceleration * Time.deltaTime; // Redueix la velocitat
            movementSpeed = Mathf.Clamp(movementSpeed, minSpeed, maxSpeed); // Limita perquè no es mogui enrere ni s'aturi per complet
        }

        else // En cas de deixar d'accelerar/frenar hem de tornar a la velocitat inicial
        {
            if (movementSpeed > initialSpeed)
            {
                movementSpeed -= 4f * Time.deltaTime;
            }
            else if (movementSpeed < initialSpeed)
            {
                movementSpeed += 1f * Time.deltaTime;
            }

            accelerationParticles.Stop();
        }



        // Moviment cap a l'esquerra/dreta (rotació horitzontal - yaw)
        float rotateHorizontal = 0;
        if (Input.GetKey(KeyCode.A)) // Gir a l'esquerra (yaw)
        {
            rotateHorizontal = -1;
            transform.Translate(Vector3.left * movementSpeed * 0.25f * Time.deltaTime); // lleuger desplaçament cap a l'esquerra
            targetRotation *= Quaternion.Euler(0, 0, rotationSpeed * 0.5f * Time.deltaTime); // lleugera rotació roll per donar efecte
        }
        if (Input.GetKey(KeyCode.D)) // Gir a la dreta (yaw)
        {
            rotateHorizontal = 1;
            transform.Translate(Vector3.right * movementSpeed * 0.25f * Time.deltaTime); // lleuger desplaçament cap a la dreta
            targetRotation *= Quaternion.Euler(0, 0, -rotationSpeed * 0.5f * Time.deltaTime); // lleugera rotació roll per donar efecte
        }
        if (rotateHorizontal != 0)
        {
            targetRotation *= Quaternion.Euler(0, rotateHorizontal * rotationSpeed * Time.deltaTime, 0);
        }

        // Rotació vertical - pitch (pujar/baixar)
        if (Input.GetKey(KeyCode.UpArrow))
        {
            targetRotation *= Quaternion.Euler(-rotationSpeed * 1.25f * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            targetRotation *= Quaternion.Euler(rotationSpeed * 1.25f * Time.deltaTime, 0, 0);
        }

        // Rotació de roll ràpida ("barrell roll")
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            targetRotation *= Quaternion.Euler(0, 0, rotationSpeed * 20 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            targetRotation *= Quaternion.Euler(0, 0, -rotationSpeed * 20 * Time.deltaTime);
        }

        // Apliquem rotació suavitzada amb Lerp
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);


        // DISPAR DEL PROJECTIL - BOTÓ ESQUERRA DEL RATOLÍ
        if (Input.GetMouseButtonDown(0)) 
        {
            if (!isDoubleShot)
            {
                projectileShooter.firePoint = singleFirePoint;
                projectileShooter.ShootProjectile(true);
            } else
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
                // Instanciar la bomba al punto designado
                GameObject bomb = Instantiate(bombPrefab, bombSpawnPoint.position, Quaternion.identity);

                // Añadir fuerza inicial a la bomba (opcional)
                Rigidbody rb = bomb.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.down; // * bombDropForce;
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

    /*public float detectionRange = 10f; // Radi del rang de detecció
    public Color gizmoColor = Color.blue; // Color de la esfera

    private void OnDrawGizmosSelected()
    {
        // Canviar el color del Gizmo
        Gizmos.color = gizmoColor;

        // Dibuixar una esfera a la posició de l'objecte amb el radi definit
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }*/
}
