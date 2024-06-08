using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet prefab to instantiate
    public Transform bulletSpawnPoint; // The position from where the bullet will be spawned
    public LayerMask aimColliderLayerMask; // Layer mask to determine what the bullets can hit
    public float bulletSpeed = 20f; // Speed of the bullet
    public float fireRate = 0.1f; // Time between shots
    public int maxAmmo = 10; // Maximum ammo capacity
    public float reloadTime = 1.5f; // Time it takes to reload
    public AudioClip shootSound; // Sound to play when shooting

    private StarterAssetsInputs _input;
    private Vector3 _gizmoTargetPoint; // To store the target point for gizmo
    private float nextFireTime = 0f;
    private int currentAmmo;
    private bool isReloading = false;
    private AudioSource _audioSource;

    VisualElement contaner;
    Label Ammo;

    void Start()
    {
        var root = GameObject.FindAnyObjectByType<UIDocument>().rootVisualElement;

        contaner = root.Q<VisualElement>("UIContainer");
        Ammo = contaner.Q<Label>("Ammo");

        _input = transform.root.GetComponent<StarterAssetsInputs>();
        currentAmmo = maxAmmo; // Initialize current ammo
        _audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    void Update()
    {
        if (isReloading)
        {
            return; // Don't allow shooting while reloading
        }

        if (_input.reload)
        {
            StartCoroutine(Reload());
            _input.reload = false; // Reset reload input
        }

        if (_input.shoot && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
                currentAmmo--;
                Ammo.text = currentAmmo.ToString();
            }
            else
            {
                StartCoroutine(Reload());
            }
        }
    }

    void Shoot()
    {
        // Play the shooting sound
        if (shootSound != null)
        {
            _audioSource.PlayOneShot(shootSound);
        }

        // Determine the center of the screen
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        // Create a ray from the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        // Determine the direction of the shot
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimColliderLayerMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(1000);
        }

        _gizmoTargetPoint = targetPoint; // Update gizmo target point

        Vector3 direction = (targetPoint - bulletSpawnPoint.position).normalized;

        // Instantiate the bullet and set its direction
        GameObject bullet = Instantiate(
            bulletPrefab,
            bulletSpawnPoint.position,
            Quaternion.LookRotation(direction)
        );

        // Optionally, you can add velocity to the bullet if it has a Rigidbody
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = direction * bulletSpeed; // Adjust the speed as needed
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reloaded.");
    }

    // Draw gizmos to visualize the shot direction
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(bulletSpawnPoint.position, _gizmoTargetPoint);
        Gizmos.DrawSphere(_gizmoTargetPoint, 0.1f); // Draw a small sphere at the target point
    }
}
