using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBehaviour : MonoBehaviour
{
    public float secondsBetweenHealing;
    public float healingAmount;
    public bool working;
    float secondsToNextHeal;
    public CastlePiece piece;
    public float healingRadius;
    
    // Start is called before the first frame update
    void Start()
    {
        secondsToNextHeal = secondsBetweenHealing;
        working = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(working)
        {
            secondsToNextHeal -= Time.deltaTime;
            if(secondsToNextHeal <= 0f)
            {
                Heal();
                secondsToNextHeal = secondsBetweenHealing;
            }
        }
    }

    void Heal()
    {
        var allTiles = piece.tileGrid.allPieces;
        for(int i = 0; i < allTiles.Count; i++)
        {
            var p = allTiles[i];
            if(p.working && Vector2.Distance(p.transform.position, this.transform.position) < healingRadius)
            {
                p.health += healingAmount;
                p.health = Mathf.Min(p.health, p.startingHealth);
            }
        }
    }

    
}
