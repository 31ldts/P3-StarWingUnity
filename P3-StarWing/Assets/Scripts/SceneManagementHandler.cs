using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagementHandler : MonoBehaviour
{
    public static void ChangeScene(GameObject invoker, int levelIndex, string shipName)
    {
        // Imprimir el nivel y el nombre de la nave en la consola
        Debug.Log($"Cambiando a la escena con índice {levelIndex} y nave {shipName}");

        // Desactivar y eliminar el objeto invocador
        if (invoker != null)
        {
            Destroy(invoker);
            Debug.Log("Objeto invocador eliminado.");
        }

        // Eliminar la escena actual
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(currentScene);

        // Cargar una nueva escena genérica (ajusta el nombre o índice según sea necesario)
        string newSceneName = "IvanTestScene";
        SceneManager.LoadScene(newSceneName, LoadSceneMode.Single);
    }
}
