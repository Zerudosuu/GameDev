using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseBehaviour : StateMachineBehaviour
{
    private Transform playerPosition;
    private AttackTrigger attackTrigger;

    private float MovementSpeed = 5f;

    private int Decision;
    private bool isFacingRight = false;
    private Rigidbody2D RB;

    private float horizontal;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        attackTrigger = animator.gameObject.GetComponentInChildren<AttackTrigger>();
        RB = animator.GetComponent<Rigidbody2D>();

        Decision = Random.Range(1, 4);

        Debug.Log(Decision);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        horizontal = RB.velocity.x;
        if (Decision == 1)
        {
            if (horizontal > 0 && isFacingRight == false)
            {
                Flip(animator);
            }
            else if (horizontal < 0 && isFacingRight == true)
            {
                Flip(animator);
            }

            Vector2 moveDirection = (
                playerPosition.position - animator.transform.position
            ).normalized;

            RB.velocity = moveDirection * MovementSpeed;

            if (attackTrigger.isInAttackRange)
            {
                animator.SetBool("isCleaving", true);
            }
        }
        else if (Decision == 2)
        {
            RB.velocity = Vector2.zero;
            animator.SetBool("isShootingProjectile", true);
        }
        else
        {
            animator.SetBool("isShootingProjectile", false);
            animator.SetBool("isMoving", false);
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

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
