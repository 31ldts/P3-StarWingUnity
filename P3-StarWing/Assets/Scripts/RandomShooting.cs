using UnityEngine;

public class RandomShooting : MonoBehaviour
{
    public float fireRate = 20f; // Velocitat de dispar dels projectils (un cada X segons)
    private float nextFireTime = 0f; // Temporitzador per al seg�ent dispar

    public Transform player; // Referència a la nau del jugador
    public float detectionRange = 100f; // Distància màxima per detectar el jugador

    public ProjectileShooter projectileShooter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        projectileShooter = GetComponent<ProjectileShooter>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calcula la distància al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Comprova si el jugador és dins del rang establert i davant la torreta
        if (distanceToPlayer <= detectionRange)
        {
            if (Time.time >= nextFireTime)
            {
                projectileShooter.ShootProjectile(false); // No és un projectil del jugador
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
           
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // Dibuixa un cercle representant el rang de detecció
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

