using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemybullet : MonoBehaviour
{
    public GameObject target;
    private Animator animator;
    private Rigidbody2D rb;

    public float distanceToTarget;
    public float force;

    public Vector3 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");

        direction = target.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        animator = GetComponent<Animator>();
    }

    void Update() { }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Set the "hasCollided" parameter to true, triggering the transition to "Break" animation
            animator.SetBool("hasCollided", true);

            rb.velocity = Vector2.zero;
            StartCoroutine(DestroyBulletAfterAnimation());
        }
    }

    IEnumerator DestroyBulletAfterAnimation()
    {
        // Wait for the duration of the "Break" animation
        yield return new WaitForSeconds(
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.length - .3f
        );

        // Destroy the bullet GameObject after the animation has played
        Destroy(gameObject);
    }
}
