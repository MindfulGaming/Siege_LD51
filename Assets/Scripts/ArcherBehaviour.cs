using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour : MonoBehaviour
{
    public bool active;
    public bool firing;
    public Enemy enemy;
    CastlePiece targetPiece;
    TileGrid tileGrid; //the grid being attacked
    float timeSinceLastShot;
    Vector2 moveToLocation;
    public SpriteRenderer arrow;

    void Awake()
    {
        tileGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<TileGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            if(targetPiece == null) ChooseAction();
            //if(targetPiece != null)
            {
                if(firing)
                {
                    timeSinceLastShot += Time.deltaTime;
                    if(timeSinceLastShot > enemy.secondsBetweenShots)
                    {
                        //fire arrow
                        FireArrow(targetPiece);
                        timeSinceLastShot = 0f;
                    }
                }
                else //moving
                {
                    float step = enemy.movementSpeed * Time.deltaTime;
                    enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, moveToLocation, step);
                }
            }
        }
    }

    void ChooseAction()
    {
        firing = false;
        //find new target
        targetPiece = tileGrid.GetClosestCastlePiece(enemy.transform.position, enemy.range);
        if(targetPiece == null) return; //nothing to do
        if(targetPiece.transform.position.x < this.transform.position.x) transform.rotation = Quaternion.Euler(0f, -180f, 0f);

        if(Vector2.Distance(targetPiece.transform.position, enemy.transform.position) <= enemy.range)
        {
            //fire!
            firing = true;
            enemy.SetAttacking();
        }

        else
        {
            //choose firing position and walk towards it
            var angle = Random.Range(0, 360);
            var radius = enemy.range - Random.Range(0f, 0.2f); //add a bit of random to where units stand
            var x = targetPiece.transform.position.x + (enemy.range * Mathf.Cos(angle));
            var y = targetPiece.transform.position.y + (enemy.range * Mathf.Sin(angle));
            moveToLocation = new Vector2(x, y);
            enemy.SetMoving();
        }
    }

    void FireArrow(CastlePiece targetPiece)
    {
        float arrowSpeed = 1f / enemy.secondsBetweenShots;
        float arrowDuration = Vector2.Distance(targetPiece.transform.position, this.transform.position) / arrowSpeed;
        targetPiece.DamageAfterDelay(arrowDuration, enemy.damage);
        arrow.transform.up = (targetPiece.transform.position - this.transform.position);
        StartCoroutine(MoveArrow(this.transform.position, targetPiece.transform.position, arrowDuration));
    }

    IEnumerator MoveArrow(Vector2 from, Vector2 to, float duration)
    {
        arrow.gameObject.SetActive(true);
        float timeElapsed = 0f;
        while(timeElapsed < duration)
        {
            float x = timeElapsed / duration;
            arrow.transform.position = Vector2.Lerp(from, to, x);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        arrow.gameObject.SetActive(false);
    }


}
