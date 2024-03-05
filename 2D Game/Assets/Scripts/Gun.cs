using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

    public bool isCharging;

    public float timeRemaining = 5;

    private Animator animator;

    // Update is called once per frame


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && chargeTime < 5)
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
            Instantiate(projectile, firepoint.position, firepoint.rotation);
            chargeTime = 0;
            animator.SetBool("isAttacking", false);
        }
        else if (Input.GetMouseButtonUp(0) && chargeTime >= 2)
        {
            ReleaseCharge();
        }
    }

    void ReleaseCharge()
    {
        Instantiate(chargedProjectile, firepoint.position, firepoint.rotation);
        isCharging = false;

        chargeTime = 0;
    }
}
