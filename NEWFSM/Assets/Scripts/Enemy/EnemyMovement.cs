using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform PA;
    public Transform PB;

    private AttackTrigger attackTrigger;

    public GameObject AttackPoint;
    public float radius;
    public LayerMask mainTarget;

    public Rigidbody2D rb;

    AgrroTrigger agrroTrigger;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        agrroTrigger = GetComponentInChildren<AgrroTrigger>();
        // Find the AttackTrigger component in the children of this game object
        attackTrigger = GetComponentInChildren<AttackTrigger>();
        if (attackTrigger == null)
        {
            Debug.LogError("AttackTrigger component not found!");
        }
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
            if (target.CompareTag("Player"))
            {
                CharacterHander CharHander = target.GetComponent<CharacterHander>();
                PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();

                CloneHealth cloneHealth = target.GetComponent<CloneHealth>();

                if (cloneHealth != null)
                {
                    cloneHealth.TakeDamage(10);
                }

                CharHander.IncrementDamageTaken(3);
                playerHealth.TakeDamage(3);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackPoint.transform.position, radius);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            this.agrroTrigger.isAggroed = true;
            print("Im hit!");
        }
    }
}
