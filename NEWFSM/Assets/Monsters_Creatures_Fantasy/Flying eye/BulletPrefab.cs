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
            Debug.Log("Player was hit!");

            GameObject.Instantiate(Blast, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
