using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    private bool isSpawned = false;
    public GameObject[] enemies;
    public bool isPlayerFound;

    private int EnemyCount;
    private int EnemyDead;

    public Vector3 areaSize = new Vector3(50, 1, 50);

    void Start()
    {
        EnemyCount = enemies.Length;
        EnemyHit.OnEnemyDeath += OnEnemyDeath;
        MageShoot.OnEnemyDeath += OnEnemyDeath; // Subscribe to MageShoot's OnEnemyDeath event
    }

    void OnDestroy()
    {
        EnemyHit.OnEnemyDeath -= OnEnemyDeath;
        MageShoot.OnEnemyDeath -= OnEnemyDeath; // Unsubscribe to prevent memory leaks
    }

    void Update()
    {
        if (isPlayerFound && !isSpawned)
        {
            StartSpawn();
            isSpawned = true;
        }

        if (EnemyDead >= EnemyCount)
        {
            print("Success stage");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerFound = true;
        }
    }

    void StartSpawn()
    {
        foreach (GameObject enemy in enemies)
        {
            Vector3 spawnPosition = GetRandomPosition();
            Instantiate(enemy, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(-areaSize.x / 2, areaSize.x / 2);
        float y = Random.Range(-areaSize.y / 2, areaSize.y / 2);
        float z = Random.Range(-areaSize.z / 2, areaSize.z / 2);
        return new Vector3(x, y, z) + transform.position; // Offset by spawner position
    }

    private void OnEnemyDeath(GameObject enemy)
    {
        EnemyDead++;
    }
}
