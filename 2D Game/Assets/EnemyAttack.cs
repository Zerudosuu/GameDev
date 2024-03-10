using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Animator animator;

    public GameObject AttackPoint;
    public float radius;
    public LayerMask mainTarget;

    private enemyMove enemy;

    public int damage;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = FindObjectOfType<enemyMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.willAttack)
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }

    public void EndAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    public void Attack()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(
            AttackPoint.transform.position,
            radius,
            mainTarget
        );

        foreach (Collider2D target in targets)
        {
            if (target.CompareTag("Tower"))
            {
                Health towerHealth = target.GetComponent<Health>(); // get the health scriptsa tower
                if (towerHealth != null) // if may health script na nakua
                    towerHealth.TakeDamage(damage); // call the takeDamage method sa health script kang tower
                GameManager gameManager = FindAnyObjectByType<GameManager>();

                if (gameManager != null)
                {
                    if (towerHealth.currentHealth <= 0)
                    {
                        gameManager.GameOver();
                    }
                }
            }

            if (target.CompareTag("Player"))
            {
                Debug.Log("Player was hit!");
                Health PlayerHealth = target.GetComponent<Health>(); // get the health scriptsa tower
                if (PlayerHealth != null) // if may health script na nakua
                    PlayerHealth.TakeDamage(damage); // call the takeDamage method sa health script kang tower

                GameManager gameManager = FindAnyObjectByType<GameManager>();

                if (gameManager != null)
                {
                    if (PlayerHealth.currentHealth <= 0)
                    {
                        gameManager.GameOver();
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackPoint.transform.position, radius);
    }
}
