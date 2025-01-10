using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab; // Objecte projectil

    public Transform firePoint;  // Punt des d'on surten els projectils

    public float projectileSpeed = 80f; // Velocitat del projectil
    private AudioSource laserAudio;


    private void Start()
    {
       laserAudio = GetComponent<AudioSource>();
    }

    // Metode genèric per disparar
    public void ShootProjectile(bool isPlayerProjectile = false)
    {
        // Instanciar el projectil en el firePoint
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Assignar si el projectil és del jugador o no (si aquesta propietat existeix)
        if (projectile.GetComponent<Projectile>() != null)
        {
            projectile.GetComponent<Projectile>().isPlayerProjectile = isPlayerProjectile;
        }

        // Afegir moviment al projectil
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * projectileSpeed;
        }

        // Reproduir l'àudio del dispar
        if (laserAudio != null)
        {
            laserAudio.Play();
        }
    }
}