using UnityEngine;

public class PainDealer : MonoBehaviour
{
    public float damageAmount = -10f; // Daño que se inflige al colisionar (negativo para daño, positivo para curar)

    private void OnTriggerEnter(Collider other)
    {
        // Obtén el componente LifeComponent del objeto con el que colisionamos
        LifeComponent lc = other.gameObject.GetComponent<LifeComponent>();

        // Si el objeto tiene un LifeComponent, aplica el daño
        if (lc != null)
        {
            lc.doDamage(damageAmount);
            Debug.Log($"Dealt {damageAmount} damage to {other.gameObject.name}");
        }
    }
}
