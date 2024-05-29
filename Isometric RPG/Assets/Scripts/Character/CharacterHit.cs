using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHit : MonoBehaviour
{
    #region HitEffect
    public float tintFloat;
    private Material[] materials;
    private MeshRenderer meshRenderer;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    private Animator animator;

    private bool isHit;
    private Rigidbody rb;
    #endregion


    PlayerCurrentWeapon playerCurrentWeapon;

    void Start()
    {
        playerCurrentWeapon = GetComponent<PlayerCurrentWeapon>();
        rb = GetComponent<Rigidbody>();
        animator = gameObject?.GetComponent<Animator>();

        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();

        materials = new Material[skinnedMeshRenderers.Length];
        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            materials[i] = skinnedMeshRenderers[i].material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update shader property for each material
        foreach (Material material in materials)
        {
            material.SetFloat("_StrongTintFade", tintFloat);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyWeapon enemyWeapon = other.gameObject.GetComponent<EnemyWeapon>();
        BulletScript bulletScript = other.gameObject.GetComponent<BulletScript>();

        if (enemyWeapon != null || bulletScript != null)
        {
            if (!isHit)
            {
                isHit = true; // Ensure isHit is set to true to prevent re-triggering immediately

                // Trigger the animation
                animator.SetTrigger("Washit");

                // Get the Health component of the current object
                Health health = GetComponent<Health>();

                if (health != null)
                {
                    // Determine the damage based on the component present
                    float damage = enemyWeapon != null ? enemyWeapon.Damage : bulletScript.Damage;

                    // Apply the damage
                    health.TakeDamage(damage);

                    // Log for debugging
                    Debug.Log($"Damage taken: {damage}");

                    // Start the coroutine to change tint temporarily
                    StartCoroutine(ChangeTintTemporarily());

                    // Calculate the knockback direction and apply knockback
                    Vector3 knockbackDirection = (
                        transform.position - other.transform.position
                    ).normalized;
                    ApplyKnockback(knockbackDirection);
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
        isHit = false;
    }

    private void ApplyKnockback(Vector3 direction)
    {
        if (rb != null)
        {
            rb.AddForce(direction * playerCurrentWeapon.weapon.knockback, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("No Rigidbody component found on this object.");
        }
    }
}
