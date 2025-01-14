using System.Collections;
using UnityEngine;

public class BossLogic : MonoBehaviour
{
    // MOVIMENT 
    public float movementSpeed; //Moviment en Z
    public float moveSpeed; //Moviment XY
    public float lookSpeed = 5f;
    private PlayerGameplay playerGameplay; // Referencia al script PlayerGameplay

    private InfinityMovement infinityMovement;



    // RAIG DESRTRUCTOR

    public AudioSource beamAudioSource; // Referencia al AudioSource del rayo
    public Transform player;
    public float detectionRange = 100f;
    public LineRenderer lineRenderer;   // LineRenderer que simula el raig
    public float beamDuration = 1.5f;   // Durada del raig (en segons)
    public float beamCooldown = 2f;     // Temps entre rajos
    public float beamDamage = 100f;      // Quantitat de dany que infligeix el raig

    public float beamTrackingDelay = 0.1f; // Temps entre actualitzacions del seguiment del raig
    public float aimOffset = 1f;
    private float timer = 0f; // Acumulador de tiempo

    //public LayerMask damageLayer;       // Capes que poden rebre dany

    private float nextFireTime;
    private bool isFiring;
    private Vector3 targetPosition;  // Posici� objectiu del moviment

    private Vector3 targetRandom;  // Posici� objectiu del moviment


    void Start()
    { 
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
        lineRenderer.enabled = false; // Assegura que el raig est� desactivat inicialment
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Obtén el componente PlayerGameplay del jugador
        if (player != null)
        {
            playerGameplay = player.GetComponent<PlayerGameplay>();
        }
        InfinityMovement infinityMovement = GetComponent<InfinityMovement>();
        if (infinityMovement == null)
        {
            Debug.Log("q es esto");
            infinityMovement = gameObject.AddComponent<InfinityMovement>();
        }
        if (beamAudioSource == null)
        {
            beamAudioSource = GetComponent<AudioSource>();
        }

        // Configura los parámetros del movimiento en infinito
        infinityMovement.speed = 1f;
        infinityMovement.width = 20f;
        infinityMovement.height = 12f;
        infinityMovement.zSpeed = movementSpeed; // Usa la velocidad de alejamiento configurada en el boss
        infinityMovement.smoothness = 10f;

        // Asigna la referencia al jugador
        infinityMovement.player = player.transform;
        infinityMovement.zSpeed = movementSpeed;
        
    }

    void Update()
    {
            // Accede al componente infinityMovement en el Update
        infinityMovement = GetComponent<InfinityMovement>();
        movementSpeed = playerGameplay.movementSpeed+1;   //Sadapta a la velocitat del player
        infinityMovement.zSpeed = movementSpeed;
        
        // Acumular el tiempo transcurrido
        timer += Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        //MoveTowardsPosition();

        if (distanceToPlayer < detectionRange)
        {
            // Seguiment del jugador
            LookAtPlayer();
            

            if (timer >= 4)     //Cada 4 segons moverse en infinito XY
            {
                infinityMovement.enabled = true;
                if(timer >= 7){
                    infinityMovement.enabled = false;
                    timer = 0;
                } 
            } else {
                Debug.Log("memue");
                MoveAwayFromPlayer();
            }
            

            // Atac
            if (Time.time >= nextFireTime && !isFiring)
            {
                Debug.Log("DISPARO DELIBRO SOCIO");
                StartCoroutine(FireBeam());
            }
        }
    }
    public void ActiveBoss(bool active)
    {
        Debug.Log("YEEEE");
        gameObject.SetActive(active);
    }

    private void MoveAwayFromPlayer()
    {
        // Calcular la dirección desde el jugador hacia el jefe
        Vector3 direction = (transform.position - player.position).normalized;

        // Asegurarse de que el movimiento sea solo en el eje Z
        direction = new Vector3(0, 0, direction.z);

        // Mover al jefe usando movementSpeed
        transform.position += direction * movementSpeed * Time.deltaTime;
    }

    private void LookAtPlayer()
    {
        // Gira el boss cap al jugador lentament
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
    }

    private IEnumerator FireBeam()
    {
        isFiring = true;
        if (beamAudioSource != null)
        {
            beamAudioSource.Play();
        }
        // Activar el raig
        lineRenderer.enabled = true;

        // Duraci� activa del raig
        float elapsedTime = 0f;

        while (elapsedTime < beamDuration)
        {
            // Configurar el LineRenderer per mostrar el raig
            lineRenderer.SetPosition(0, transform.position); // Inici del raig

            // Crear una posici� amb lleugera desviaci�
            Vector3 targetPosition = player.position + new Vector3(
                Random.Range(-aimOffset, aimOffset),
                Random.Range(-aimOffset, aimOffset),
                Random.Range(-aimOffset, aimOffset)
            );

            lineRenderer.SetPosition(1, targetPosition); // Direcci� lleugerament despla�ada cap al jugador

            // Aplica dany al jugador si el raig el colpeja
            if (Physics.Raycast(transform.position, (targetPosition - transform.position).normalized, out RaycastHit hit, Mathf.Infinity)) //, damageLayer))
            {
                LifeComponent lifeComponent = hit.collider.GetComponent<LifeComponent>();
                if (lifeComponent != null)
                {
                    lifeComponent.doDamage(beamDamage * 5 *Time.deltaTime);
                }
            }

            elapsedTime += beamTrackingDelay;
            yield return new WaitForSeconds(beamTrackingDelay); // Fes un seguiment retardat
        }


        // Desactiva el raig
        lineRenderer.enabled = false;
        nextFireTime = Time.time + beamCooldown;
        isFiring = false;
    }
}


public class InfinityMovement : MonoBehaviour
{
    public float speed = 0.5f;           // Velocidad del movimiento en infinito
    public float width = 5f;          // Amplitud del eje X
    public float height = 3f;         // Amplitud del eje Y
    public float zSpeed = 2f;         // Velocidad de alejamiento en el eje Z
    public float smoothness = 5f;     // Suavidad del movimiento

    private float time = 0f;          // Temporizador para el movimiento
    public Transform player;          // Referencia al jugador

    void Update()
    {
        // Incrementar el tiempo según la velocidad
        time += speed * Time.deltaTime;

        // Movimiento en forma de infinito en XY
        float x = Mathf.Sin(time) * width;
        float y = Mathf.Sin(2 * time) / 2 * height;

        // Alejarse del jugador en Z
        float z = transform.position.z - zSpeed * Time.deltaTime;

        // Actualizar la posición suavemente
        Vector3 targetPosition = new Vector3(x, y, z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothness);
    }
    void OnEnable()
    {
        // Lógica que se ejecuta al habilitar el componente
        Debug.Log("InfinityMovement activado");
    }

    void OnDisable()
    {
        // Lógica que se ejecuta al deshabilitar el componente
        Debug.Log("InfinityMovement desactivado");
    }
}

