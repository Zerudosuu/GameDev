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

    public Transform attackPoint1;
    public Transform attackPoint2;
    private Transform mainTargetPosition;
    public Transform AttackPointFinal;
    public float movementSpeed = 1f;

    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        attackPoint1 = GameObject.FindGameObjectWithTag("AttackPoint1")?.transform;
        attackPoint2 = GameObject.FindGameObjectWithTag("AttackPoint2")?.transform;

        mainTargetPosition = GameObject.FindGameObjectWithTag("Tower")?.transform;
        if (attackPoint1 == null || attackPoint2 == null)
        {
            Debug.LogError("Attack points not found or tagged correctly.");
            return;
        }

        // Check the enemy's initial position and set the appropriate attack point
        if (transform.position.x < 0)
        {
            AttackPointFinal = attackPoint1;
        }
        else
        {
            AttackPointFinal = attackPoint2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition != AttackPointFinal.position)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                AttackPointFinal.position,
                Time.deltaTime * movementSpeed
            );
        }

        if (transform.localPosition == AttackPointFinal.position)
        {
            willAttack = true;
        }
        else
        {
            willAttack = false;
        }
        if (willAttack)
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }

        if (mainTargetPosition.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }
        else if (mainTargetPosition.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        isFacingRight = !isFacingRight;
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
                Debug.Log("Tower was hit!");
            }
        }
    }

    // void OnTriggerStay2D(Collider2D trig)
    // {
    //     if (trig.gameObject.CompareTag("Tower"))
    //     {
    //         willAttack = true;
    //     }
    // }

    // void OnTriggerExit2D(Collider2D other)
    // {
    //     willAttack = false;
    // }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackPoint.transform.position, radius);
    }
}
