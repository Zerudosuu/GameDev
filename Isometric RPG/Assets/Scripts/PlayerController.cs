using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputAction inputActions;
    private InputAction move;

    private Animator animator;

    [SerializeField]
    private Rigidbody _rb;

    [SerializeField]
    private float _speed = 5;

    [SerializeField]
    private float _turnSpeed = 360;

    private Vector3 _input = Vector3.zero;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInputAction();
        animator = GetComponent<Animator>();

        move = inputActions.Player.Move;
        move.performed += ctx => OnMove(ctx.ReadValue<Vector2>());
        move.canceled += ctx => OnMove(Vector2.zero);

        inputActions.Player.Enable();
    }

    private void OnMove(Vector2 movement)
    {
        _input = new Vector3(movement.x, 0, movement.y);
    }

    private void Update()
    {
        float speed = _rb.velocity.magnitude;
        animator.SetFloat("Speed", speed);
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Look()
    {
        if (_input == Vector3.zero)
            return;

        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            rot,
            _turnSpeed * Time.deltaTime
        );
    }

    private void Move()
    {
        Vector3 movement = transform.forward * _input.magnitude * _speed;

        _rb.velocity = movement;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
