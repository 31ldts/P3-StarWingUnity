using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class OpenDoor : MonoBehaviour
{
    private List<GameObject> expObjects;    // Lista que contendrá todos los objetos cargados en la escena que aporten experiencia
    private List<string> expTags;    // Lista de tags de los objetos expObjects

    public Transform comportaEsquerra;
    public Transform comportaDreta;

    public float distancia = 20f;
    public float velocitat = 5f;

    private Vector3 posicioInicialEsquerra;
    private Vector3 posicioInicialDreta;

    private float numTriggers = 0f;
    [SerializeField] private LifeComponent lifeComponent;

    void Start()
    {
        posicioInicialEsquerra = comportaEsquerra.position;
        posicioInicialDreta = comportaDreta.position;

        expObjects = new List<GameObject>();
        expTags = new List<string>();

        expTags.Add("Enemy");
        expTags.Add("Ring");

        foreach (GameObject obj in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            if (expTags.Contains(obj.tag)){
                expObjects.Add(obj);
            }
        }

        lifeComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<LifeComponent>();
    }

    void Update()
    {
        // Abrir las puestas solo cuando se haya obtenido toda la experiencia posible
        if (NoExpObjects()) {
            ObrirPorta();
        }
        
    }

    private bool NoExpObjects()
    {
        foreach (GameObject obj in expObjects)
        {
            if (obj != null)
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
            numTriggers++;
            if (numTriggers == 2)
            {
                Scene currentScene = SceneManager.GetActiveScene();
                if (currentScene.name == "Level_3")
                {
                    // Script que controla la logiaca del Boss
                    BossLogic bossLogic = Resources.FindObjectsOfTypeAll<BossLogic>()[0];
                    bossLogic.ActiveBoss(true);
                } else {
                    CanvasHandler.DeactivateCanvas("Completed");
                    CanvasHandler.ActivateCanvas("Completed");  // Pasamos de nivel
                }
            }
            else if (numTriggers == 1 && !NoExpObjects())
            {
                lifeComponent.PlayerDied(false);
                numTriggers = 0;
            }
        }
    }
}
