using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float minForce = 10f;
    public float maxForce = 50f;
    public float maxChargeTime = 2.0f;

    private float chargeStartTime;
    private bool charging = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCharging();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            FireProjectile();
        }
        else if (charging)
        {
            UpdateCharge();
        }
    }

    void StartCharging()
    {
        charging = true;
        chargeStartTime = Time.time;
    }

    void UpdateCharge()
    {
        float chargeTime = Time.time - chargeStartTime;
        float chargeRatio = Mathf.Clamp01(chargeTime / maxChargeTime);
        float force = Mathf.Lerp(minForce, maxForce, chargeRatio);
        // You can visualize the charge level or do other feedback here if needed

        Debug.Log("Charging: " + chargeRatio);

        // You can also play charging sound, show visual feedback, etc.
    }

    void FireProjectile()
    {
        if (!charging)
            return;

        float chargeTime = Time.time - chargeStartTime;
        float chargeRatio = Mathf.Clamp01(chargeTime / maxChargeTime);
        float force = Mathf.Lerp(minForce, maxForce, chargeRatio);

        GameObject projectile = Instantiate(
            projectilePrefab,
            transform.position,
            transform.rotation
        );
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * force;
        }

        // Reset charging state
        charging = false;
    }
}
