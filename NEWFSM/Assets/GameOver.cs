using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharacterHander CharHander = other.GetComponent<CharacterHander>();
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            CharHander.IncrementDamageTaken(20);
            playerHealth.TakeDamage(20);
            playerMovement.Die();
        }
    }
}
