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

    // Update is called once per frame
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
            Instantiate(projectile, firepoint.position, firepoint.rotation);
            chargeTime = 0;
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
