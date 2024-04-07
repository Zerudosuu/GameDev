using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBehaviour : StateMachineBehaviour
{
    IdleBehaviour idleBehaviour;
    AgrroTrigger agrroTrigger;

    private Transform Player;
    GroundCheck groundCheck;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        groundCheck = animator.GetComponentInChildren<GroundCheck>();
        agrroTrigger = animator.gameObject.GetComponentInChildren<AgrroTrigger>();
        idleBehaviour = animator.GetBehaviour<IdleBehaviour>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        if (idleBehaviour.waitTimer > 0f)
        {
            idleBehaviour.waitTimer -= Time.deltaTime;

            if (idleBehaviour.waitTimer <= 0f)
            {
                animator.SetBool("isWaiting", false);
            }

            if (agrroTrigger.isAggroed)
            {
                animator.SetBool("isWaiting", false);

                if (
                    Player.transform.position.x > animator.transform.position.x
                    && !idleBehaviour.isFacingRight
                )
                {
                    Flip(animator);
                }
                else if (
                    Player.transform.position.x < animator.transform.position.x
                    && idleBehaviour.isFacingRight
                )
                {
                    Flip(animator);
                }
            }
        }
    }

    public void Flip(Animator animator)
    {
        animator.transform.Rotate(0f, 180f, 0f);
        idleBehaviour.isFacingRight = !idleBehaviour.isFacingRight;
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
