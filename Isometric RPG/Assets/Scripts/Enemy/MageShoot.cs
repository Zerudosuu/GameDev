using System.Collections;
using UnityEngine;

public class MageShoot : MonoBehaviour
{
    Skill skill;
    public bool isSkilling;
    public GameObject bulletPrefab;
    public Transform player;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 20f;

    #region HitEffect
    public float tintFloat;

    private Material[] materials;
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
        skill = GameObject.FindGameObjectWithTag("Player").GetComponent<Skill>();

        if (skill == null)
        {
            Debug.LogError("Skill component not found on Player GameObject");
            return;
        }
        player = GameObject.FindWithTag("Player").transform;
        spawnEnemy = GameObject.Find("SpawnerSample").GetComponent<SpawnEnemy>();

        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

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

        // Update the field based on the skill state
        if (skill != null)
        {
            isSkilling = skill.isSkillActive;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Check if the SphereCollider component is enabled
            SphereCollider sphereCollider = other.GetComponent<SphereCollider>();
            if (sphereCollider != null && sphereCollider.enabled)
            {
                print(other.gameObject.name);

                Health health = GetComponent<Health>();
                if (!ishit && health != null && !health.isDead)
                {
                    ishit = true;
                    print("Hit");

                    Stats stats = other.gameObject.GetComponent<Stats>();
                    float damage = CalculateCriticalDamage(stats, out bool isCritical);

                    health.TakeDamage(damage, isCritical);

                    if (health.isDead)
                    {
                        NotifyDeath();

                        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

                        float minXP = playerHealth.BaseExperience * 0.1f; // 10% of base XP
                        float maxXP = playerHealth.BaseExperience * 0.3f; // 30% of base XP
                        float awardedXP = Random.Range(minXP, maxXP); // Randomized XP within range

                        playerHealth.ExperienceUp(awardedXP);
                    }

                    StartCoroutine(ChangeTintTemporarily());

                    // Apply knockback
                    Vector3 knockbackDirection = (
                        transform.position - other.transform.position
                    ).normalized;
                    ApplyKnockback(knockbackDirection, stats.Knockback);
                }
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

    private float CalculateCriticalDamage(Stats stats, out bool isCriticalHit)
    {
        float damage = stats.baseDamage;

        if (Random.value <= stats.criticalChance)
        {
            damage *= stats.criticalDamage;
            isCriticalHit = true;
        }
        else
        {
            isCriticalHit = false;
        }

        return damage;
    }
}
