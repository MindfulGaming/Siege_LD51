using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public float damage;
    public int points;
    public float secondsBetweenShots;
    public float minSecondsBetweenShots;
    public float movementSpeed;
    public float range;
    public float maxSecondsBetweenShots;
    public Animator animator;
    public SpriteRenderer deathAnim;
    public IntVariable score;
    public SpriteRenderer sprite;
    
    void Awake()
    {
        minSecondsBetweenShots = secondsBetweenShots;
    }
    
    public void OnDeath()
    {
        //animation
        var g = Instantiate(deathAnim, this.transform.position, Quaternion.identity);
        g.color = GetComponent<SpriteRenderer>().color;
        score.RuntimeValue += points;
        Destroy(this.gameObject);
    }

    public void OnSpawn()
    {

    }

    public void DamageEnemyAfterTime(float damage, float delay)
    {
        StartCoroutine(DamageDelay(damage, delay));
    }

    public void SlowRateAfterDelay(float slowFactor, float seconds, float slowDuration)
    {
        StartCoroutine(SlowDelay(slowFactor, seconds, slowDuration));
    }

    IEnumerator DamageDelay(float damage, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        health -= damage;
    }

    IEnumerator SlowDelay(float slowFactor, float seconds, float slowDuration)
    {
        yield return new WaitForSeconds(seconds);
        secondsBetweenShots = Mathf.Min(secondsBetweenShots * slowFactor, maxSecondsBetweenShots);
        yield return new WaitForSeconds(slowDuration);
        secondsBetweenShots = Mathf.Max(secondsBetweenShots / slowFactor);
    }

    public void SetMoving()
    {
        //moving animation
        animator.Play("Moving");
    }

    public void SetAttacking()
    {
        //attacking animation
        animator.Play("Attacking");
    }




}
