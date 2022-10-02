using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportBehaviour : MonoBehaviour
{
    public CastlePiece piece;
    public float damageIncreaseMultiplier;
    public float rangeIncreaseMultiplier;
    public float treeSpawnMutplier;
    public float supportRadius;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BoostNeighbours()
    {
        var allTiles = piece.tileGrid.allPieces;
        for(int i = 0; i < allTiles.Count; i++)
        {
            var p = allTiles[i];
            if(p.working && Vector2.Distance(p.transform.position, this.transform.position) < supportRadius)
            {
                var firing = p.GetComponent<FireAtNearestEnemy>();
                if(firing != null) 
                {
                    firing.damage *= damageIncreaseMultiplier;
                    firing.range *= rangeIncreaseMultiplier;
                }
                
                
            }
        }

        var allSpawners = piece.tileGrid.treeSpawnPoints;
        for(int i = 0; i < allSpawners.Count; i++)
        {
            if(Vector2.Distance(allSpawners[i].transform.position, this.transform.position) < supportRadius)
            {
                allSpawners[i].chanceOfSpawningTree *= treeSpawnMutplier;
            }
        }
    }


}
