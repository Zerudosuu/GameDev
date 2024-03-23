using UnityEngine;

public class CarAcceleration : MonoBehaviour
{
    public float acceleration = 5f; // Acceleration rate
    public float maxSpeed = 10f; // Maximum speed
    public float jumpForce = 10f; // Force applied when jumping
    private Rigidbody rb;

    [SerializeField]
    private float currentSpeed; // Variable to track speed

    [SerializeField]
    private Transform cameraTransform;

    private bool IsGrounded()
    {
        RaycastHit hit;
        float distanceToGround = GetComponent<Collider>().bounds.extents.y;

        bool grounded = Physics.Raycast(
            transform.position,
            -Vector3.up,
            out hit,
            distanceToGround + 0.1f
        );

        // Draw the ray in the Scene view
        Color rayColor = grounded ? Color.green : Color.red;
        Debug.DrawRay(transform.position, -Vector3.up * (distanceToGround + 0.1f), rayColor);

        return grounded;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check for jump input
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate movement direction based on camera forward and right vectors
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0; // Ensure no vertical component
        cameraRight.y = 0; // Ensure no vertical component
        Vector3 movement =
            (cameraForward.normalized * moveVertical) + (cameraRight.normalized * moveHorizontal);

        // Apply acceleration only if grounded
        if (IsGrounded() && movement.magnitude > 0)
        {
            rb.AddForce(movement * acceleration);
        }

        // Limit maximum speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // Rotate the object based on horizontal input
        transform.Rotate(Vector3.up, moveHorizontal * Time.deltaTime * 100);

        // Camera follows the character's facing direction
        if (cameraTransform != null)
        {
            cameraTransform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }

        currentSpeed = rb.velocity.magnitude;
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = focus ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
