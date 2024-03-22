using UnityEngine;

public class MoveNoho : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float speed = 4f;
    private float jump = 8f;
    private float accel = 2f;
    private float accellimit = 20f;
    private bool acceldone;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private LayerMask groundLayer;

    void Start()
    {
        acceldone = false;
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jump, rb.velocity.z);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, rb.velocity.z);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            speed += 5 * Time.deltaTime;
        }
        else
        {
            speed = 4;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(
            horizontal * speed * accel,
            rb.velocity.y,
            vertical * speed * accel
        );
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 0.2f, groundLayer);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.layer == groundLayer)
        {
            IsGrounded();
        }
    }
}
