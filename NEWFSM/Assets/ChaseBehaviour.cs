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
        horizontal = RB.velocity.x;
        if (!agrroTrigger.isAggroed)
        {
            animator.SetBool("isChasing", false);
        }

        if (attackTrigger.isInAttackRange)
        {
            RB.velocity = Vector2.zero;
            animator.SetBool("isAttacking", true);
        }

        Vector3 direction = (Player.transform.position - animator.transform.position).normalized;
        RB.velocity = direction * RandomMovementSpeed;
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
