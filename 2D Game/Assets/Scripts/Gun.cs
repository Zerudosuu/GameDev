using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private Transform firepoint;

    [Header("Charged Bullet")]
    [SerializeField]
    private GameObject chargedProjectile;

    [SerializeField]
    private float chargeSpeed;

    [SerializeField]
    private float chargeTime;

    [SerializeField]
    private int chargedBulletManaCost; // Mana cost for the charged bullet

    public bool isCharging;
    public float timeRemaining = 5;

    private Animator animator;
    private Mana mana;

    void Start()
    {
        animator = GetComponent<Animator>();
        mana = GetComponent<Mana>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && chargeTime < 5 && mana.currentMana >= chargedBulletManaCost)
        {
            isCharging = true;

            if (isCharging)
            {
                chargeTime += Time.deltaTime * chargeSpeed;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isAttacking", true);
            chargeTime = 0;
        }
        else if (
            Input.GetMouseButtonUp(0)
            && chargeTime >= 2
            && mana.currentMana >= chargedBulletManaCost
        )
        {
            ReleaseCharge();
        }
    }

    void ReleaseCharge()
    {
        Instantiate(chargedProjectile, firepoint.position, firepoint.rotation);
        isCharging = false;

        // Deduct mana cost
        mana.ReduceMana(chargedBulletManaCost);

        chargeTime = 0;
    }

    public void Shoot()
    {
        Instantiate(projectile, firepoint.position, firepoint.rotation);
    }

    public void EndAttack()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isMoving", true);
        animator.SetBool("isJumping", false);
    }
}
