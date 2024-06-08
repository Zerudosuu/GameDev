using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    Stats stats;

    //input fields
    private PlayerInputAction playerActionsAsset;
    private InputAction move;
    private InputAction attack;

    //movement fields
    private Rigidbody rb;

    [SerializeField]
    private float movementForce = 1f;

    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;
    private Animator animator;

    Skill skill;

    private int currentComboAttack = 1; // Track the current combo attack (1, 2, or 3)`
    private float lastAttackTime = 0f; // Timer for combo window
    private float maxComboDelay = 1f; // Maximum time allowed between attacks for a combo

    public float knockbackForce = 10f;

    private void Awake()
    {
        stats = GetComponent<Stats>();
        rb = this.GetComponent<Rigidbody>();
        playerActionsAsset = new PlayerInputAction();
        animator = gameObject?.GetComponent<Animator>();

        playerCamera = Camera.main;

        move = playerActionsAsset.Player.Move;
        attack = playerActionsAsset.Player.Attack;

        skill = GetComponent<Skill>();
    }

    private void OnEnable()
    {
        // playerActionsAsset.Player.Jump.started += DoJump;
        attack.performed += DoAttack;
        playerActionsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        // playerActionsAsset.Player.Jump.started -= DoJump;
        attack.performed -= DoAttack;
        playerActionsAsset.Player.Disable();
    }

    void Update()
    {
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        forceDirection +=
            move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection +=
            move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

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

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
            return true;
        else
            return false;
    }

    private void DoAttack(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Attack1");
        print("Damage" + stats.baseStats.baseDamage);
    }
}
