using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<bulletScript>() != null)
        {
            Health health = GetComponent<Health>();

            health.TakeDamage(2);
            Destroy(other.gameObject);

            if (health.currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
