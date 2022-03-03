using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public PlayerControl player;

    [SerializeField] Vector2 enemySpawnTime;
    [SerializeField] Enemy[] enemyPrefabs;
    float currEnemySpawnTime;
    float enemySpawnTimeCounter = 0f;
    [SerializeField] float enemySpawnDistance;
    [SerializeField] float enemySpawnAddedHeight;
    [SerializeField] float enemySpawnIncreasePerSecond;
    float enemySpawnRateIncrease = 1f;
    private bool spawning = false;

    public void StartSpawning(int zoneID)
    {
        spawning = true;
    }
    
    private void Awake()
    {
        ResetEnemySpawnTime();
    }

    private void Update()
    {
        enemySpawnTimeCounter += Time.deltaTime;

        if(enemySpawnTimeCounter >= currEnemySpawnTime)
        {
            SpawnEnemy();
        }

        enemySpawnRateIncrease += enemySpawnIncreasePerSecond * Time.deltaTime;
    }

    void ResetEnemySpawnTime()
    {
        currEnemySpawnTime = Random.Range(enemySpawnTime.x, enemySpawnTime.y);
        currEnemySpawnTime *= 1f / enemySpawnRateIncrease;
    }

    void SpawnEnemy()
    {
        if (!spawning) return;
        Enemy currEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
        ResetEnemySpawnTime();
        enemySpawnTimeCounter = 0f;

        Vector3 spawnPosition = player.transform.position;
        spawnPosition.y += enemySpawnAddedHeight;

        if (Random.Range(0, 2) == 0)
        {
            //Spawn on X axis
            float xOffset = enemySpawnDistance;
            if (Random.Range(0, 2) == 0)
                xOffset *= -1f;

            spawnPosition.x += xOffset;
        }
        else
        {
            //Spawn on Z axis
            float zOffset = enemySpawnDistance;
            if (Random.Range(0, 2) == 0)
                zOffset *= -1f;

            spawnPosition.z += zOffset;
        }
        currEnemy.transform.position = spawnPosition;

        currEnemy.SetTarget(player);
    }
}
