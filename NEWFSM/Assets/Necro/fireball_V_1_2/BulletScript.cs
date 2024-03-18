using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
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

        Invoke("Lifeline", 1);
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
}
