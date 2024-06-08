using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnConfig", menuName = "ScriptableObjects/SpawnConfig", order = 1)]
public class Spawn : ScriptableObject
{
    public List<SpawnEnemies> Enemies;
}

[System.Serializable]
public class SpawnEnemies
{
    public GameObject Enemy;
    public SpawnCount spawnCount;
}

[System.Serializable]
public enum SpawnCount
{
    Few = 3,
    Some = 5,
    Many = 10
}
