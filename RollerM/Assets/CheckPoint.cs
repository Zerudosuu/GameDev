using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    PlayerMovement playerMovement;
    Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.UpdateCheckpoint(gameObject.transform.position);
            col.enabled = false;
        }
    }
}
