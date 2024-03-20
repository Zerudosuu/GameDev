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

    public GameObject boomprefab;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(boomprefab, transform.position, transform.rotation);
        }
    }
}
