using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform player; // Referència a la nau del jugador
    public GameObject projectilePrefab; // Objecte projectil
    public Transform firePoint; // Punt des d'on surten els projectils

    public float detectionRange = 30f; // Distància màxima per detectar el jugador
    public float projectileSpeed = 20f; // Velocitat del projectil
    public float rotationSpeed = 15f; // Velocitat de rotació de la torreta
    public float fireRate = 0.5f; // Velocitat de dispar dels projectils (un cada X segons)
    private float fireCooldown = 0f; // Temporitzador de dispar

    private void Update()
    {
        // Calcula la distància al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Comprova si el jugador és dins del rang establert i davant la torreta
        if (distanceToPlayer <= detectionRange && player.position.z < transform.position.z)
        {
            // Fer que la torreta apunti cap a la nau
            Vector3 direction = (player.position - transform.position).normalized; // Direcció cap a la nau
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // Disparar al jugador si es troba dins del rang i es pot disparar
            if (Time.time >= fireCooldown)
            {
                ShootAtPlayer();
                fireCooldown = Time.time + 1f / fireRate; // Calcula el següent dispar
            }
        }
    }

    void ShootAtPlayer()
    {
        // Instanciar el projectil en el firePoint
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Projectile>().isPlayerProjectile = false;

        // Afegir moviment al projectil
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * projectileSpeed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // Dibuixa un cercle representant el rang de detecció
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
