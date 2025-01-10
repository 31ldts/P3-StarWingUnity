using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform player; // Refer�ncia a la nau del jugador

    public float detectionRange = 50f; // Dist�ncia m�xima per detectar el jugador
    public float rotationSpeed = 15f; // Velocitat de rotaci� de la torreta
    public float fireRate = 1f; // Velocitat de dispar dels projectils (un cada X segons)
    private float fireCooldown = 0.5f; // Temporitzador de dispar

    public ProjectileShooter projectileShooter;

    private void Start()
    {
        projectileShooter = GetComponent<ProjectileShooter>();
    }

    private void Update()
    {
        // Calcula la dist�ncia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Comprova si el jugador �s dins del rang establert i davant la torreta
        if (distanceToPlayer <= detectionRange && player.position.z < transform.position.z)
        {
            // Fer que la torreta apunti cap a la nau
            Vector3 direction = (player.position - transform.position).normalized; // Direcci� cap a la nau
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // Disparar al jugador si es troba dins del rang i es pot disparar
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)// && IsAimingAtPlayer(lookRotation))
            {
                projectileShooter.ShootProjectile(false);
                fireCooldown = fireRate; // Calcula el seg�ent dispar
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // Dibuixa un cercle representant el rang de detecci�
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
