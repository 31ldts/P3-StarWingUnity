using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isPlayerProjectile;

    public float damageAmount = 10f; // Cantidad de da�o que causa el impacto de un proyectil

    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        switch(isPlayerProjectile)
        {
            case true:
               if (!(other.gameObject.tag == "Player"))
                {
                    // Si es enemigo, hacemos da�o (pendiente)
                    Destroy(gameObject);
                }
                break;

            case false:
                if (!(other.gameObject.tag == "Enemy"))
                {
                    // Si es jugador, hacemos da�o (pendiente)
                    Destroy(gameObject);
                }
                break;


        }
    }*/

    void Start()
    {
        // Destruir el objeto despu�s del tiempo especificado
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Opcional: Comprobamos contra qu� ha colisionado el proyectil
        //Debug.Log($"Projectil ha col�lisionat amb: {collision.gameObject.name}");

        // Obtiene el componente LifeComponent del objeto con el que colisionamos
        LifeComponent life = collision.gameObject.GetComponent<LifeComponent>();

        // Si el objeto tiene un LifeComponent, aplica el da�o
        if ( life != null )
        {
            life.doDamage(damageAmount);
        }

        // Destruimos el proyectil en el momento de la colisi�n
        Destroy(gameObject);
    }
}
