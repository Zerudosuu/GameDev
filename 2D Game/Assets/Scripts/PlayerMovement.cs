using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 10f;
    private float jumpingPower = 24f;
    private bool isFacingRight = true;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundLayer;

    [Header("DashingProperty")]
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 30f;

    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal > 0.1f || horizontal < -0.1f)
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isJumping", false);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetBool("isJumping", true);
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", false);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            animator.SetBool("isJumping", false);
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttacking", false);
        }

        if (horizontal > 0 && isFacingRight == false)
        {
            Flip();
        }
        else if (horizontal < 0 && isFacingRight == true)
        {
            Flip();
        }

        if (IsGrounded())
        {
            animator.SetBool("isJumping", false);
        }

        if (Input.GetMouseButtonDown(1) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        isFacingRight = !isFacingRight;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Determine the dash direction based on the player's facing direction
        int dashDirection = isFacingRight ? 1 : -1;

        rb.velocity = new Vector2(dashDirection * dashingPower, 0f);

        yield return new WaitForSeconds(dashingTime);

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
