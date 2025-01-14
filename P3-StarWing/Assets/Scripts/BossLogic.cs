using System.Collections;
using UnityEngine;

public class BossLogic : MonoBehaviour
{
    // MOVIMENT 
    public float movementSpeed = 8f;
    public float lookSpeed = 5f;

    // RAIG DESRTRUCTOR
    public Transform player;            
    public float detectionRange = 100f;
    public LineRenderer lineRenderer;   // LineRenderer que simula el raig
    public float beamDuration = 1.5f;   // Durada del raig (en segons)
    public float beamCooldown = 2f;     // Temps entre rajos
    public float beamDamage = 100f;      // Quantitat de dany que infligeix el raig

    public float beamTrackingDelay = 0.1f; // Temps entre actualitzacions del seguiment del raig
    public float aimOffset = 1f;

    //public LayerMask damageLayer;       // Capes que poden rebre dany

    private float nextFireTime;
    private bool isFiring;
    private Vector3 targetPosition;  // Posició objectiu del moviment

    void Start()
    { 
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
        lineRenderer.enabled = false; // Assegura que el raig està desactivat inicialment
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        //MoveTowardsPosition();

        if (distanceToPlayer < detectionRange)
        {
            // Seguiment del jugador
            LookAtPlayer();
            MoveAwayFromPlayer();

            // Atac
            if (Time.time >= nextFireTime && !isFiring)
            {
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
        // Direcció cap al jugador
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Direcció contrària (cap enrere)
        Vector3 retreatDirection = -directionToPlayer;

        // Moviment irregular al voltant de la direcció de retirada
        Vector3 randomOffset = new Vector3(
            Random.Range(0f, 4f),
            Random.Range(0f, 2f), // Opcional, per si vols que el moviment vertical sigui menor
            Random.Range(0f, 4f)
        ).normalized;

        Vector3 finalDirection = (retreatDirection + randomOffset).normalized;

        // Mou el boss
        transform.position += finalDirection * movementSpeed * Time.deltaTime;
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

        // Activar el raig
        lineRenderer.enabled = true;

        // Duració activa del raig
        float elapsedTime = 0f;

        while (elapsedTime < beamDuration)
        {
            // Configurar el LineRenderer per mostrar el raig
            lineRenderer.SetPosition(0, transform.position); // Inici del raig

            // Crear una posició amb lleugera desviació
            Vector3 targetPosition = player.position + new Vector3(
                Random.Range(-aimOffset, aimOffset),
                Random.Range(-aimOffset, aimOffset),
                Random.Range(-aimOffset, aimOffset)
            );

            lineRenderer.SetPosition(1, targetPosition); // Direcció lleugerament desplaçada cap al jugador

            // Aplica dany al jugador si el raig el colpeja
            if (Physics.Raycast(transform.position, (targetPosition - transform.position).normalized, out RaycastHit hit, Mathf.Infinity)) //, damageLayer))
            {
                LifeComponent lifeComponent = hit.collider.GetComponent<LifeComponent>();
                if (lifeComponent != null)
                {
                    lifeComponent.doDamage(beamDamage * Time.deltaTime);
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
