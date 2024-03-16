using UnityEngine;

public class ChaseBehaviour : StateMachineBehaviour
{
    private Transform Player;
    private Rigidbody2D RB;

    private AgrroTrigger agrroTrigger;
    private AttackTrigger attackTrigger;

    [SerializeField]
    private float MovementSpeed = 5f;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        RB = animator.GetComponent<Rigidbody2D>();
        agrroTrigger = animator.gameObject.GetComponentInChildren<AgrroTrigger>();
        attackTrigger = animator.gameObject.GetComponentInChildren<AttackTrigger>();
    }

    public override void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        if (!agrroTrigger.isAggroed)
        {
            animator.SetBool("isChasing", false);
        }

        if (attackTrigger.isInAttackRange)
        {
            RB.velocity = Vector2.zero;
            animator.SetBool("isAttacking", true);
        }

        Vector2 moveDirection = (Player.position - animator.transform.position).normalized;

        RB.velocity = moveDirection * MovementSpeed;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RB.velocity = Vector2.zero;
    }
}
