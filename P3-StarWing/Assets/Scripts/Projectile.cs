using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isPlayerProjectile;

    public float damageAmount = 10f; // Cantidad de daño que causa el impacto de un proyectil

    void Start()
    {
        // Destruir el objeto despues del tiempo especificado
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"Projectil ha col�lisionat amb: {collision.gameObject.name}");
        if (!collision.gameObject.CompareTag("Ring")) {
            // Obtiene el componente LifeComponent del objeto con el que colisionamos
            LifeComponent life = collision.gameObject.GetComponent<LifeComponent>();

            // Si el objeto tiene un LifeComponent, aplica el da�o
            if ( life != null )
            {
                life.doDamage(damageAmount);
            }            
        }
        // Destruimos el proyectil en el momento de la colision
            Destroy(gameObject);
    }
}
