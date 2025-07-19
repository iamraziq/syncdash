using System.Collections;
using UnityEngine;

public class GhostVisualSpawner : MonoBehaviour
{
    [Header("Ghost Prefabs")]
    public GameObject orbGhostPrefab;
    public GameObject obstacleGhostPrefab;
    public GameObject vfxGhostPrefab;
    [Header("Settings")]
    public float ghostDelay = 0.3f;

    public void SpawnGhost(GameObject original)
    {
        GameObject prefab = null;

        if (original.CompareTag("Orb"))
            prefab = orbGhostPrefab;
        else if (original.CompareTag("Obstacle"))
            prefab = obstacleGhostPrefab;

        if (prefab == null) return;

        GameObject ghost = Instantiate(prefab, original.transform.position, original.transform.rotation);
        ghost.AddComponent<GhostFollower>().Init(original.transform, ghostDelay);
    }

    public void SpawnVFXGhost(Vector3 position, Quaternion rotation)
    {
        if (vfxGhostPrefab == null) return;
        StartCoroutine(DelayedVFXGhostSpawn(position, rotation));
    }

    private IEnumerator DelayedVFXGhostSpawn(Vector3 position, Quaternion rotation)
    {
        yield return new WaitForSeconds(ghostDelay);
        GameObject ghost = Instantiate(vfxGhostPrefab, position, rotation);
        Destroy(ghost, 2f); 
    }

}
