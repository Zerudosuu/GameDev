using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    public float raycastDistance = 5f;
    public LayerMask layerMask;

    void Update()
    {
        // Cast a ray downwards from the raycast origin
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            raycastDistance,
            layerMask
        );

        // Draw the ray in the scene view for visualization purposes
        Debug.DrawRay(transform.position, Vector2.right * raycastDistance, Color.green);

        // Check if the ray hit something
        if (hit.collider != null)
        {
            // Handle the collision
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
        }
    }
}
