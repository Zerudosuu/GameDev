using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPosition;
    public float timer = 5;

    private bool isFacingRight = true;

    private Animator animator;

    #region movement

    public float movementSpeed = 1f;

    public Transform AttackPointFinal;

    public Transform[] AttackPoints;

    private Transform mainTargetPosition;

    public bool willAttack;

    //public int Health = 3;

    public float speed = 2;

    public Rigidbody2D rb;

    public AnimationClip isDeadClip;
    public AnimationClip wasHit;

    public int Damage;

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject[] flyingAttackPointObjects = GameObject.FindGameObjectsWithTag(
            "FlyingAttackPoint"
        );

        mainTargetPosition = GameObject.FindGameObjectWithTag("Tower")?.transform;
        AttackPoints = new Transform[flyingAttackPointObjects.Length];
        for (int i = 0; i < flyingAttackPointObjects.Length; i++)
        {
            AttackPoints[i] = flyingAttackPointObjects[i].transform;
        }

        // Select a random AttackPointFinal from the attackPoints array
        AttackPointFinal = AttackPoints[Random.Range(0, AttackPoints.Length)];
        animator = GetComponent<Animator>();
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
            if (timer <= 2) // Corrected condition
            {
                animator.SetBool("isAttacking", true);
                StartCoroutine(AttackAnimation());

                timer = 5;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }

        if (mainTargetPosition.transform.position.x < transform.position.x && isFacingRight == true)
        {
            Flip();
        }
        else if (
            mainTargetPosition.transform.position.x > transform.position.x
            && isFacingRight == false
        )
        {
            Flip();
        }
    }

    IEnumerator AttackAnimation()
    {
        // Wait for the duration of the "Break" animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // Destroy the bullet GameObject after the animation has played

        Shoot();
        animator.SetBool("isAttacking", false);
    }

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        isFacingRight = !isFacingRight;
    }

    void Shoot()
    {
        Instantiate(bullet, bulletPosition.position, UnityEngine.Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("Bullet"))
        {
            Health enemyHealth = GetComponent<Health>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(Damage);

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

        // Wait for the duration of the "isDead" animation
        yield return new WaitForSeconds(isDeadClip.length);
        // Destroy the game object after the animation has played
        Destroy(gameObject);
    }

    public void Knockback(Collider2D trig)
    {
        Vector3 direction = (transform.position - trig.transform.position).normalized;

        if (isFacingRight)
        {
            transform.Translate(trig.transform.right * 2);
        }
        else
        {
            transform.Translate(-trig.transform.right * 2);
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
}
