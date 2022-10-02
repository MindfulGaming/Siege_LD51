using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleLeveller : MonoBehaviour
{
    public List<CastlePiece> prefabs;
    public List<Color32> colours;
    public Grid grid;
    public CastleStats castleStats;
    public AudioSource audioSource;

    
    //merge p2 into p1
    public void Merge(CastlePiece p1, CastlePiece p2)
    {
        int currentRow = p1.currentRow;
        int currentColumn = p1.currentColumn;
        int mergeRank = p1.mergeRank;
        TileGrid tileGrid = p1.tileGrid;
        audioSource.Play();

        //Destroy p1 and p2
        tileGrid.CreateTreeSpawnPoint(p2.currentRow, p2.currentColumn);
        p1.tileGrid.RemovePiece(p1);
        p2.tileGrid.RemovePiece(p2);
        Destroy(p1.gameObject);
        Destroy(p2.gameObject);

        //Instantiate new tower at next rank
        mergeRank = Mathf.Min(mergeRank + 1, prefabs.Count);
        CastlePiece newPiece = Instantiate(prefabs[mergeRank]);
        newPiece.currentColumn = currentColumn;
        newPiece.currentRow = currentRow;
        newPiece.tileGrid = tileGrid;
        tileGrid.AddPiece(newPiece);
        newPiece.transform.position = grid.CellToWorld(new Vector3Int(currentColumn, currentRow, 0));
        SpriteRenderer sprite = newPiece.GetComponent<SpriteRenderer>();
        int i = Random.Range(0, colours.Count);
        sprite.color = colours[i];
        switch(i)
        {
            case 0:
            {
                newPiece.castleType = CastlePiece.CastleType.fast;
                break;
            }

            case 1:
            {
                newPiece.castleType = CastlePiece.CastleType.support;
                break;
            }

            case 2:
            {
                newPiece.castleType = CastlePiece.CastleType.heavy;
                break;
            }

            case 3:
            {
                newPiece.castleType = CastlePiece.CastleType.disruptor;
                break;
            }

            case 4:
            {
                newPiece.castleType = CastlePiece.CastleType.healer;
                break;
            }

            default:
            {
                newPiece.castleType = CastlePiece.CastleType.fast;
                break;
            }
        }
        
    }

    void SetStats(CastlePiece piece)
    {
        int rank = piece.mergeRank;
        switch(piece.castleType)
        {
            case CastlePiece.CastleType.fast:
            {
                var turret = piece.GetComponent<FireAtNearestEnemy>();
                turret.slowEffect = false;
                turret.damage = castleStats.baseAttackSeconds[0] / castleStats.damageMultipliers[rank];
                turret.range = castleStats.baseRange[0] * castleStats.rangeMultipliers[rank];
                turret.secondsBetweenShots = castleStats.baseAttackSeconds[0] * castleStats.attackSecondsMultipliers[rank];
                piece.health = castleStats.baseHealth[0] * castleStats.healthMutipliers[rank];
                piece.startingHealth = piece.health;
                break;
            }

            case CastlePiece.CastleType.support:
            {
                var turret = piece.GetComponent<FireAtNearestEnemy>();
                turret.slowEffect = false;
                turret.damage = castleStats.baseAttackSeconds[1] / castleStats.damageMultipliers[rank];
                turret.range = castleStats.baseRange[1] * castleStats.rangeMultipliers[rank];
                turret.secondsBetweenShots = castleStats.baseAttackSeconds[1] * castleStats.attackSecondsMultipliers[rank];
                piece.health = castleStats.baseHealth[1] * castleStats.healthMutipliers[rank];
                piece.startingHealth = piece.health;

                var supporter = piece.GetComponent<SupportBehaviour>();
                supporter.damageIncreaseMultiplier = castleStats.baseSupportDamage * castleStats.supportMultipliers[rank];
                supporter.rangeIncreaseMultiplier = castleStats.baseSupportRange * castleStats.supportMultipliers[rank];
                supporter.treeSpawnMutplier = castleStats.baseSupportTreeChance * castleStats.supportMultipliers[rank];
                supporter.BoostNeighbours();
                break;
            }

            case CastlePiece.CastleType.heavy:
            {
                var turret = piece.GetComponent<FireAtNearestEnemy>();
                turret.slowEffect = false;
                turret.damage = castleStats.baseAttackSeconds[2] / castleStats.damageMultipliers[rank];
                turret.range = castleStats.baseRange[2] * castleStats.rangeMultipliers[rank];
                turret.secondsBetweenShots = castleStats.baseAttackSeconds[2] * castleStats.attackSecondsMultipliers[rank];
                piece.health = castleStats.baseHealth[2] * castleStats.healthMutipliers[rank];
                piece.startingHealth = piece.health;
                break;
            }

            case CastlePiece.CastleType.disruptor:
            {
                var turret = piece.GetComponent<FireAtNearestEnemy>();
                turret.slowEffect = true;
                turret.damage = castleStats.baseAttackSeconds[3] / castleStats.damageMultipliers[rank];
                turret.range = castleStats.baseRange[3] * castleStats.rangeMultipliers[rank];
                turret.secondsBetweenShots = castleStats.baseAttackSeconds[3] * castleStats.attackSecondsMultipliers[rank];
                piece.health = castleStats.baseHealth[3] * castleStats.healthMutipliers[rank];
                piece.startingHealth = piece.health;
                break;
            }

            case CastlePiece.CastleType.healer:
            {
                var healer = piece.GetComponent<HealingBehaviour>();
                healer.healingAmount = castleStats.basehealingAmount * castleStats.healingAmountMultipliers[rank];
                healer.secondsBetweenHealing = castleStats.baseSecondsBetweenHeals / castleStats.healingAmountMultipliers[rank];
                piece.health = castleStats.baseHealth[4] * castleStats.healthMutipliers[rank];
                piece.startingHealth = piece.health;
                break;
            }
        }
        

    }
    

}
