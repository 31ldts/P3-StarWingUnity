using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab del proyectil
    public Transform firePoint;        // Punto desde el que se disparará el proyectil
    public float projectileSpeed = 20f; // Velocidad del proyectil

    void Update()
    {
        // Detectar clic del ratón
        if (Input.GetMouseButtonDown(0)) // Botón izquierdo del ratón
        {
            ShootProjectile();
        }

        /*if (Input.GetKey(KeyCode.Space))
        {
            ShootProjectile();
        }*/
    }

    void ShootProjectile()
    {
        // Instanciar el proyectil en el firePoint
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Projectile>().isPlayerProjectile = true;

        // Agregar movimiento al proyectil
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * projectileSpeed;
        }
        //Destroy(projectile, 6.0f);
    }
}