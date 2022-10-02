using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawnPoint : MonoBehaviour
{
    public bool running;
    public float minTimeBetweenSpawnAttempt;
    public float maxTimeBetweenSpawnAttempt;
    float timeToNextSpawnAttempt;
    public float chanceOfSpawningTree;
    public int currentRow;
    public int currentColumn;
    public TileGrid tileGrid;

    void Awake()
    {
        timeToNextSpawnAttempt = Random.Range(minTimeBetweenSpawnAttempt, maxTimeBetweenSpawnAttempt);
    }
    
    void Update()
    {
        if(running)
        {
            timeToNextSpawnAttempt -= Time.deltaTime;
            if(timeToNextSpawnAttempt <= 0)
            {
                TrySpawningTree();
            }
        }
    }

    void TrySpawningTree()
    {
        if(Random.Range(0f, 1f) < chanceOfSpawningTree)
        {
            SpawnTree();
        }
        else 
        {
            timeToNextSpawnAttempt = Random.Range(minTimeBetweenSpawnAttempt, maxTimeBetweenSpawnAttempt);
        }
        
    }

    void SpawnTree()
    {
        tileGrid.AddTree(currentRow, currentColumn);
        Destroy(this.gameObject);
    }
}
