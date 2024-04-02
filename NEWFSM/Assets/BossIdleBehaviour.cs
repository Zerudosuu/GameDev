using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleBehaviour : StateMachineBehaviour
{
    private Transform playerLoc;
    private float timeRemaining;

    private bool hasDecided;
    private int Decision;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        playerLoc = GameObject.FindGameObjectWithTag("Player").transform;
        timeRemaining = 2f;
        hasDecided = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        timeRemaining -= Time.deltaTime;
        if (!hasDecided && timeRemaining <= 0)
        {
            DecisionRandom();
            hasDecided = true;
        }

        if (Decision == 1)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    public void DecisionRandom()
    {
        Decision = Random.Range(1, 2);
    }

    public override void OnStateExit(
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
