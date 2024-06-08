using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using StarterAssets;
using UnityEngine;
using UnityEngine.UIElements;

public class Shoot : MonoBehaviour
{
    public GunDetails gunDetails;

    private StarterAssetsInputs _input;
    private Vector3 _gizmoTargetPoint; // To store the target point for gizmo
    private float nextFireTime = 0f;
    private int currentAmmo;
    private bool isReloading = false;
    private AudioSource _audioSource;

    [SerializeField]
    private MMFeedbacks mMFeedbacks;

    public float Range => gunDetails.Range;
    public Camera fpsCam;

    public ParticleSystem flash;
    public GameObject hitEffect;
    public GameObject bloodEffect;

    public AudioSource audioSource;

    VisualElement container;
    Label label;

    void Start()
    {
        _input = transform.root.GetComponent<StarterAssetsInputs>();
        currentAmmo = gunDetails.maxAmmo; // Initialize current ammo
        _audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

        if (gunDetails == null)
        {
            Debug.LogError("No gun details assigned to " + gameObject.name);
        }

        var root = GameObject.FindObjectOfType<UIDocument>().rootVisualElement;

        container = root.Q("AmmoWeapon");
        label = container.Q<Label>("Ammo");

        label.text = currentAmmo.ToString() + "/" + gunDetails.maxAmmo;
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
                ShootProjectile();
                nextFireTime = Time.time + gunDetails.fireRate;
                currentAmmo--;
                label.text = currentAmmo.ToString() + "/" + gunDetails.maxAmmo;
                // Ammo.text = currentAmmo.ToString();
            }
            else
            {
                StartCoroutine(Reload());
            }
        }
    }

    void ShootProjectile()
    {
        mMFeedbacks.PlayFeedbacks();
        _audioSource.PlayOneShot(gunDetails.shootSound);
        flash.Play();
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, Range))
        {
            var health = hit.transform.GetComponent<Health>();
            if (health != null)
            {
                health.HandleHit();
                health.TakeDamage(gunDetails.Damage);
                GameObject blood = Instantiate(
                    bloodEffect,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );

                Destroy(blood, 1f);
            }
            else
            {
                GameObject Impact = Instantiate(
                    hitEffect,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );

                Destroy(Impact, 2f);
            }

            if (hit.rigidbody != null)
                hit.rigidbody.AddForce(-hit.normal * 60f);
        }
    }

    // void ShootProjectile()
    // {
    //     // Play the shooting sound
    //     if (gunDetails.shootSound != null)
    //     {
    //         _audioSource.PlayOneShot(gunDetails.shootSound);
    //     }

    //     // Determine the center of the screen
    //     Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

    //     // Create a ray from the center of the screen
    //     Ray ray = Camera.main.ScreenPointToRay(screenCenter);

    //     // Determine the direction of the shot
    //     Vector3 targetPoint;
    //     if (
    //         Physics.Raycast(
    //             ray,
    //             out RaycastHit hit,
    //             Mathf.Infinity,
    //             gunDetails.aimColliderLayerMask
    //         )
    //     )
    //     {
    //         targetPoint = hit.point;
    //     }
    //     else
    //     {
    //         targetPoint = ray.GetPoint(1000);
    //     }

    //     _gizmoTargetPoint = targetPoint; // Update gizmo target point

    //     Vector3 direction = (targetPoint - gunDetails.bulletSpawnPoint.position).normalized;

    //     // Instantiate the bullet and set its direction
    //     GameObject bullet = Instantiate(
    //         gunDetails.bulletPrefab,
    //         gunDetails.bulletSpawnPoint.position,
    //         Quaternion.LookRotation(direction)
    //     );

    //     bullet.GetComponent<bulletScript>().Damage = gunDetails.Damage;

    //     // Optionally, you can add velocity to the bullet if it has a Rigidbody
    //     Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
    //     if (bulletRb != null)
    //     {
    //         bulletRb.velocity = direction * gunDetails.bulletSpeed; // Adjust the speed as needed
    //     }
    // }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(gunDetails.reloadTime);

        currentAmmo = gunDetails.maxAmmo;
        isReloading = false;
        Debug.Log("Reloaded.");
        label.text = currentAmmo.ToString() + "/" + gunDetails.maxAmmo;
    }
}
