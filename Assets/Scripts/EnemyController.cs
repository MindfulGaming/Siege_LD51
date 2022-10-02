using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public List<Enemy> allEnemies;

    public Enemy GetEnemyIfInRange(Vector2 towerPosition, float range)
    {
        foreach(Enemy e in allEnemies)
        {
            if(Vector2.Distance(towerPosition, e.transform.position) < range)
            {
                return e;
            }
        }
        return null;
    }

    public void AddEnemy(Enemy enemy)
    {
        allEnemies.Add(enemy);
    }

    public void DestroyAllEnemies()
    {
        foreach(Enemy e in allEnemies)
        {
            Destroy(e.gameObject);
        }
        allEnemies.Clear();
    }

    void LateUpdate()
    {
       Enemy e;
       for(int i = 0; i < allEnemies.Count; i++)
       {
            e = allEnemies[i];
            if(e.health <= 0)
            {
                allEnemies.RemoveAt(i);
                i --;
                e.OnDeath();
            }
       }
    }
}
