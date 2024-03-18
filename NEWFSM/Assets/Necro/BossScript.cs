using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public GameObject AttackPoint;
    public float radius;
    public LayerMask mainTarget;

    public GameObject FireRain;

    public Transform[] attackpoints;

    public Rigidbody2D bullet;

    void Start() { }

    // Update is called once per frame
    void Update() { }

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
