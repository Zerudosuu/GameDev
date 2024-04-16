using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveToPlayer : MonoBehaviour
{
    private Transform player;
    public float speed = 2f;

    NavMeshAgent agent;
    LineRenderer lineRenderer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // Create a LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.1f;
    }

    void Update()
    {
        agent.SetDestination(player.position);

        // Visualize the path
        DrawPath(agent.path);
    }

    void DrawPath(NavMeshPath path)
    {
        // If the path is invalid or empty, return
        if (path == null || path.corners.Length < 2)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        // Set the number of points in the line renderer to match the path corners
        lineRenderer.positionCount = path.corners.Length;

        // Set each point of the line renderer to the corresponding corner of the path
        for (int i = 0; i < path.corners.Length; i++)
        {
            lineRenderer.SetPosition(i, path.corners[i]);
        }
    }
}
