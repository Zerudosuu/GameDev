using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera aimVirtualCamera;

    [SerializeField]
    private float normalSensitivity;

    [SerializeField]
    private float aimSensitivity;

    [SerializeField]
    private LayerMask aimColliderLayer = new LayerMask();

    [SerializeField]
    private Transform debugTransform;

    [SerializeField]
    private Transform pfBulletProjectile;

    [SerializeField]
    private Transform spawnBulletPosition;

    StarterAssetsInputs starterAssetsInputs;
    ThirdPersonController thirdPersonController;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayer))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(
                1,
                Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f)
            );

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(
                1,
                Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f)
            );
        }

        if (starterAssetsInputs.shoot)
        {
            Vector3 aimDirection = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(
                pfBulletProjectile,
                spawnBulletPosition.position,
                Quaternion.LookRotation(aimDirection, Vector3.up)
            );
            starterAssetsInputs.shoot = false;
        }
    }
}
