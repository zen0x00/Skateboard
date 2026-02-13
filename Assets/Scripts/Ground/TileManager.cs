using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;   
    public int tilesOnScreen = 5;
    public Transform player;

    public float slopeAngle = -7f;


    private Queue<GameObject> activeTiles = new Queue<GameObject>();
    private Stack<GameObject>[] tilePools;

    private Vector3 nextSpawnPosition;
    private Quaternion nextSpawnRotation;

   
    private int currentIndex = 0;

    void Start()
    {
        nextSpawnPosition = Vector3.zero;
        nextSpawnRotation = Quaternion.Euler(15f, 0f, 0f);


        
        tilePools = new Stack<GameObject>[tilePrefabs.Length];

        for (int i = 0; i < tilePrefabs.Length; i++)
        {
            tilePools[i] = new Stack<GameObject>();

            for (int j = 0; j < tilesOnScreen; j++)
            {
                GameObject tile = Instantiate(tilePrefabs[i]);
                tile.SetActive(false);
                tilePools[i].Push(tile);
            }
        }

        
        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        if (activeTiles.Count == 0) return;

        GameObject firstTile = activeTiles.Peek();

        if (player.position.z > firstTile.transform.position.z + 40f)
        {
            SpawnTile();
            RecycleTile();
        }

    }

    void SpawnTile()
    {
        int prefabIndex;
        
        prefabIndex = currentIndex;
        currentIndex++;
        if (currentIndex >= tilePrefabs.Length)
            currentIndex = 0;
        

        GameObject tile = GetTileFromPool(prefabIndex);
        
         tile.transform.SetPositionAndRotation(
            nextSpawnPosition,
            nextSpawnRotation
        );
        
        tile.SetActive(true);

        activeTiles.Enqueue(tile);

        Transform exitPoint = tile.transform.Find("ExitPoint");
        nextSpawnPosition = exitPoint.position;
        nextSpawnRotation = exitPoint.rotation;

     
    }

    void RecycleTile()
    {
        GameObject oldTile = activeTiles.Dequeue();
        oldTile.SetActive(false);

        
        for (int i = 0; i < tilePrefabs.Length; i++)
        {
            if (oldTile.name.Contains(tilePrefabs[i].name))
            {
                tilePools[i].Push(oldTile);
                break;
            }
        }
    }

    GameObject GetTileFromPool(int index)
    {
        if (tilePools[index].Count > 0)
        {
            return tilePools[index].Pop();
        }

       
        GameObject tile = Instantiate(tilePrefabs[index]);
        tile.SetActive(false);
        return tile;
    }
}
