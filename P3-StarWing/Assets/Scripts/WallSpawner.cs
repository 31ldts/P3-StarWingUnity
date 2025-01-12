using UnityEngine;
using System.Collections;

public class WallSpawner : MonoBehaviour
{
    public GameObject[] ringObject;
    public float xRange= 1.0f;
    public float yRange= 1.0f;
    public float minSpawnTime = 1.0f;
    public float maxSpawnTime = 10.0f;

    void Start()
    {
        Invoke("SpawnWall", Random.Range(minSpawnTime, maxSpawnTime));
    }

    void SpawnWall()
    {
        float xOffset = Random.Range(-xRange, xRange);
        float yOffset = Random.Range(-yRange, yRange);
        int ringObjIndex = Random.Range(0, ringObject.Length);

        Instantiate(ringObject[ringObjIndex], transform.position + new Vector3(xOffset, yOffset, 0.0f), ringObject[ringObjIndex].transform.rotation);
        Invoke("SpawnWall", Random.Range(minSpawnTime, maxSpawnTime));
    }

    private void Instantiate(GameObject gameObject, Vector3 vector3)
    {
        throw new System.NotImplementedException();
    }
}
