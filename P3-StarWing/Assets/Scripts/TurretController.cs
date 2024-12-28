using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform player; // Referència a la nau del jugador
    public GameObject projectilePrefab; // Objecte projectil
    public Transform firePoint; // Punt des d'on surten els projectils

    private bool playerInRange = false; // Determina si el jugador és al rang
    public float projectileSpeed = 20f; // Velocidad del proyectil
    public float rotationSpeed = 15f; // Velocitat de rotació de la torreta
    public float fireRate = 0.5f; // Velocitat de dispar dels projectils (un cada X segons)
    private float fireCooldown = 0f; // Temporitzador de dispar

    private void OnTriggerEnter(Collider other)
    {
        // Comprova si el jugador ha entrat al rang
        if (other.CompareTag("Player")) // Assegura't que el jugador té el Tag "Player"
        {
            Debug.Log("Estic dins el rang!");
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Comprova si el jugador ha sortit del rang
        if (other.CompareTag("Player"))
        {
            Debug.Log("Estic fora el rang!");
            playerInRange = false;
        }
    }

    private void Update()
    {
        // Fer que la torreta apunti cap a la nau
        Vector3 direction = (player.position - transform.position).normalized; // Direcció cap a la nau

        if (player.position.z < transform.position.z) // La torreta s'atura quan ja ha passat la nau per davant d'ella
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            //Debug.Log("HOLAAAAAAAAAA");
            if (playerInRange && Time.time >= fireCooldown)
            {
                ShootAtPlayer();
                fireCooldown = Time.time + 1f / fireRate; // Calcula el següent dispar
            }
        }
    }

    void ShootAtPlayer()
    {
        // Instanciar el proyectil en el firePoint
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Projectile>().isPlayerProjectile = false;

        // Agregar movimiento al proyectil
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * projectileSpeed;
        }
        //Destroy(projectile, 6.0f);
    }

}

