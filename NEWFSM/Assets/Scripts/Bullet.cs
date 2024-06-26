using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float lifetime;

    UiManager uiManager;

    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIController").GetComponent<UiManager>();
        rb.velocity = transform.right * speed;
        Invoke(nameof(DestroyProjectile), lifetime);
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(10);

            if (health.currentHealth <= 0)
            {
                if (other.transform.parent != null)
                {
                    uiManager.EnemyCount--;
                    uiManager.UpdateEnemyCount();
                    Destroy(other.transform.parent.gameObject);
                }
            }

            Destroy(gameObject);
            // Check if the parent exists before destroying it
        }

        if (other.GetComponent<BossScript>() != null)
        {
            BossScript bossScript = other.GetComponent<BossScript>();
            bossScript.AddRage(2);
        }
    }
}
