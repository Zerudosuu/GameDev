using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int NumberofEnemies;
    public LayerMask enemyLayer;

    public GameObject[] enemyTypes;
    public float spawnInterval;
}

public class WaveSpawner : MonoBehaviour
{
    [SerializeField]
    Wave[] waves;

    public Transform[] spawnPointsUp;
    public Transform[] spawnPoints;
    private Wave currentWave;
    private int currentWaveNumber;
    private float nextSpawnTime;
    private bool canSpawn = true;

    public Animator animator;
    public TextMeshProUGUI WaveName;
    private bool canAnimate = false;

    void Update()
    {
        currentWave = waves[currentWaveNumber];
        SpawnWave();

        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (totalEnemies.Length == 0)
        {
            if (currentWaveNumber + 1 != waves.Length)
            {
                if (canAnimate)
                {
                    WaveName.text = waves[currentWaveNumber + 1].waveName;
                    animator.SetTrigger("WaveComplete");
                    canAnimate = false;
                }
            }
            else
            {
                Debug.Log("GameisFinished");
            }
        }
    }

    void SpawnNextWave()
    {
        currentWaveNumber++;
        canSpawn = true;
    }

    void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            GameObject randomEnemy;
            Transform randomPoint;

            randomEnemy = currentWave.enemyTypes[Random.Range(0, currentWave.enemyTypes.Length)];

            if (randomEnemy.layer == LayerMask.NameToLayer("FlyingEnemy"))
            {
                // Spawn flying enemy
                randomPoint = spawnPointsUp[Random.Range(0, spawnPointsUp.Length)];
            }
            else
            {
                // Spawn ground enemy
                randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            }

            Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
            currentWave.NumberofEnemies--;
            nextSpawnTime = Time.time + currentWave.spawnInterval;

            if (currentWave.NumberofEnemies == 0)
            {
                canSpawn = false;
                canAnimate = true;
            }
        }
    }
}
