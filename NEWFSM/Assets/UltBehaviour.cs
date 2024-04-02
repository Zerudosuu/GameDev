using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UltBehaviour : StateMachineBehaviour
{
    private ParticleSystem particleSys;
    Rigidbody2D rigidbody2D;

    CapsuleCollider2D capsuleCollider2D;
    public float TimeRemaining = 10;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        capsuleCollider2D = animator.GetComponent<CapsuleCollider2D>();
        capsuleCollider2D.enabled = false;
        TimeRemaining = 10;
        rigidbody2D = animator.GetComponent<Rigidbody2D>();
        particleSys = GameObject.FindObjectOfType<ParticleSystem>();

        rigidbody2D.velocity = Vector2.zero;
        particleSys.Play();
    }

    // Coroutine to stop the particle system after a delay


    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        if (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
        }
        else
        {
            particleSys.Stop();
            animator.SetBool("Ulting", false);
            capsuleCollider2D.enabled = true;
        }
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
