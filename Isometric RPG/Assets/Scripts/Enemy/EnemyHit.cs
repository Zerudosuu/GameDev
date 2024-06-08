using System.Collections;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public float tintFloat;
    public float knockbackForce = 10f;

    private Material[] materials;
    private MeshRenderer meshRenderer;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    SpawnEnemy spawnEnemy;
    public bool isPlayerFound;
    public bool ishit;

    private Rigidbody rb;

    Skill skill;

    public bool isSkilling;

    public delegate void EnemyDeathHandler(GameObject enemy);
    public static event EnemyDeathHandler OnEnemyDeath;

    void Start()
    {
        skill = GameObject.FindGameObjectWithTag("Player").GetComponent<Skill>();

        if (skill == null)
        {
            Debug.LogError("Skill component not found on Player GameObject");
            return;
        }
        spawnEnemy = GameObject.Find("SpawnerSample").GetComponent<SpawnEnemy>();
        // Get all SkinnedMeshRenderer components in children
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
                    print("Hotdog");

                    Stats stats = other.gameObject.GetComponent<Stats>();
                    float damage = CalculateCriticalDamage(stats, out bool isCritical);

                    health.TakeDamage(damage, isCritical);

                    if (health.isDead)
                    {
                        NotifyDeath();

                        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

                        float minXP = playerHealth.BaseExperience * 0.1f; // 80% of base XP
                        float maxXP = playerHealth.BaseExperience * 0.3f; // 150% of base XP
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
