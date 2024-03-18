using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooterBehaviour : StateMachineBehaviour
{
    private Transform playerPosition;
    public float _bulletSpeed = 10f;
    private float timeRemaining;
    private BossScript bossScript; // Reference to the BossScript component
    private int bulletsShot = 0;
    private bool shootingFinished = false;
    private int currentAttackPointIndex = 0;

    public Rigidbody2D bulletPrefab;
    public float intervalBetweenBullets = 1f;
    public int maxBullets = 16;

    private float horizontal;
    private bool isFacingRight = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        bossScript = animator.GetComponent<BossScript>(); // Get the BossScript component from the Animator's GameObject
        shootingFinished = false;
        bulletsShot = 0;
        timeRemaining = intervalBetweenBullets;
        currentAttackPointIndex = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        if (!shootingFinished)
        {
            if (horizontal > 0 && isFacingRight == false)
            {
                Flip(animator);
            }
            else if (horizontal < 0 && isFacingRight == true)
            {
                Flip(animator);
            }
            if (bulletsShot < maxBullets)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }
                else
                {
                    // Access the current attack point from the BossScript component
                    Transform attackPoint = bossScript.attackpoints[currentAttackPointIndex];

                    Vector2 dir = (playerPosition.position - attackPoint.position).normalized;

                    Rigidbody2D bullet = GameObject.Instantiate(
                        bulletPrefab,
                        attackPoint.position,
                        Quaternion.identity
                    );

                    // Set the velocity of the bullet to move towards the player position
                    bullet.velocity = dir * _bulletSpeed;

                    // Increment the current attack point index or reset to 0 if reached the end
                    currentAttackPointIndex =
                        (currentAttackPointIndex + 1) % bossScript.attackpoints.Length;

                    timeRemaining = intervalBetweenBullets;
                    bulletsShot++;
                }
            }
            else
            {
                shootingFinished = true;
                // Set the animation parameter to false when shooting is finished
                animator.SetBool("isShootingProjectile", false);
            }
        }
    }

    private void Flip(Animator animator)
    {
        animator.transform.Rotate(0f, 180f, 0f);
        isFacingRight = !isFacingRight;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    ) { }
}
