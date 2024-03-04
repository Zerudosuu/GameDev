using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPosition;
    public float timer = 5;

    private GameObject target;
    private bool isFacingRight = true;

    private Animator animator;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target.transform.position.x < transform.position.x && isFacingRight == true)
        {
            Flip();
        }
        else if (target.transform.position.x > transform.position.x && isFacingRight == false)
        {
            Flip();
        }

        if (timer <= 2) // Corrected condition
        {
            animator.SetBool("isAttacking", true);
            StartCoroutine(AttackAnimation());

            timer = 5;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    IEnumerator AttackAnimation()
    {
        // Wait for the duration of the "Break" animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // Destroy the bullet GameObject after the animation has played

        Shoot();
        animator.SetBool("isAttacking", false);
    }

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        isFacingRight = !isFacingRight;
    }

    void Shoot()
    {
        Instantiate(bullet, bulletPosition.position, UnityEngine.Quaternion.identity);
    }
}
