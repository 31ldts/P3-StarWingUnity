using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isPlayerProjectile;

    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        switch(isPlayerProjectile)
        {
            case true:
               if (!(other.gameObject.tag == "Player"))
                {
                    // Si es enemigo, hacemos daño (pendiente)
                    Destroy(gameObject);
                }
                break;

            case false:
                if (!(other.gameObject.tag == "Enemy"))
                {
                    // Si es jugador, hacemos daño (pendiente)
                    Destroy(gameObject);
                }
                break;


        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        // Opcional: Comprovar el tipus de col·lisió
        Debug.Log($"Projectil ha col·lisionat amb: {collision.gameObject.name}");

        // Destruir el projectil quan col·lisiona
        Destroy(gameObject);
    }
}
