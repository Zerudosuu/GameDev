using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float lifetime;

    public bool washit = false;

    void Start()
    {
        rb.velocity = transform.right * speed;
        Invoke(nameof(DestroyProjectile), lifetime);
    }

    // Update is called once per frame
    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
