using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] goodEnemyPrefabs; // Array of Good enemy prefabs
    public GameObject[] badEnemyPrefabs; // Array of Bad enemy prefabs
    public Transform[] spawnPoints; // Array of spawn points
    public int initialEnemyCount = 5; // Initial number of enemies per wave
    public float timeBetweenWaves = 5f; // Time between waves
    public float timeBetweenSpawns = 1f; // Time between enemy spawns within a wave

    public int currentWave = 0;
    private bool isSpawning = false;
    public List<GameObject> activeEnemies = new List<GameObject>();

    VisualElement contaner;
    Label Wave;

    void Start()
    {
        var root = GameObject.FindAnyObjectByType<UIDocument>().rootVisualElement;

        contaner = root.Q<VisualElement>("UIContainer");
        Wave = contaner.Q<Label>("Wave");

        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        Wave.text = "Wave:" + currentWave.ToString();
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            currentWave++;
            int enemyCount = initialEnemyCount + (currentWave * 2); // Increase enemy count per wave

            yield return StartCoroutine(SpawnEnemies(enemyCount));

            // Wait until all enemies are defeated
            yield return new WaitUntil(() => activeEnemies.Count == 0);

            // Wait before starting the next wave
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator SpawnEnemies(int count)
    {
        isSpawning = true;

        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenSpawns); // Wait between spawns
        }

        isSpawning = false;
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        // Randomly decide enemy type to spawn
        GameObject enemyPrefab;
        if (Random.value > 0.5f)
        {
            // Randomly select a good enemy prefab
            enemyPrefab = goodEnemyPrefabs[Random.Range(0, goodEnemyPrefabs.Length)];
        }
        else
        {
            // Randomly select a bad enemy prefab
            enemyPrefab = badEnemyPrefabs[Random.Range(0, badEnemyPrefabs.Length)];
        }

        GameObject spawnedEnemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        // Set enemy type (if needed)
        EnemyScript enemyScript = spawnedEnemy.GetComponent<EnemyScript>();
        if (enemyScript != null)
        {
            if (System.Array.Exists(goodEnemyPrefabs, element => element == enemyPrefab))
            {
                enemyScript.enemyType = EnemyType.Good;
            }
            else
            {
                enemyScript.enemyType = EnemyType.Bad;
            }
        }

        // Adjust speed if the current wave is 3 or higher
        if (currentWave >= 3)
        {
            NavMeshAgent agent = spawnedEnemy.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.speed += 1.5f;
            }
        }

        // Add the spawned enemy to the active enemies list
        activeEnemies.Add(spawnedEnemy);

        // Subscribe to the enemy's death event
        Health enemyHealth = spawnedEnemy.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += () => RemoveEnemy(spawnedEnemy);
        }
    }

    void RemoveEnemy(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }
}
