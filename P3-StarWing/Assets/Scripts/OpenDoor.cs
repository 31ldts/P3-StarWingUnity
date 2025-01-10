using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject[] enemies; // Array que contindrà tots els objectes classificats com a enemics

    public Transform comportaEsquerra;
    public Transform comportaDreta;

    public float distancia = 20f;
    public float velocitat = 5f;

    private Vector3 posicioInicialEsquerra;
    private Vector3 posicioInicialDreta;

    //private bool obrint = false; // Controla si la porta s'ha d'obrir

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posicioInicialEsquerra = comportaEsquerra.position;
        posicioInicialDreta = comportaDreta.position;

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        // if (obrint)
        if (AllEnemiesDead()) {
            ObrirPorta();
        }
        
    }

    private bool AllEnemiesDead()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                return false;
            }
        }
        return true;
    }

    private void ObrirPorta()
    {
        // Mou la comporta esquerre cap a l'esquerra
        comportaEsquerra.position = Vector3.MoveTowards(
            comportaEsquerra.position,
            posicioInicialEsquerra + Vector3.left * distancia,
            velocitat * Time.deltaTime
        );

        // Mou la comporta dreta cap a la dreta
        comportaDreta.position = Vector3.MoveTowards(
            comportaDreta.position,
            posicioInicialDreta + Vector3.right * distancia,
            velocitat * Time.deltaTime
        );
        
    }

    /*private void OnTriggerEnter(Collider other)
    {
        // Comprovem si l'objecte que entra és el jugador (a canviar la condició posteriorment)
        if (other.CompareTag("Player"))
        {
            obrint = true; // Inicia l'obertura
        }
    }*/
}
