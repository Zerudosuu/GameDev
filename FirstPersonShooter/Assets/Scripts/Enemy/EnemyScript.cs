using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public EnemyType enemyType;
    public EnemyDetails enemyDetails;

    public Transform playerLocation;
    public Transform mainHouse;
    public Transform mainTarget;

    private NavMeshAgent agent;
    private bool isHit;
    private Health health;
    private Animator anim;

    void Start()
    {
        health = GetComponent<Health>();
        agent = GetComponent<NavMeshAgent>();
        mainHouse = GameObject.FindWithTag("Target").transform;
        playerLocation = GameObject.FindWithTag("Player").transform;

        if (enemyType == EnemyType.Bad)
        {
            // Randomly choose between the player and the house
            mainTarget = Random.value > 0.5f ? playerLocation : mainHouse;
        }
        else if (enemyType == EnemyType.Good)
        {
            // Good enemies always target the player
            mainTarget = mainHouse;
        }

        // Set the agent's destination to the selected target
        agent.destination = mainTarget.position;

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!health.isDead)
        {
            agent.destination = mainTarget.position;
        }
        else
        {
            agent.isStopped = true; // Stop the NavMeshAgent when dead
        }

        if (isHit)
        {
            anim.SetTrigger("Onhit");
            agent.isStopped = true; // Stop agent momentarily during hit animation
            isHit = false; // Reset the flag after setting the trigger
        }

        if (health.isDead)
        {
            anim.Play("Death");
            Destroy(gameObject, 1.5f); // Destroy the enemy after 3 seconds
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<House>() != null && enemyType == EnemyType.Good)
        {
            health.Die();
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("yehey nag collide sa player");
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            health.TakeDamage(other.gameObject.GetComponent<bulletScript>().Damage);
            Debug.Log("salvador");
            Destroy(other.gameObject);
            HandleHit(); // Call HandleHit when hit by a bullet
        }
    }

    public void HandleHit()
    {
        isHit = true;
    }
}

[System.Serializable]
public enum EnemyType
{
    Good,
    Bad,
}

[System.Serializable]
public class EnemyDetails
{
    public float Health;
    public float Speed;
}
