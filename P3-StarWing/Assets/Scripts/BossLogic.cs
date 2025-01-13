using System.Collections;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    // MOVIMENT
    public Transform areaCenter;  
    public float movementRadius = 5f;
    public float movementSpeed = 2f;
    public float lookSpeed = 5f;

    // RAIG DESRTRUCTOR
    public Transform player;            
    public float detectionRange = 100f;
    public LineRenderer lineRenderer;   // LineRenderer que simula el raig
    public float beamDuration = 1.5f;   // Durada del raig (en segons)
    public float beamCooldown = 2f;     // Temps entre rajos
    public float beamDamage = 10f;      // Quantitat de dany que infligeix el raig

    public float beamTrackingDelay = 0.1f; // Temps entre actualitzacions del seguiment del raig
    public float aimOffset = 1f;

    //public LayerMask damageLayer;       // Capes que poden rebre dany

    private float nextFireTime;
    private bool isFiring;
    private Vector3 targetPosition;  // Posici� objectiu del moviment

    void Start()
    {
        lineRenderer.enabled = false; // Assegura que el raig est� desactivat inicialment
        AssignNewTargetPosition();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        //MoveTowardsPosition();

        if (distanceToPlayer < detectionRange)
        {
            // Seguiment del jugador
            LookAtPlayer();
            // Atac
            if (Time.time >= nextFireTime && !isFiring)
            {
                StartCoroutine(FireBeam());
            }
        }
    }

    private void MoveTowardsPosition()
    {
        // Mou cap a la posici� objectiu
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        // Assigna una nova posici� si ha arribat al punt objectiu
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            AssignNewTargetPosition();
        }
    }

    private void AssignNewTargetPosition()
    {
        // Genera una nova posici� dins del radi
        Vector2 randomOffset = Random.insideUnitCircle * movementRadius;
        targetPosition = areaCenter.position + new Vector3(randomOffset.x, 0, randomOffset.y);
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
