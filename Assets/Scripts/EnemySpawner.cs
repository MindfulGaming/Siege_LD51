using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy catapultPrefab;
    public Enemy archerPrefab;
    public GameEvent startGame;
    public GameEvent endOfGame;
    public int baseSizePerWave; //base number of units in one wave
    float lastWaveSize;
    public float waveSizeMultiplier; //rate at which waves get bigger
    public float secondsSinceLastWave;
    public bool running;
    public EnemyController enemyController;
    public int currentWave = 0;
    public float waveHealthMultiplier; //multiplied by base heath so waves get harder over time
    public float proportionOfCatapults; //percentage of enemies that are catapults
    public float timeBetweenWaves;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public Color32 maxColour;
    public Color32 minColour;
    float width;
    float height;
    float perimeter;

    void Awake()
    {
        height = maxY - minY;
        width = maxX - minX;
        perimeter = (height * 2) + (width * 2);
        lastWaveSize = baseSizePerWave;
    }

    void OnEnable()
    {
        startGame.RegisterListener(StartGame);
        endOfGame.RegisterListener(OnGameEnd);
    }

    void OnDisable()
    {
        startGame.UnregisterListener(StartGame);
        endOfGame.UnregisterListener(OnGameEnd);
    }

    void OnGameEnd()
    {
        running = false;
        enemyController.DestroyAllEnemies();
    }


    void StartGame()
    {
        running = true;
    }

    public void SpawnCatapults(int n)
    {
        for(int i = 0; i < n; i++)
        {
            Enemy e = Instantiate(catapultPrefab, PickRandomSpawnPoint(), Quaternion.identity);
            enemyController.AddEnemy(e);
            e.health *= e.health * waveHealthMultiplier;
        }
    }

    public void SpawnArchers(int n)
    {
        for(int i = 0; i < n; i++)
        {
            Enemy e = Instantiate(archerPrefab, PickRandomSpawnPoint(), Quaternion.identity);
            e.sprite.color = Color32.Lerp(minColour, maxColour, Random.Range(0f, 1f));
            enemyController.AddEnemy(e);
            e.health *= e.health * waveHealthMultiplier;
        }
    }

    Vector2 PickRandomSpawnPoint()
    {
        float r = Random.Range(0f, perimeter); //pick point along perimeter
        if(r > perimeter - width) //bottom edge
        {
            return new Vector2(r - width - height - height, minY - 1f);
        }
        else if(r > perimeter - width - height) //right edge
        {
            return new Vector2(maxX + 1f, r - width - height );
        }
        else if(r > perimeter - width - width - height) //top edge
        {
            return new Vector2(r - width, maxY + 1f);
        }
        else //left edge
        {
            return new Vector2(minX - 1f, r);
        }
    }

    void IncreaseWaveNumber()
    {
        currentWave ++;
        proportionOfCatapults *= 1.2f;
        proportionOfCatapults = Mathf.Min(proportionOfCatapults, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(running)
        {
            secondsSinceLastWave += Time.deltaTime;
            if(secondsSinceLastWave >= timeBetweenWaves)
            {
                SpawnNewWave();
                IncreaseWaveNumber();
                secondsSinceLastWave = 0f;
            }
        }
    }

    void SpawnNewWave()
    {
        lastWaveSize = lastWaveSize * waveSizeMultiplier; //should increase with wave number
        int numCatapults = (int)(lastWaveSize * proportionOfCatapults);
        int numArchers = (int)(lastWaveSize - numCatapults);

        SpawnCatapults(numCatapults);
        SpawnArchers(numArchers);
    }
}
