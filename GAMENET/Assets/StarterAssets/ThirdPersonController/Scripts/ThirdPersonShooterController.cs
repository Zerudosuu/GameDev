using System;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField]
    private Rig aimRig;

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

    private bool isAiming = false;
    private bool isShooting;
    private float aimRigWeight;

    void Awake()
    {
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();

        // Subscribe to aim events
        starterAssetsInputs.onAimStarted += OnAimStarted;
        starterAssetsInputs.onAimStopped += OnAimStopped;
        starterAssetsInputs.onShootStarted += OnShootStarted;
    }

    void Update()
    {
        HandleMouseWorldPosition();
        HandleShooting();
        HandleAiming();

        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);
    }

    private void HandleMouseWorldPosition()
    {
        // Handle mouse world position logic
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, aimColliderLayer))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }
    }

    private void HandleShooting()
    {
        // Handle shooting logic
        if (isAiming && isShooting)
        {
            Vector3 aimDirection = (
                debugTransform.position - spawnBulletPosition.position
            ).normalized;
            Instantiate(
                pfBulletProjectile,
                spawnBulletPosition.position,
                Quaternion.LookRotation(aimDirection, Vector3.up)
            );
            isShooting = false;
        }
    }

    private void HandleAiming()
    {
        // Handle aiming logic
        if (isAiming)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(
                1,
                Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f)
            );

            Vector3 worldAimTarget = debugTransform.position;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            animator.SetLayerWeight(2, 0);
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
            animator.SetLayerWeight(2, 1);
        }
    }

    private void OnShootStarted(object sender, EventArgs e)
    {
        isShooting = true;
    }

    private void OnAimStarted(object sender, EventArgs e)
    {
        isAiming = true;
        aimRigWeight = 1f;
    }

    private void OnAimStopped(object sender, EventArgs e)
    {
        isAiming = false;
        aimRigWeight = 0f;
    }
}
