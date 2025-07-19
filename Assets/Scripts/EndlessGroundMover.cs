using UnityEngine;

public class EndlessGroundMover : MonoBehaviour
{
    public GameObject[] groundTiles; 
    public float tileLength = 30f; 

    private void Update()
    {
        float moveSpeed = GameManager.Instance != null ? GameManager.Instance.CurrentSpeed : 5f;

        foreach (GameObject tile in groundTiles)
        {
            tile.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

            if (tile.transform.position.z < -tileLength)
            {

                float maxZ = GetFurthestTileZ();
                tile.transform.position = new Vector3(
                    tile.transform.position.x,
                    tile.transform.position.y,
                    maxZ + tileLength
                );
            }
        }
    }

    private float GetFurthestTileZ()
    {
        float maxZ = float.MinValue;
        foreach (GameObject tile in groundTiles)
        {
            if (tile.transform.position.z > maxZ)
                maxZ = tile.transform.position.z;
        }
        return maxZ;
    }
}
