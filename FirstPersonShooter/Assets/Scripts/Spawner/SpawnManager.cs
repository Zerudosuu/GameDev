using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public SpawnLocations[] spawnLocations;
}

[System.Serializable]
public class SpawnLocations
{
    public Transform[] transformSpawnLocation;
}
