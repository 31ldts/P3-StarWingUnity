using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    public float damageAmount = 20f; // Daño que se inflige al colisionar contra un enemigo u obstáculo

    private void OnCollisionEnter(Collision collision)
    {
        // Obtén el componente LifeComponent del objeto con el que colisionamos
        LifeComponent lc = gameObject.GetComponent<LifeComponent>();

        // Si el objeto tiene un LifeComponent, aplica el daño
        if (lc != null)
        {
            lc.doDamage(damageAmount);
            Debug.Log($"Dealt {damageAmount} damage to {collision.gameObject.name}");
        }
    }
}
