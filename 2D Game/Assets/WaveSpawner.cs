using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int NumberofEnemies;

    public GameObject[] enemyTypes;
    public float spawnInterval;
}

public class WaveSpawner : MonoBehaviour
{
    [SerializeField]
    Wave[] waves;

    public Transform[] spawnPoints;
    private Wave currentWave;
    private int currentWaveNumber;
    private float nextSpawnTime;

    private bool canSpawn = true;

    void Update()
    {
        currentWave = waves[currentWaveNumber];
        SpawnWave();

        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (totalEnemies.Length == 0 && !canSpawn && currentWaveNumber + 1 != waves.Length)
        {
            currentWaveNumber++;
            canSpawn = true;
        }
    }

    void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            GameObject randomEnemy = currentWave.enemyTypes[
                Random.Range(0, currentWave.enemyTypes.Length)
            ];
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
            currentWave.NumberofEnemies--;
            nextSpawnTime = Time.time + currentWave.spawnInterval;
            if (currentWave.NumberofEnemies == 0)
            {
                canSpawn = false;
            }
        }
    }
}
