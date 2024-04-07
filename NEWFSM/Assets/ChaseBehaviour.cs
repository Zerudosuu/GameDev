using UnityEngine;

public class ChaseBehaviour : StateMachineBehaviour
{
    private Transform Player;
    private Rigidbody2D RB;

    private AgrroTrigger agrroTrigger;
    private AttackTrigger attackTrigger;

    [SerializeField]
    private float MovementSpeed = 5f;

    private float horizontal;

    public bool isFacingRight = true;
    public float RandomMovementSpeed = 5f;
    public Vector3 targetPos;

    GroundCheck groundCheck;

    IdleBehaviour idleBehaviour;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        groundCheck = animator.GetComponentInChildren<GroundCheck>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        RB = animator.GetComponent<Rigidbody2D>();
        agrroTrigger = animator.gameObject.GetComponentInChildren<AgrroTrigger>();
        attackTrigger = animator.gameObject.GetComponentInChildren<AttackTrigger>();
        idleBehaviour = animator.GetBehaviour<IdleBehaviour>();
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

    public override void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        if (!groundCheck.isGrounded)
        {
            RB.velocity = Vector3.zero;
            agrroTrigger.isAggroed = false;
            animator.SetBool("isChasing", false);
        }

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

    private void Flip(Animator animator)
    {
        animator.transform.Rotate(0f, 180f, 0f);
        isFacingRight = !isFacingRight;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RB.velocity = Vector2.zero;
    }
}
