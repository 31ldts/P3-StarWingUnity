using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneManagementHandler : MonoBehaviour
{
    private Canvas canvas;
    private LifeComponent lifeComponent;

    private static string lvl1 = "Level_1";
    private static string lvl2 = "Level_2";
    private static string lvl3 = "Level_3";

    private static int maxLevel = 3;

    static Scene currentScene;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        currentScene = SceneManager.GetActiveScene();

        canvas = GetComponentInParent<Canvas>();
        lifeComponent = player.GetComponent<LifeComponent>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Debug.Log("Tecla Enter pulsada.");
            if (canvas.CompareTag("NoCompleted"))
            {
                lifeComponent.RestartPlayer();
                CanvasHandler.DeactivateCanvas("NoCompleted");
            } else if (canvas.CompareTag("Defeat"))
            {
                MenuScene(gameObject);
            } else if (canvas.CompareTag("Completed"))
            {
                if (currentScene.name == lvl1)
                {
                    CompleteLevel(1);
                    SceneManager.LoadScene(lvl2);
                }
                else if (currentScene.name == lvl2)
                {
                    CompleteLevel(2);
                    SceneManager.LoadScene(lvl3);
                }
            } else if (canvas.CompareTag("BossDefeat"))
            {
                CompleteLevel(3);
                SceneManager.LoadScene("Menu");
            }
        }
    }

    private void CompleteLevel(int levelIndex)
    {
        string filePath = "Assets/Resources/LevelConfig.txt";

        string[] lines = File.ReadAllLines(filePath);
        levelIndex = levelIndex - 1;

        if (levelIndex < lines.Length)
        {
            lines[levelIndex] = "4";
        }

        if (levelIndex + 1 < lines.Length && levelIndex + 1 < maxLevel)
        {
            lines[levelIndex + 1] = "1";
        }

        File.WriteAllLines(filePath, lines);
    }

    public static void ChangeScene(GameObject invoker, int levelIndex, string shipName)
    {
        // Guardar el nombre de la nave en GameData
        GameData.ShipName = shipName;

        // Imprimir el nivel y el nombre de la nave en la consola
        // Debug.Log($"Cambiando a la escena con índice {levelIndex} y nave {shipName}");

        // Desactivar y eliminar el objeto invocador
        if (invoker != null)
        {
            Destroy(invoker);
        }

        // Eliminar la escena actual
        currentScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(currentScene);

        // Cargar la escena correspondiente (ha de estar en los Build Settings)

        if (levelIndex == 1)
        {
            SceneManager.LoadScene(lvl1, LoadSceneMode.Single);
        }
        else if (levelIndex == 2)
        {
            SceneManager.LoadScene(lvl2, LoadSceneMode.Single);
        } else if(levelIndex == 3)
        {
            SceneManager.LoadScene(lvl3, LoadSceneMode.Single);
        }
        
    }

    public static void MenuScene(GameObject invoker)
    {
        // Desactivar y eliminar el objeto invocador
        if (invoker != null)
        {
            Destroy(invoker);
        }

        // Eliminar la escena actual
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(currentScene);

        // Cargar una nueva escena
        string newSceneName = "Menu"; // Asegúrate de que esta escena esté en las Build Settings
        SceneManager.LoadScene(newSceneName, LoadSceneMode.Single);
    }
}
