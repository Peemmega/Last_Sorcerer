using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class GameDayLoopManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;  //The number of enemies of this type to spawn in this wave
        public int spawnCount;  //The number of enemies of this type already spawned in this wave
        public GameObject enemyPrefab;
    }

    [System.Serializable]
    public class Day
    {
        public string dayName;
        public List<Wave> waves;
    }
    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints; //A list to store all the relative spawn points of enemies

    public float dayTimeDuration = 10;
    public bool onRaid = false;
    float watchTime;

    public List<Day> days;
    public int currentDayCount;   
    public int currentWave;
    public int enemiesAlive;
    float spawnTimer;
    public bool onSpawn = false;
    public SpawnItems spawnItems;

    [Header("UI")]
    public TextMeshProUGUI TimerText;

    public DayNightCycle daynight;
    public float dayLerpSpeed;

    [Header("Audio")]
    BGM_Manager BgmManager;

    void Start()
    {
        BgmManager = FindFirstObjectByType<BGM_Manager>();
        watchTime = dayTimeDuration;
        spawnItems.SpawnItemsOnWave();
    }

    void Update()
    {
        if (onRaid && daynight.timeOfDay > 6)
        {
            daynight.timeOfDay = daynight.timeOfDay - Time.deltaTime * dayLerpSpeed;
        }
        else if ((!onRaid)  && daynight.timeOfDay < 10)
        {
            daynight.timeOfDay += Time.deltaTime * dayLerpSpeed;
        }

        UpdateStopwatch();
    }

    void UpdateStopwatch()
    {
     
        if (!onRaid)
        {
            watchTime -= Time.deltaTime;
            if (watchTime <= 0)
            {
                onRaid = !onRaid;
                BgmManager.Swap();
                watchTime = dayTimeDuration;
            }
        } else
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= 2f)
            {
                spawnTimer = 0f;
                onSpawn = SpawnEnemies();
                if (onSpawn == false && enemiesAlive == 0)
                {
                    onRaid = !onRaid;
                    BgmManager.Swap();
                    currentDayCount++;
                    spawnItems.SpawnItemsOnWave();
                    Debug.Log("Day Clear");
                }
            }
        }

        UpdateStopwatchDisplay();
    }

    void UpdateStopwatchDisplay()
    {
        // Calculate the number of minutes and seconds that have elapsed
        int minutes = Mathf.FloorToInt(watchTime / 60);
        int seconds = Mathf.FloorToInt(watchTime % 60);

        // Update the stopwatch text to display the elapsed time
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    bool SpawnEnemies()
    {
        bool spawned = false;
        if (days[currentDayCount] != null)
        {
            foreach (var enemyGroup in days[currentDayCount].waves[currentWave].enemyGroups)
            {
                // Check if the minimum number of enemies of this type have been spawned
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    // Spawn the enemy at a random position close to the player
                    int randomint = UnityEngine.Random.Range(0, relativeSpawnPoints.Count);
                    GameObject enemy = Instantiate(enemyGroup.enemyPrefab, relativeSpawnPoints[randomint].position + new Vector3(UnityEngine.Random.Range(-2, -2), 0, UnityEngine.Random.Range(-2, -2)), Quaternion.identity);
                    enemy.transform.SetParent(GameObject.Find("enemies").transform);
                    enemyGroup.spawnCount++;
                    //waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                    spawned = true;
                }
            }

            return spawned;
        } else
        {
            Debug.Log("You Win");
            Time.timeScale = 0;
            return spawned;
        }
       
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }
}

