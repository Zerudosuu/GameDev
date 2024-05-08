using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputAction inputActions;
    private InputAction move;

    private Rigidbody rb;

    [SerializeField]
    private float movementForce = 1f;

    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    // Default forward direction

    [SerializeField]
    private Camera playerCamera;
    private Animator animator;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        inputActions = new PlayerInputAction();
        animator = this.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // playerActionsAsset.Player.Jump.started += DoJump;
        // playerActionsAsset.Player.Attack.started += DoAttack;
        move = inputActions.Player.Move;
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        // playerActionsAsset.Player.Jump.started -= DoJump;
        // playerActionsAsset.Player.Attack.started -= DoAttack;
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        forceDirection = Vector3.zero;
        forceDirection += move.ReadValue<Vector2>().x * movementForce * Vector3.right;
        forceDirection += move.ReadValue<Vector2>().y * Vector3.forward * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;

        LookAt();
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }

    private Vector3 Forward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 Right(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }
}
