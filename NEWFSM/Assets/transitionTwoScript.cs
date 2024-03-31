using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transitionTwoScript : StateMachineBehaviour
{
    private CharacterHander characterHander;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        characterHander = GameObject
            .FindGameObjectWithTag("Player")
            .GetComponentInParent<CharacterHander>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        if (PlayerMovement.instance.isAttacking && characterHander.currentIndex == 0)
        {
            PlayerMovement.instance.animator.Play("SamuraiArcherAttack3");
        }
        else if (PlayerMovement.instance.isAttacking && characterHander.currentIndex == 1)
        {
            PlayerMovement.instance.animator.Play("SamuraiAttackThree");
        }
        else if (PlayerMovement.instance.isAttacking && characterHander.currentIndex == 2)
        {
            PlayerMovement.instance.animator.Play("SamuraiCommanderAttackThree");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement.instance.isAttacking = false;
        PlayerMovement.instance.canShoot = true;
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
