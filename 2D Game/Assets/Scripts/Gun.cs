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

    // Method to be called from the Animation Event
    void ResetAnimation()
    {
        animator.SetBool("isAttacking", false);
        animator.Play("Slingshot"); // Replace "YourAnimationName" with the actual animation name
    }

    // Method to set up the Animation Event
    void SetupAnimationEvent()
    {
        AnimationClip clip = animator.runtimeAnimatorController.animationClips[6]; // Assuming it's the first animation clip
        AnimationEvent animationEvent = new AnimationEvent
        {
            functionName = "ResetAnimation",
            time = 0
        };
        clip.AddEvent(animationEvent);
    }
}
