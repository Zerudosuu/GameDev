using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform player;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 20f;

    #region HitEffect

    public float tintFloat;

    private Material[] materials;
    private MeshRenderer meshRenderer;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    public bool ishit;

    private Rigidbody rb;
    #endregion

    #region PlayerFoundSequence
    SpawnEnemy spawnEnemy;
    public bool isPlayerFound;

    public delegate void EnemyDeathHandler(GameObject enemy);
    public static event EnemyDeathHandler OnEnemyDeath;
    #endregion

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        spawnEnemy = GameObject.Find("SpawnerSample").GetComponent<SpawnEnemy>();

        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Get materials from all SkinnedMeshRenderers
        materials = new Material[skinnedMeshRenderers.Length];
        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            materials[i] = skinnedMeshRenderers[i].material;
        }

        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    public void Shoot()
    {
        if (player != null)
        {
            // Instantiate the bullet at the bulletSpawnPoint's position and rotation
            GameObject bulletInstance = Instantiate(
                bulletPrefab,
                bulletSpawnPoint.position,
                bulletSpawnPoint.rotation
            );

            // Get the Rigidbody component of the instantiated bullet
            Rigidbody bulletRigidbody = bulletInstance.GetComponent<Rigidbody>();

            if (bulletRigidbody != null)
            {
                // Calculate direction from the bullet to the player, ignoring the Y position
                Vector3 targetPosition = new Vector3(
                    player.position.x,
                    bulletSpawnPoint.position.y,
                    player.position.z
                );
                Vector3 direction = (targetPosition - bulletSpawnPoint.position).normalized;

                // Set the velocity of the bullet towards the player
                bulletRigidbody.velocity = direction * bulletSpeed;
            }
            else
            {
                Debug.LogWarning("Bullet prefab does not have a Rigidbody component.");
            }
        }
        else
        {
            Debug.LogWarning("Player target not set for MageShoot.");
        }
    }

    void Update()
    {
        // Update shader property for each material
        foreach (Material material in materials)
        {
            material.SetFloat("_StrongTintFade", tintFloat);
        }

        isPlayerFound = spawnEnemy.isPlayerFound;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerCurrentWeapon>() != null)
        {
            Health heatlh = GetComponent<Health>();
            if (!ishit && !heatlh.isDead)
            {
                ishit = true;
                StartCoroutine(ChangeTintTemporarily());

                heatlh.TakeDamage(
                    other.gameObject.GetComponent<PlayerCurrentWeapon>().weapon.Damage
                );

                if (heatlh.isDead)
                {
                    NotifyDeath();
                }

                // Apply knockback
                Vector3 knockbackDirection = (
                    transform.position - other.transform.position
                ).normalized;
                ApplyKnockback(
                    knockbackDirection,
                    other.gameObject.GetComponent<PlayerCurrentWeapon>().weapon.knockback
                );
            }
        }
    }

    private IEnumerator ChangeTintTemporarily()
    {
        // Increase tintFloat by 5
        tintFloat += 5;

        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.2f);

        // Revert tintFloat to its original value
        tintFloat -= 5;

        // Reset ishit flag
        ishit = false;
    }

    private void ApplyKnockback(Vector3 direction, float KnockBackForce)
    {
        if (rb != null)
        {
            rb.AddForce(direction * KnockBackForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("No Rigidbody component found on this object.");
        }
    }

    private void NotifyDeath()
    {
        OnEnemyDeath?.Invoke(gameObject);
    }
}
