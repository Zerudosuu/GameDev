using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMove : MonoBehaviour
{
    // Start is called before the first frame update

    #region Public Properties
    public float movementSpeed = 1f;

    private bool isFacingRight = true;

    public Transform AttackPointFinal;

    public Transform attackPoint1;
    public Transform attackPoint2;
    public Transform mainTargetPosition;

    public bool willAttack;

    //public int Health = 3;


    public Rigidbody2D rb;

    private Animator animator;

    public AnimationClip isDeadClip;
    public AnimationClip wasHit;
    public GameObject manaCrystalPrefab;
    public float manaCrystalDropProbability = 0.5f;

    #endregion



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Update

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

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        isFacingRight = !isFacingRight;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition != AttackPointFinal.position)
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttacking", false);
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                AttackPointFinal.position,
                Time.deltaTime * movementSpeed
            );
        }

        if (transform.localPosition == AttackPointFinal.position)
        {
            willAttack = true;
            if (willAttack)
            {
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking", true);
            }
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

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("Bullet"))
        {
            Health enemyHealth = GetComponent<Health>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(1);

            // Apply knockback force
            Knockback(trig);
            // rb.AddForce(direction * speed, ForceMode2D.Impulse);

            if (enemyHealth.currentHealth <= 0)
            {
                rb.velocity = Vector2.zero;
                StartCoroutine(DestroyWithAnimation());
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ChargedBullet"))
        {
            Health enemyHealth = GetComponent<Health>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(2);

            // Apply knockback forc
            // rb.AddForotherce(direction * speed, ForceMode2D.Impulse);

            if (enemyHealth.currentHealth <= 0)
            {
                rb.velocity = Vector2.zero;
                StartCoroutine(DestroyWithAnimation());
            }
        }
    }

    IEnumerator DestroyWithAnimation()
    {
        // Play the "isDead" animation
        animator.SetBool("isDead", true);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isMoving", false);

        // Wait for the duration of the "isDead" animation
        yield return new WaitForSeconds(isDeadClip.length);
        // Destroy the game object after the animation has played

        // Check if the mana crystal should be dropped based on the probability
        if (UnityEngine.Random.value < manaCrystalDropProbability)
        {
            DropManaCrystal();
        }
        Destroy(gameObject);
    }

    public void Knockback(Collider2D trig)
    {
        Vector3 direction = (transform.position - trig.transform.position).normalized;

        if (isFacingRight)
        {
            transform.Translate(trig.transform.right * 1);
        }
        else
        {
            transform.Translate(-trig.transform.right * 1);
        }

        Destroy(trig.gameObject);

        StartCoroutine(TakenHitWithAnimation());
    }

    IEnumerator TakenHitWithAnimation()
    {
        // Play the "isDead" animation
        animator.SetBool("wasHit", true);

        // Wait for the duration of the "isDead" animation
        yield return new WaitForSeconds(wasHit.length);
        // Destroy the game object after the animation has played
        animator.SetBool("wasHit", false);
    }

    private void DropManaCrystal()
    {
        // Instantiate mana crystal at the enemy's position
        if (manaCrystalPrefab != null)
        {
            Instantiate(manaCrystalPrefab, transform.position, Quaternion.identity);
        }
    }
}
