using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseShooterBehaviour : StateMachineBehaviour
{
    IdleShooterBehaviour idleBehaviour;

    public bool isFacingRight = true;

    private Transform Player;
    private Rigidbody2D RB;

    private AgrroTrigger agrroTrigger;
    private AttackTrigger attackTrigger;

    public float RandomMovementSpeed = 5f;

    private float horizontal;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        RB = animator.GetComponent<Rigidbody2D>();
        agrroTrigger = animator.gameObject.GetComponentInChildren<AgrroTrigger>();
        attackTrigger = animator.gameObject.GetComponentInChildren<AttackTrigger>();
        idleBehaviour = animator.GetBehaviour<IdleShooterBehaviour>();
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

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        horizontal = RB.velocity.x;
        if (!agrroTrigger.isAggroed)
        {
            animator.SetBool("isChasing", false);
            // Move towards the target position if not aggroed
            Vector3 direction = (idleBehaviour.targetPos - animator.transform.position).normalized;
            RB.velocity = direction * RandomMovementSpeed;
        }
        else
        {
            // Move towards the player if aggroed
            Vector3 direction = (
                Player.transform.position - animator.transform.position
            ).normalized;
            RB.velocity = direction * RandomMovementSpeed;
        }

        if (attackTrigger.isInAttackRange)
        {
            RB.velocity = Vector2.zero;
            animator.SetBool("isAttacking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    ) { }

    private void Flip(Animator animator)
    {
        animator.transform.Rotate(0f, 180f, 0f);
        isFacingRight = !isFacingRight;
    }

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
