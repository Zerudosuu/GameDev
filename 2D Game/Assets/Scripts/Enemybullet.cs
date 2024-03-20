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
        target = GameObject.FindGameObjectWithTag("Tower");

        direction = target.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        animator = GetComponent<Animator>();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            animator.SetBool("hasCollided", true);

            // Check if the bullet is moving before stopping it
            if (rb.velocity != Vector2.zero)
            {
                rb.velocity = Vector2.zero;
                StartCoroutine(DestroyBulletAfterAnimation());

                Health towerHealth = other.gameObject.GetComponent<Health>(); // get the health script of the tower
                if (towerHealth != null) // if there is a health script
                    towerHealth.TakeDamage(1); // call the TakeDamage method in the health script of the tower

                if (towerHealth.currentHealth <= 0)
                {
                    GameManager gameManager = FindAnyObjectByType<GameManager>();
                    gameManager.GameOver();
                }
            }
        }
    }
}
