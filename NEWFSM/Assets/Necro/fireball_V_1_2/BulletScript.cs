using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Transform player;

    private bool isFacingRight = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }

        Invoke("Lifeline", 5);
    }

    // Update is called once per frame
    void Update() { }

    private void Flip()
    {
        gameObject.transform.Rotate(0f, 180f, 0f);
        isFacingRight = !isFacingRight;
    }

    void Lifeline()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterHander CharHander = other.GetComponent<CharacterHander>();
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            CharHander.IncrementDamageTaken(5);
            CharHander.UpdateDamage();
            playerHealth.TakeDamage(5);

            print("Player was hit by the fireball");
        }
    }
}
