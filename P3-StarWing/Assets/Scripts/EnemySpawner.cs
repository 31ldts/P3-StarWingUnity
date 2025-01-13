using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // Array de enemigos a generar
    public float spawnDistance = 150;  // Distancia desde la cámara en el eje Z
    public float spawnInterval = 4f;   // Intervalo de tiempo entre spawns
    public Vector2 spawnRangeX = new Vector2(-10f, 10f); // Rango aleatorio para el eje X
    public Vector2 spawnRangeY = new Vector2(-5f, 5f);   // Rango aleatorio para el eje Y
    public float moveSpeed = 10f;       // Velocidad a la que se mueven los enemigos
    public Vector2 scaleRange = new Vector2(5f, 8f); // Rango de escala aleatoria
    public float stopDistance;    // Distancia a la que los enemigos dejan de avanzar en Z

    public int maxEnemies = 3;         // Número máximo de enemigos en la escena

    private Transform playerTransform;
    private ExperienceLogic experienceLogic;

    void Start()
    {
        playerTransform = Camera.main.transform; // Supone que la cámara es el jugador
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
        stopDistance = Random.Range(50, 55);
        experienceLogic = Object.FindFirstObjectByType<ExperienceLogic>();
    }

    void SpawnEnemy()
    {
        // Comprobar cuántos enemigos hay en la escena actualmente
        EnemyMovement[] existingEnemies = Object.FindObjectsByType<EnemyMovement>(FindObjectsSortMode.None);
        
        // Si ya hay 3 enemigos, no crear más
        if (existingEnemies.Length >= maxEnemies)
        {
            return; // No generar nuevos enemigos
        }

        if (enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned!");
            return;
        }

        // Seleccionar un enemigo aleatorio
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyPrefab = enemyPrefabs[randomIndex];

        // Generar posiciones aleatorias en los planos X y Y
        float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
        float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);

        // Establecer la posición de generación basada en la distancia desde el jugador
        Vector3 spawnPosition = new Vector3(randomX, randomY, playerTransform.position.z + spawnDistance);

        // Instanciar el enemigo en la posición generada con rotación de 180 grados en el eje Y
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.Euler(0, 180, 0));

        // Generar una escala aleatoria
        float randomScale = Random.Range(scaleRange.x, scaleRange.y);
        spawnedEnemy.transform.localScale = Vector3.one * randomScale;

        // Añadir el componente para mover el enemigo en el eje Z y luego aleatoriamente en X e Y
        spawnedEnemy.AddComponent<EnemyMovement>().Initialize(moveSpeed, stopDistance, playerTransform);


        if(experienceLogic.getTotalExperience()>=1){
            CancelInvoke("SpawnEnemy");
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            // Iterar y destruir cada enemigo encontrado
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }


    }
}

public class EnemyMovement : MonoBehaviour
{
    private float moveSpeed;
    private float stopDistance;
    private Transform playerTransform;
    private bool moveRandomly = false;
    private Vector3 targetPosition;

    public void Initialize(float speed, float stopDist, Transform player)
    {
        moveSpeed = speed;
        stopDistance = stopDist;
        playerTransform = player;
    }

    void Update()
    {
        if (moveRandomly)
        {
            moveSpeed = 2.5f;
            // Movimiento aleatorio más suave y de mayor alcance
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Si el enemigo ha llegado a la posición aleatoria, elige una nueva
            if (Vector3.Distance(transform.position, targetPosition) < 0.2f)
            {
                SetNewRandomTarget();
            }

        }
        else
        {
            // Movimiento en el eje Z hacia el jugador
            transform.position += Vector3.back * moveSpeed * Time.deltaTime;

            // Comprobar si el enemigo está a una distancia razonable del jugador
            if (Vector3.Distance(transform.position, playerTransform.position) <= stopDistance)
            {
                moveRandomly = true;
                SetNewRandomTarget();
            }
            
        }
        
    }

    // Establecer una nueva posición aleatoria en el plano X e Y
    private void SetNewRandomTarget()
    {
        // Generar nueva posición aleatoria dentro de un rango mayor
        float randomX = Random.Range(playerTransform.position.x - 10f, playerTransform.position.x + 10f);
        float randomY = Random.Range(playerTransform.position.y - 10f, playerTransform.position.y + 10f);

        targetPosition = new Vector3(randomX, randomY, transform.position.z); // Mantener la misma posición Z
    }

}
