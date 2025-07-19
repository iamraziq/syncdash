using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject orbPrefab;
    public GhostVisualSpawner ghostVisualSpawner;

    public int poolSize = 10;
    public float spawnInterval = 2f;
    public float spawnZ = 30f;
    public float[] lanePositionsX = new float[] { -3f, 0f, 3f };

    private List<GameObject> obstaclePool;
    private List<GameObject> orbPool;
    private float timer;

    void Start()
    {
        obstaclePool = CreatePool(obstaclePrefab);
        orbPool = CreatePool(orbPrefab);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnRandomObject();
            timer = 0f;
        }
    }

    List<GameObject> CreatePool(GameObject prefab)
    {
        List<GameObject> pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
        return pool;
    }

    void SpawnRandomObject()
    {
        int randomType = Random.Range(0, 2); // 0 = Obstacle, 1 = Orb
        float x = lanePositionsX[Random.Range(0, lanePositionsX.Length)];
        Vector3 spawnPosition = new Vector3(x, 0.5f, spawnZ);

        if (randomType == 0)
        {
            GameObject obstacle = GetPooledObject(obstaclePool);
            if (obstacle != null)
            {
                obstacle.GetComponent<Obstacle>().Activate(spawnPosition);
                ghostVisualSpawner.SpawnGhost(obstacle);
            }               
        }
        else
        {
            GameObject orb = GetPooledObject(orbPool);
            if (orb != null)
            {
                orb.GetComponent<Orb>().Activate(spawnPosition);
                ghostVisualSpawner.SpawnGhost(orb);

            }               
        }
    }

    GameObject GetPooledObject(List<GameObject> pool)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }
        return null;
    }
}
