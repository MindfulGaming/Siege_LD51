using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAtNearestEnemy : MonoBehaviour
{
    public float damage;
    public float range;
    public float secondsBetweenShots;
    EnemyController enemyController;
    public CastlePiece piece;
    float timeSinceLastShot;
    public ParticleSystem arrows;
    bool firing;
    public float bulletSpeed;
    public Enemy lastEnemyFiredAt;
    public bool slowEffect;
    public float slowFactor;
    public float slowDuration;


    void OnEnable()
    {
        enemyController = GameObject.FindWithTag("EnemyController").GetComponent<EnemyController>();
        var emission = arrows.emission;
        emission.rateOverTime = 1f / secondsBetweenShots;
    }

    void TryFiring()
    {
        Enemy nearestEnemy = enemyController.GetEnemyIfInRange(piece.transform.position, range);
        if(nearestEnemy != null)
        {
            firing = true;
            FireAt(nearestEnemy);
            lastEnemyFiredAt = nearestEnemy;
        }
        else 
        {
            arrows.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    void FireAt(Enemy enemy)
    {
        var emission = arrows.main;
        var trails = arrows.trails;
        if(enemy != lastEnemyFiredAt) 
        {
            trails.enabled = false; //reset trails
        }
        else trails.enabled = true;
        
        float bulletDuration = Vector2.Distance(enemy.transform.position, arrows.transform.position) / bulletSpeed;
        emission.startLifetime = bulletDuration;
        arrows.Play();
        arrows.transform.up = enemy.transform.position - piece.transform.position;
        if(slowEffect) enemy.SlowRateAfterDelay(slowFactor, bulletDuration, slowDuration);
        enemy.DamageEnemyAfterTime(damage, bulletDuration);
    }

    void Update()
    {
        if(piece.working)
        {
            timeSinceLastShot += Time.deltaTime;
            if(timeSinceLastShot >= secondsBetweenShots)
            {
                TryFiring();
                timeSinceLastShot = 0f;
            }
        }
        else if(firing)
        {
            firing = false;
            arrows.Stop(true, ParticleSystemStopBehavior.StopEmitting); //stop firing if no longer working, eg if picked up
        }
        
    }
}
