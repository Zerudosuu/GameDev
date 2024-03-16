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

    void Start()
    {
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
                Debug.Log("Player was hit!");
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackPoint.transform.position, radius);
    }
}
