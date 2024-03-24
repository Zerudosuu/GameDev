using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    private bool isIdle;
    private bool isAimig;

    private Rigidbody rigidbody1;

    [SerializeField]
    private float stopVelocity = .05f;

    [SerializeField]
    private float shotPower;

    private void Awake()
    {
        rigidbody1 = GetComponent<Rigidbody>();

        isAimig = false;

        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (rigidbody1.velocity.magnitude < stopVelocity)
            Stop();

        ProcessAim();
    }

    private void Stop()
    {
        rigidbody1.velocity = Vector3.zero;
        rigidbody1.angularVelocity = Vector3.zero;
        isIdle = true;
    }

    private void OnMouseDown()
    {
        if (isIdle)
            isAimig = true;
    }

    private void ProcessAim()
    {
        if (!isAimig || !isIdle)
            return;

        Vector3? worlPoint = CastMouseClickRay();

        if (!worlPoint.HasValue)
            return;

        DrawLine(worlPoint.Value);

        if (Input.GetMouseButtonUp(0))
        {
            Shoot(worlPoint.Value);
        }
    }

    private void Shoot(Vector3 worlPoint)
    {
        isAimig = false;
        lineRenderer.enabled = false;

        Vector3 horizontalWorlpoint = new Vector3(worlPoint.x, transform.position.y, worlPoint.z);

        Vector3 direction = horizontalWorlpoint - transform.position;
        direction.Normalize();

        float strength = Vector3.Distance(transform.position, horizontalWorlpoint);

        rigidbody1.AddForce(direction * strength * shotPower);
    }

    private void DrawLine(Vector3 worlPoint)
    {
        Vector3[] positions = { transform.position, worlPoint };

        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;
    }

    private Vector3? CastMouseClickRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane
        );

        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane
        );

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit hit;

        if (
            Physics.Raycast(
                worldMousePosNear,
                worldMousePosFar - worldMousePosNear,
                out hit,
                float.PositiveInfinity
            )
        )
        {
            return hit.point;
        }
        else
        {
            return null;
        }
    }
}
