using System.Collections.Generic;
using UnityEngine;

public class NearestEnemy : MonoBehaviour
{
    public static NearestEnemy instance;
    public Transform playerTransform; // Reference to the player's transform
    public List<GameObject> enemies; // List of enemy GameObjects

    Rigidbody2D rb;

    MeleeScript meleeScript;

    public bool isAttacking;
    public Animator anim;
    bool isFacingRight = true;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy")); // Populate the list

        meleeScript = GetComponentInChildren<MeleeScript>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Subscribe to the death event of enemies
        foreach (var enemy in enemies)
        {
            var health = enemy.GetComponent<Health>();
            if (health != null)
            {
                health.OnDeath += RemoveDeadEnemy;
            }
        }
    }

    void Update()
    {
        GameObject nearestEnemy = GetNearestEnemy();
        // Now you have the nearest enemy, you can do whatever you need with it
        if (nearestEnemy != null)
        {
            Vector3 directionToEnemy = (
                nearestEnemy.transform.position - transform.position
            ).normalized;

            // Adjust the speed at which the GameObject moves towards the enemy
            float speed = 8f; // Adjust this value as needed

            rb.velocity = directionToEnemy * speed;
            anim.SetFloat("Speed", speed);

            if (directionToEnemy.x < 0 && isFacingRight)
            {
                Flip(); // If enemy is to the left and player is facing right, flip the player
            }
            else if (directionToEnemy.x > 0 && !isFacingRight)
            {
                Flip(); // If enemy is to the right and player is facing left, flip the player
            }
        }

        if (meleeScript.canAttack)
        {
            print("can attack");
            rb.velocity = Vector2.zero;

            Attack();
        }
        else
        {
            isAttacking = false;
        }
    }

    private void Attack()
    {
        isAttacking = true;
    }

    GameObject GetNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemyObject in enemies)
        {
            Transform enemyTransform = enemyObject.transform;
            float distanceToEnemy = Vector3.Distance(
                playerTransform.position,
                enemyTransform.position
            );

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemyObject;
            }
        }

        return nearestEnemy;
    }

    void RemoveDeadEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(Vector3.up, 180f);
    }
}
