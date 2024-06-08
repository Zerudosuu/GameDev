using System.Collections;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    #region Variables

    public Transform target;

    [SerializeField]
    private float smoothTime;

    private Vector3 _currentVelocity = Vector3.zero;

    #endregion

    #region Unity callbacks

    private void Awake()
    {
        StartCoroutine(FindTargetCoroutine());
    }

    private IEnumerator FindTargetCoroutine()
    {
        while (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;
            Vector3 smoothedPosition = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref _currentVelocity,
                smoothTime
            );
            transform.position = smoothedPosition;
        }
    }

    #endregion
}
