using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Comprova si l'objecte col·lisionat és el jugador
        if (other.CompareTag("Player"))
        {
            PlayerGameplay player = other.GetComponent<PlayerGameplay>();
            if (player != null)
            {
                player.SetDoubleShotMode(true);
                Debug.Log("Mode de dispar doble activat!");

                // Destruim l'objecte item
                Destroy(gameObject);
            }
        }
    }
}
