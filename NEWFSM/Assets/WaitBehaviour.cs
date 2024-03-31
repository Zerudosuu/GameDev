using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitBehaviour : StateMachineBehaviour
{
    private IdleBehaviour idleBehaviour;
    private AgrroTrigger agrroTrigger;

    private float horizontal;

    public bool isFacingRight = true;

    private Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        rb = animator.GetComponent<Rigidbody2D>();
        idleBehaviour = animator.GetBehaviour<IdleBehaviour>();
        agrroTrigger = animator.gameObject.GetComponentInChildren<AgrroTrigger>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        horizontal = rb.velocity.x;
        if (idleBehaviour.waiting)
        {
            animator.SetBool("isWaiting", true);
        }
        else
        {
            animator.SetBool("isWaiting", false);
        }

        if (idleBehaviour.waiting)
        {
            idleBehaviour.waitTimer -= Time.deltaTime;

            if (idleBehaviour.waitTimer <= 0f)
            {
                animator.SetBool("isWaiting", false);
                idleBehaviour.waiting = false; // Reset waiting to false here
                idleBehaviour.targetPos = idleBehaviour.GetRandomPointBetween(
                    idleBehaviour.enemymovement.PA.position,
                    idleBehaviour.enemymovement.PB.position
                );

                // Set isWaiting to false after waiting
            }
        }

        if (agrroTrigger.isAggroed)
        {
            animator.SetBool("isChasing", true);

            if (idleBehaviour.isFacingRight)
            {
                idleBehaviour.Flip(animator); // Flip if facing right
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
