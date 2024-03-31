using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    private float horizontal;
    private float currentSpeed = 0f;

    [Header("Movement")]
    public float acceleration; // Adjust this value to control acceleration rate
    public float maxSpeed; // Maximum normal speed of the player
    public float sprintSpeed; // Speed when sprinting
    public float jumpingPower;

    public int Damage;

    public int healthpoints;

    [Space(10)]
    [Header("Check"),]
    private bool isFacingRight = true;
    private bool jump;
    private bool grounded;
    private bool wasGrounded = true;

    public bool canShoot = false;

    public bool isAttacking = false;

    public bool isinMelleRange = false;

    public bool isRange;

    [Header("AttackRange")]
    public GameObject AttackPoint;
    public float radius;
    public LayerMask mainTarget;

    [Space(10)]
    [Header("Rigidbody")]
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    public Animator animator;

    [SerializeField]
    private Transform spawnArrow;

    [Space(10)]
    [Header("DashingProperty")]
    public bool canDash = true;
    private bool isDashing;
    public float dashingPower = 30f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [Space(10)]
    [Header("RayCast")]
    public float raycastDistance = 5f;
    public LayerMask layerMask;

    [Space(20)]
    [Header("Character")]
    CharacterHander characters;
    public Rigidbody2D arrow;

    public float TimeRemaining = .40f;

    MeleeScript meleeScript;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        meleeScript = GetComponentInChildren<MeleeScript>();
        characters = GetComponent<CharacterHander>();
        characters.UpdateCharacter();
    }

    void Update()
    {
        isinMelleRange = meleeScript.canAttack;

        if (isinMelleRange)
        {
            canShoot = false;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        jump = Input.GetButtonDown("Jump");

        // Set animation parameters
        animator.SetFloat("Speed", Mathf.Abs(currentSpeed));
        animator.SetBool("Grounded", grounded);
        animator.SetBool("Falling", !grounded && rb.velocity.y < 0f);

        Vector2 raycastDirection = isFacingRight ? Vector2.right : Vector2.left;

        // Cast a ray in the appropriate direction from the raycast origin
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            raycastDirection,
            raycastDistance,
            layerMask
        );

        // Draw the ray in the scene view for visualization purposes
        Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.green);

        // Check if the ray hit something
        if (hit.collider != null)
        {
            // Handle the collision
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            canShoot = true;
            print(canShoot);
        }
        else
        {
            canShoot = false;
        }

        if (isDashing)
            return;

        if (jump && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetBool("Jump", true);
            grounded = false;
        }

        if (grounded)
        {
            animator.SetBool("Jump", false);
        }
        if (Input.GetKey(KeyCode.LeftShift) && horizontal != 0)
        {
            currentSpeed = sprintSpeed * Mathf.Sign(horizontal);
        }
        else
        {
            currentSpeed = maxSpeed * horizontal;
        }

        if (horizontal > 0 && !isFacingRight || horizontal < 0 && isFacingRight)
            Flip();

        if (Input.GetMouseButtonDown(1) && canDash)
            StartCoroutine(Dash());

        if (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime; // Decrease the time remaining
        }

        if (
            Input.GetMouseButtonDown(0)
            && isRange
            && canShoot
            && TimeRemaining <= 0
            && !isinMelleRange
        )
        {
            animator.SetBool("ShootingArrow", true);
            StartCoroutine(ShootAndResetTimer());
        }

        if (Input.GetMouseButton(0) && isinMelleRange && !isAttacking)
        {
            Attack();
        }
    }

    IEnumerator ShootAndResetTimer()
    {
        // Wait for the duration of the shooting animation
        yield return new WaitForSeconds(.40f); // Adjust shootingAnimationDuration as needed
        Shoot();
        canShoot = false;
        // Set shootingArrow back to false after the animation duration
        animator.SetBool("ShootingArrow", false);

        // Reset the time remaining to the interval after shooting
        TimeRemaining = 0.40f;
        canShoot = true;
    }

    private void Shoot()
    {
        Instantiate(arrow, spawnArrow.position, spawnArrow.rotation);
    }

    private void Attack()
    {
        isAttacking = true;
    }

    public void AttackTrigger()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(
            AttackPoint.transform.position,
            radius,
            mainTarget
        );

        foreach (Collider2D target in targets)
        {
            if (target.CompareTag("Enemy"))
            {
                target.GetComponent<Health>().TakeDamage(Damage);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackPoint.transform.position, radius);
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;

        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // If the player was previously grounded and now isn't, it means the player is falling
        if (wasGrounded && !grounded)
            animator.SetBool("Falling", true);

        wasGrounded = grounded;

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            horizontal * maxSpeed,
            acceleration * Time.fixedDeltaTime
        );

        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(Vector3.up, 180f);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        int dashDirection = isFacingRight ? 1 : -1;
        rb.velocity = new Vector2(dashDirection * dashingPower, 0f);

        yield return new WaitForSeconds(dashingTime);

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Enemy"))
    //     {
    //         Health playerHealth = GetComponent<Health>();
    //         playerHealth.TakeDamage(2);

    //         Knockback(other);

    //         if (playerHealth.currentHealth <= 0)
    //         {
    //             StartCoroutine(DestroyWithAnimation());
    //         }
    //     }
    // }

    // IEnumerator DestroyWithAnimation()
    // {
    //     animator.SetBool("isDead", true);

    //     yield return new WaitForSeconds(isDeadClip.length);

    //     Destroy(gameObject);
    // }

    // public void Knockback(Collider2D trig)
    // {
    //     Vector3 direction = (transform.position - trig.transform.position).normalized;

    //     if (isFacingRight)
    //     {
    //         transform.Translate(trig.transform.right * 1);
    //     }
    //     else
    //     {
    //         transform.Translate(-trig.transform.right * 1);
    //     }

    //     StartCoroutine(TakenHitWithAnimation());
    // }

    // IEnumerator TakenHitWithAnimation()
    // {
    //     // Play the "isDead" animation
    //     animator.SetBool("wasHit", true);

    //     // Wait for the duration of the "isDead" animation
    //     yield return new WaitForSeconds(wasHit.length);
    //     // Destroy the game object after the animation has played
    //     animator.SetBool("wasHit", false);
    // }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Player was hit!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            isinMelleRange = true;
        }
    }
}
