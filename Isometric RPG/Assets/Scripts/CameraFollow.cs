using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float smoothTime;

    private Vector3 _currentVelocity = Vector3.zero;

    #endregion

    #region Unity callbacks

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
