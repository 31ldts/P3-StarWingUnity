using UnityEngine;
using UnityEngine.UI;

public class NoCompleted : MonoBehaviour
{
    [SerializeField] private LifeComponent lifeComponent;
    [SerializeField] private Canvas canvas;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Tecla Enter pulsada.");
            lifeComponent.RestartPlayer();

            canvas.gameObject.SetActive(false);
        }
    }
}
