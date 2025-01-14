using UnityEngine;
using UnityEngine.SceneManagement;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject[] prefabs;       // Array de prefabs a generar
    public float spawnDistance = 20f;  // Distancia desde la cámara en el eje Z
    public float spawnInterval = 2f;   // Intervalo de tiempo entre spawns
    public Vector2 spawnRangeX = new Vector2(-10f, 10f); // Rango aleatorio para el eje X
    public Vector2 spawnRangeY = new Vector2(-5f, 5f);   // Rango aleatorio para el eje Y
    public float moveSpeed = 5f;       // Velocidad a la que se mueven los prefabs
    public Vector2 scaleRange = new Vector2(0.8f, 1.2f); // Rango de escala aleatoria
    public Vector2 rotationSpeedRange = new Vector2(5f, 15f); // Rango de velocidad de rotación

    private Transform cameraTransform;
    public Transform naveTransform;
    private ExperienceLogic experienceLogic;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        InvokeRepeating("SpawnPrefab", 0f, spawnInterval);
        experienceLogic = Object.FindFirstObjectByType<ExperienceLogic>();
    }

    void SpawnPrefab()
    {
        if(experienceLogic.getTotalExperience()<1){
            if (prefabs.Length == 0)
            {
                Debug.LogWarning("No prefabs assigned!");
                return;
            }

            // Seleccionar un prefab aleatorio
            int randomIndex = Random.Range(0, prefabs.Length-1);
            GameObject prefab = prefabs[randomIndex];

            // Generar posiciones aleatorias en los planos X y Y
            float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
            float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);

            // Establecer la posición de generación basada en la distancia desde la cámara
            Vector3 spawnPosition = new Vector3(randomX, randomY, cameraTransform.position.z + spawnDistance);

            // Generar una rotación aleatoria
            Quaternion randomRotation = Random.rotation;

            // Instanciar el prefab en la posición generada con rotación aleatoria
            GameObject spawnedPrefab = Instantiate(prefab, spawnPosition, randomRotation);

            // Generar una escala aleatoria
            float randomScale = Random.Range(scaleRange.x, scaleRange.y);
            spawnedPrefab.transform.localScale = Vector3.one * randomScale;

            // Añadir el componente para mover el prefab en el eje Z
            spawnedPrefab.AddComponent<MoveInZDirection>().SetSpeed(moveSpeed);

            // Añadir el componente para rotación aleatoria
            float randomRotationSpeed = Random.Range(rotationSpeedRange.x, rotationSpeedRange.y);
            spawnedPrefab.AddComponent<RandomRotation>().SetRotationSpeed(randomRotationSpeed);
        } else {
            CancelInvoke("SpawnPrefab");
            GameObject prefab = prefabs[prefabs.Length-1];    //Puerta
            Vector3 spawnPosition = naveTransform.position + naveTransform.forward * 70f;
            Instantiate(prefab, spawnPosition, Quaternion.identity);
            
            GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
            // Iterar y destruir cada asteroide encontrado
            foreach (GameObject asteroid in asteroids)
            {
                Destroy(asteroid);
            }
            Scene currentScene = SceneManager.GetActiveScene();
            CanvasHandler.ActivateCanvas("Completed");
        }
    }
        
}

public class MoveInZDirection : MonoBehaviour
{
    private float speed;
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Mover el prefab a lo largo del eje Z
        transform.position += Vector3.back * speed * Time.deltaTime;

        // Eliminar el prefab si sobrepasa la posición Z de la cámara
        if (transform.position.z < cameraTransform.position.z)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float moveSpeed)
    {
        speed = moveSpeed;
    }
}

public class RandomRotation : MonoBehaviour
{
    private Vector3 rotationAxis;
    private float rotationSpeed;

    void Start()
    {
        // Escoger un eje de rotación aleatorio
        rotationAxis = Random.onUnitSphere;  // Vector aleatorio unitario
    }

    void Update()
    {
        // Rotar alrededor del eje escogido a la velocidad especificada
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
}