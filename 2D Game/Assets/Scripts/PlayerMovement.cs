using System.Collections;
using System.Data.Common;
using Unity.VisualScripting;
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
    public bool canDash = true;
    private bool isDashing;
    private float dashingPower = 30f;

    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private Animator animator;

    public AnimationClip isDeadClip;
    public AnimationClip wasHit;

    public GameObject playerprefab;

    public int LimitFps = 60;

    public float manaReductionAmount = 1f;
    public Animator BarricadeAnimator;

    private bool isOpen = false;

    private Mana mana;

    private Health pHealth;

    public float healAmount = 20f;

    void Start()
    {
        animator = GetComponent<Animator>();
        Application.targetFrameRate = LimitFps;
        BarricadeAnimator = GameObject
            .FindGameObjectWithTag("BarricadeContainer")
            .GetComponent<Animator>();

        mana = gameObject.GetComponent<Mana>();
        pHealth = gameObject.GetComponent<Health>();
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isOpen)
            {
                BarricadeAnimator.SetBool("isBarricadeOn", false);
                isOpen = false;
            }
            else
            {
                BarricadeAnimator.SetBool("isBarricadeOn", true);
                isOpen = true;
            }
        }

        if (isOpen)
        {
            mana.ReduceMana(manaReductionAmount * Time.deltaTime);
            if (mana.currentMana <= 0)
            {
                BarricadeAnimator.SetBool("isBarricadeOn", false);
                isOpen = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Heal();
        }
    }

    private void Heal()
    {
        Debug.Log("Healed");
        // Check if there's enough mana to exchange for health
        if (mana.currentMana >= healAmount && pHealth.currentHealth != pHealth.maxHealth)
        {
            // Increase player's health and decrease mana
            pHealth.currentHealth += healAmount;
            pHealth.healthBar.value = pHealth.currentHealth;
            mana.ReduceMana(healAmount);

            // Ensure player's health doesn't exceed max health
            pHealth.currentHealth = Mathf.Min(pHealth.currentHealth, pHealth.maxHealth);
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            Instantiate(playerprefab, transform.position, transform.rotation);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Health playerHealth = GetComponent<Health>();
            playerHealth.TakeDamage(2);

            Knockback(other);

            if (playerHealth.currentHealth <= 0)
            {
                StartCoroutine(DestroyWithAnimation());
            }
        }
    }

    IEnumerator DestroyWithAnimation()
    {
        animator.SetBool("isDead", true);

        yield return new WaitForSeconds(isDeadClip.length);

        Destroy(gameObject);
    }

    public void Knockback(Collider2D trig)
    {
        Vector3 direction = (transform.position - trig.transform.position).normalized;

        if (isFacingRight)
        {
            transform.Translate(trig.transform.right * 1);
        }
        else
        {
            transform.Translate(-trig.transform.right * 1);
        }

        StartCoroutine(TakenHitWithAnimation());
    }

    IEnumerator TakenHitWithAnimation()
    {
        // Play the "isDead" animation
        animator.SetBool("wasHit", true);

        // Wait for the duration of the "isDead" animation
        yield return new WaitForSeconds(wasHit.length);
        // Destroy the game object after the animation has played
        animator.SetBool("wasHit", false);
    }
}
