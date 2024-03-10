using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float bounceForce = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Apply your bounce effect or any other logic here

            // For example, you can access the Rigidbody2D of the player and add a force
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Add force to simulate bouncing
                playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}
