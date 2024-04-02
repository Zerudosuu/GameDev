using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPrefab : MonoBehaviour
{
    public GameObject Blast;

    void Start()
    {
        Invoke("DestroyObject", 2f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharacterHander CharHander = other.GetComponent<CharacterHander>();

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            CharHander.IncrementDamageTaken(2);
            playerHealth.TakeDamage(2);

            Destroy(gameObject);
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
