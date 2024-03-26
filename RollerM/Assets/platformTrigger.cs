using System.Collections;
using UnityEngine;

public class platformTrigger : MonoBehaviour
{
    public Transform startPosition; // Initial position of the platform
    public Transform endPosition; // Final position of the platform
    public float speed = 2.0f; // Speed at which the platform moves
    public float delay = 1.0f; // Delay before the platform starts moving again at the end

    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        targetPosition = startPosition.position;
    }

    void Update()
    {
        if (!isMoving)
        {
            StartCoroutine(CheckForPlayer());
        }
    }

    IEnumerator CheckForPlayer()
    {
        yield return new WaitForFixedUpdate(); // Ensure physics updates before checking for player presence

        Collider[] colliders = Physics.OverlapBox(
            transform.position,
            transform.localScale / 2f,
            transform.rotation
        );
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                StartCoroutine(MovePlatform());
                yield break;
            }
        }
    }

    IEnumerator MovePlatform()
    {
        isMoving = true;

        targetPosition = endPosition.position;

        while (transform.position != targetPosition)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            yield return null;
        }

        yield return new WaitForSeconds(delay);

        targetPosition = startPosition.position;

        while (transform.position != targetPosition)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            yield return null;
        }

        isMoving = false;
    }
}
