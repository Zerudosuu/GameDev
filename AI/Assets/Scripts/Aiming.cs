using UnityEngine;

namespace BarthaSzabolcs.IsometricAiming
{
    public class IsometricAiming : MonoBehaviour
    {
        #region Datamembers

        #region Editor Settings

        [SerializeField]
        private LayerMask groundMask;

        [SerializeField]
        private GameObject bulletPrefab;

        [SerializeField]
        private Transform bulletSpawnPoint;

        [SerializeField]
        private float bulletSpeed = 10;

        #endregion
        #region Private Fields

        private Camera mainCamera;
        private bool isAiming;

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        private void Start()
        {
            // Cache the camera, Camera.main is an expensive operation.
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isAiming = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isAiming = false;
                Shoot();
            }

            if (isAiming)
            {
                Aim();
            }
        }

        #endregion

        private void Aim()
        {
            var (success, position) = GetMousePosition();
            if (success)
            {
                // Calculate the direction
                var direction = position - transform.position;

                // Ignore the height difference.
                direction.y = 0;

                // Make the transform look in the direction.
                transform.forward = direction;

                // Draw the raycast
                Debug.DrawRay(transform.position, direction, Color.green);
            }
        }

        private void Shoot()
        {
            var (success, position) = GetMousePosition();
            if (success)
            {
                // Instantiate the bullet prefab at the bulletSpawnPoint position and rotation
                GameObject bullet = Instantiate(
                    bulletPrefab,
                    bulletSpawnPoint.position,
                    bulletSpawnPoint.rotation
                );

                // Calculate the direction towards the mouse position
                var direction = position - transform.position;

                // Set the velocity of the bullet in the direction towards the mouse position
                bullet.GetComponent<Rigidbody>().velocity = direction.normalized * bulletSpeed;
            }
        }

        private (bool success, Vector3 position) GetMousePosition()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
            {
                // The Raycast hit something, return with the position.
                return (success: true, position: hitInfo.point);
            }
            else
            {
                // The Raycast did not hit anything.
                return (success: false, position: Vector3.zero);
            }
        }

        #endregion
    }
}
