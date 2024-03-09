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
    private Transform mainTargetPosition;

    public bool willAttack;

    //public int Health = 3;

    public float speed = 2;

    public Rigidbody2D rb;

    #endregion



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        }
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
    }
}
