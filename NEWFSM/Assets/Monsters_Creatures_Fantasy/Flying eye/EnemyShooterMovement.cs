using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterMovement : MonoBehaviour
{
    public Transform PA;
    public Transform PB;

    public Rigidbody2D bulletPrefab;

    public Rigidbody2D rb;

    private float _timer;
    private float _exitTimer;

    [SerializeField]
    private float _timeBetweenShots = 2f;

    [SerializeField]
    private float _timeTillExit = 1f;

    [SerializeField]
    private float _distanceToCountExit = 3f;

    [SerializeField]
    public float _bulletSpeed = 2f;

    private Transform playerTransform;

    private float horizontal;

    private bool isFacingRight = true;

    AgrroTrigger agrroTrigger;

    void Start()
    {
        agrroTrigger = GetComponentInChildren<AgrroTrigger>();
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Attack()
    {
        if (playerTransform.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }
        else if (playerTransform.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }

        Vector2 dir = (playerTransform.position - transform.position).normalized;

        Rigidbody2D bullet = GameObject.Instantiate(
            bulletPrefab,
            transform.position,
            Quaternion.identity
        );
        bullet.velocity = dir * _bulletSpeed;

        // if (horizontal > 0 && isFacingRight == false)
        // {
        //     Flip();
        // }
        // else if (horizontal < 0 && isFacingRight == true)
        // {
        //     Flip();
        // }
    }

    private void Flip()
    {
        gameObject.transform.Rotate(0f, 180f, 0f);
        isFacingRight = !isFacingRight;
    }
}
