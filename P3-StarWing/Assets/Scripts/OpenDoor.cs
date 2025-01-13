using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour
{
    public GameObject[] enemies; // Array que contindr� tots els objectes classificats com a enemics

    public Transform comportaEsquerra;
    public Transform comportaDreta;

    public float distancia = 20f;
    public float velocitat = 5f;

    private Vector3 posicioInicialEsquerra;
    private Vector3 posicioInicialDreta;

    private float numTriggers = 0f;
    private ExperienceLogic experienceLogic;
    [SerializeField] private LifeComponent lifeComponent;

    //private bool obrint = false; // Controla si la porta s'ha d'obrir

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posicioInicialEsquerra = comportaEsquerra.position;
        posicioInicialDreta = comportaDreta.position;

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        experienceLogic = Object.FindFirstObjectByType<ExperienceLogic>();
        lifeComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<LifeComponent>();
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

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
        {
            Debug.Log("He entrado en el trigger!");
            numTriggers++;
            Debug.Log(numTriggers);
            if (numTriggers == 2)
            {
                CanvasHandler.DeactivateCanvas("Completed");
                // Pasamos de nivel
                Scene currentScene = SceneManager.GetActiveScene();
                Debug.Log(currentScene.name);
                CanvasHandler.ActivateCanvas("Completed");
            }
            else if ((numTriggers == 1) && (experienceLogic.getTotalExperience() < 1.0f))
            {
                Debug.Log(experienceLogic.getTotalExperience());
                Debug.Log("Estoy en el segundo if!");
                lifeComponent.PlayerDied(false);
                numTriggers = 0;
            }
        }
    }
}
