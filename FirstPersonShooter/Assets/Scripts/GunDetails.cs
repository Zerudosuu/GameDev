using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunDetails : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet prefab to instantiate
    public Transform bulletSpawnPoint; // The position from where the bullet will be spawned
    public LayerMask aimColliderLayerMask; // Layer mask to determine what the bullets can hit
    public float bulletSpeed; // Speed of the bullet
    public float fireRate; // Time between shots
    public int maxAmmo; // Maximum ammo capacity
    public float reloadTime; // Time it takes to reload
    public AudioClip shootSound; // Sound to play when shooting

    public float Range;
    public float Damage;
}
