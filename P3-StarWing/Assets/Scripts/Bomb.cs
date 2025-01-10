using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float radi = 5f; // Radi de l'explosi�
    public float dany = 300f; // Dany causat per la bomba

    private void OnCollisionEnter(Collision collision)
    {
        // Gestionar danys o efectes d'explosi�
        Debug.Log($"Bomba col�lisiona amb {collision.gameObject.name}");

        // Cercar objectes dins del radi d'explosi�
        Collider[] hits = Physics.OverlapSphere(transform.position, radi);
        foreach (Collider hit in hits)
        {
            Debug.Log(hit.name);
            LifeComponent life = hit.GetComponent<LifeComponent>();
            if (life != null)
            {
                life.doDamage(-dany);
            }
        }

        // Destruir despr�s de l'impacte
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // Dibuixa un cercle representant el rang de detecci�
        Gizmos.DrawWireSphere(transform.position, radi);
    }
}
