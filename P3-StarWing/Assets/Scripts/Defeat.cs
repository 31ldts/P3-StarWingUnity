using UnityEngine;

public class Defeat : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Tecla Enter pulsada.");
            SceneManagementHandler.MenuScene(gameObject);
        }
    }
}
