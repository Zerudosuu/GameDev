using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float Damage;

    void Start()
    {
        if (gameObject != null)
        {
            Destroy(gameObject, 4f);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);

        if (other.gameObject.CompareTag("Enemy"))
        {
            print("ROnaldo");
        }
    }
}
