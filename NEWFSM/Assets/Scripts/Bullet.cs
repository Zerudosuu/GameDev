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

    void Start()
    {
        rb.velocity = transform.right * speed;
        Invoke(nameof(DestroyProjectile), lifetime);
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            print("Enemyhit");
            Destroy(gameObject);
        }
    }
}
