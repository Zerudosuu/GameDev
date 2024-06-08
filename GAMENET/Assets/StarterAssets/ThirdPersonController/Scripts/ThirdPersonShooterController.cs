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
    private Camera shootingCamera;

    public float Range = 100f;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private Animator animator;

    private bool isAiming = false;
    private bool isShooting;
    private float aimRigWeight;

    #region Hit
    [SerializeField]
    private GameObject hitPrefab;
    #endregion

    void Awake()
    {
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();

        // Subscribe to aim and shoot events
        starterAssetsInputs.onAimStarted += OnAimStarted;
        starterAssetsInputs.onAimStopped += OnAimStopped;
        starterAssetsInputs.onShootStarted += OnShootStarted;
        starterAssetsInputs.onShootStopped += OnShootStopped;
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
        if (isAiming && isShooting)
        {
            RaycastHit hit;
            if (
                Physics.Raycast(
                    shootingCamera.transform.position,
                    shootingCamera.transform.forward,
                    out hit,
                    Range,
                    aimColliderLayer
                )
            )
            {
                Debug.Log("Hit: " + hit.collider.gameObject.name);
                GameObject Impact = Instantiate(
                    hitPrefab,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );

                Destroy(Impact, 2f);
                // Add logic to apply damage or effects to the hit object here
            }
            else
            {
                Debug.Log("Missed");
            }
        }
    }

    private void HandleAiming()
    {
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

    private void OnShootStopped(object sender, EventArgs e)
    {
        isShooting = false;
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
