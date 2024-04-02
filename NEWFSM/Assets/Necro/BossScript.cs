using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    public GameObject AttackPoint;
    public float radius;
    public LayerMask mainTarget;

    public GameObject FireRain;

    public Transform[] attackpoints;

    public Rigidbody2D bullet;

    public Slider rageSlider;

    public int RagePoint = 0;

    void Start()
    {
        rageSlider.value = RagePoint;
        rageSlider.maxValue = 100;
    }

    public void AddRage(int RP)
    {
        RagePoint += RP;
        rageSlider.value = RagePoint;

        if (RagePoint > 100)
        {
            RagePoint = 100;
        }
    }

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
                CharacterHander CharHander = target.GetComponent<CharacterHander>();
                PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();
                PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();

                CharHander.IncrementDamageTaken(15);
                playerHealth.TakeDamage(15);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackPoint.transform.position, radius);
    }
}
