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

    public delegate void EnemyDeathHandler(GameObject enemy);
    public static event EnemyDeathHandler OnEnemyDeath;

    void Start()
    {
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

                    PlayerCurrentWeapon playerWeapon = other.GetComponent<PlayerCurrentWeapon>();
                    if (playerWeapon != null)
                    {
                        health.TakeDamage(playerWeapon.weapon.Damage);

                        if (health.isDead)
                        {
                            NotifyDeath();
                        }

                        StartCoroutine(ChangeTintTemporarily());

                        // Apply knockback
                        Vector3 knockbackDirection = (
                            transform.position - other.transform.position
                        ).normalized;
                        ApplyKnockback(knockbackDirection, playerWeapon.weapon.knockback);
                    }
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
}
