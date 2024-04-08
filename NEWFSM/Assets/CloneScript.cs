using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneScript : MonoBehaviour
{
    [Header("AttackRange")]
    public GameObject AttackPoint;
    public float radius;
    public LayerMask mainTarget;

    public int Damage;

    void Start()
    {
        Invoke("DestroyClone", 3f);
    }

    public void AttackTrigger()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(
            AttackPoint.transform.position,
            radius,
            mainTarget
        );

        foreach (Collider2D target in targets)
        {
            if (target.CompareTag("Enemy"))
            {
                target.GetComponent<Health>().TakeDamage(Damage);

                if (target.GetComponent<BossScript>() != null)
                {
                    BossScript bossScript = target.GetComponent<BossScript>();
                    bossScript.AddRage(5);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackPoint.transform.position, radius);
    }

    void DestroyClone()
    {
        Destroy(this.gameObject);
    }
}
