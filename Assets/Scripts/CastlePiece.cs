using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastlePiece : MonoBehaviour
{
    public enum CastleType {fast, support, heavy, disruptor, healer}
    public CastleType castleType;
    public int mergeRank;
    public int currentRow;
    public int currentColumn;
    public TileGrid tileGrid;
    public bool working;
    public float health;
    public float startingHealth;
    public SpriteRenderer spriteRenderer;
    Coroutine resizeRoutine;

    void Awake()
    {
        startingHealth = health;
        spriteRenderer = GetComponent<SpriteRenderer>();
        resizeRoutine = StartCoroutine(QuickResize());
    }
    
    public void PieceSelected()
    {
        if(resizeRoutine != null) StopCoroutine(resizeRoutine);
        transform.localScale *= 1.3f;
        spriteRenderer.sortingOrder = 10;
        working = false;
    }

    public void PieceDeselected()
    {
        transform.localScale /= 1.3f;
        spriteRenderer.sortingOrder = 0;
        working = true;
    }

    IEnumerator QuickResize()
    {
        float timeElapsed = 0f;
        Vector3 startScale = transform.localScale;
        while(timeElapsed < 0.5f)
        {
            float t = timeElapsed / 0.5f;
            t = EasingFunctions.DoEase(EasingFunctions.EasingFunction.EaseInOut, t);
            transform.localScale = startScale * Mathf.Lerp(1f, 0.8f, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        timeElapsed = 0f;
        while(timeElapsed < 0.5f)
        {
            float t = timeElapsed / 0.5f;
            t = EasingFunctions.DoEase(EasingFunctions.EasingFunction.EaseInOut, t);
            transform.localScale = startScale * Mathf.Lerp(0.8f, 1f, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = startScale;
    }

    public void DamageAfterDelay(float seconds, float damage)
    {
        StartCoroutine(DamageAfterSeconds(seconds, damage));
    }

    IEnumerator DamageAfterSeconds(float seconds, float damage)
    {
        yield return new WaitForSeconds(seconds);
        health -= damage;
    }

    void LateUpdate()
    {
        if(health <= 0) 
        {
            tileGrid.RemovePiece(this);
            Destroy(this.gameObject);
        }
        
    }
    

}
