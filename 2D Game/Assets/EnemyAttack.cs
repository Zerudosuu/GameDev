using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public CircleCollider2D AttackCollider;

    private Animator animator;

    private Rigidbody2D rb;

    private bool willAttack;
    public GameObject AttackPoint;
    public float radius;
    public LayerMask mainTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (willAttack)
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
            Debug.Log("Hit Player");
        }
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("Player"))
        {
            willAttack = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        willAttack = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackPoint.transform.position, radius);
    }
}
