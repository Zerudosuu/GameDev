using System.Collections;
using TMPro;
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

    public TextMeshProUGUI chargingText;

    public float chargedBulletCooldown = 5f; // Cooldown duration for the charged bullet
    private float cooldownTimer = 0f; // Timer to track cooldown progress

    void Start()
    {
        animator = GetComponent<Animator>();
        mana = GetComponent<Mana>();

        chargingText.text = "";
    }

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                cooldownTimer = 0;
                chargingText.text = ""; // Clear the cooldown message when cooldown ends
            }
        }

        if (
            Input.GetMouseButton(0)
            && chargeTime < 5
            && mana.currentMana >= chargedBulletManaCost
            && cooldownTimer <= 0
        )
        {
            isCharging = true;
            chargeTime += Time.deltaTime * chargeSpeed;

            chargingText.text = "Charging " + chargeTime.ToString("F2") + "s"; // Display the current charge time

            if (chargeTime > 5)
            {
                chargingText.text = "Release to Fire!";
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            chargingText.text = "";
            animator.SetBool("isAttacking", true);
            chargeTime = 0;
        }
        else if (
            Input.GetMouseButtonUp(0)
            && chargeTime >= 2
            && mana.currentMana >= chargedBulletManaCost
            && cooldownTimer <= 0
        )
        {
            ReleaseCharge();
        }
    }

    void ReleaseCharge()
    {
        chargingText.text = "cooldown...";

        Instantiate(chargedProjectile, firepoint.position, firepoint.rotation);
        isCharging = false;

        // Deduct mana cost
        mana.ReduceMana(chargedBulletManaCost);

        chargeTime = 0;
        cooldownTimer = chargedBulletCooldown; // Start cooldown timer
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
