using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInputAction playerActionsAsset;
    private InputAction Interact;

    public Transform playerHand;
    private bool canPickUpWeapon = false;
    private bool canInteractWithShop = false;
    private GameObject interactableObject;
    private WeaponInfo currentEquippedWeapon;

    private void Awake()
    {
        playerActionsAsset = new PlayerInputAction();
        Interact = playerActionsAsset.Player.Interact;
    }

    private void OnEnable()
    {
        Interact.performed += DoInteract;
        playerActionsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        Interact.performed -= DoInteract;
        playerActionsAsset.Player.Disable();
    }

    private void DoInteract(InputAction.CallbackContext context)
    {
        if (canPickUpWeapon)
        {
            PickUpWeapon();
        }
        else if (canInteractWithShop)
        {
            OpenShop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        WeaponInfo weaponInfo = other.gameObject.GetComponent<WeaponInfo>();
        Shop shop = other.gameObject.GetComponent<Shop>();

        if (weaponInfo != null)
        {
            canPickUpWeapon = true;
            canInteractWithShop = false; // Ensure shop interaction is disabled when a weapon is present
            interactableObject = other.gameObject;
        }
        else if (shop != null)
        {
            canInteractWithShop = true;
            canPickUpWeapon = false; // Ensure weapon interaction is disabled when a shop is present
            interactableObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        WeaponInfo weaponInfo = other.gameObject.GetComponent<WeaponInfo>();
        Shop shop = other.gameObject.GetComponent<Shop>();

        if (weaponInfo != null && interactableObject == other.gameObject)
        {
            canPickUpWeapon = false;
            interactableObject = null;
        }
        else if (shop != null && interactableObject == other.gameObject)
        {
            canInteractWithShop = false;
            interactableObject = null;
        }
    }

    void PickUpWeapon()
    {
        if (interactableObject == null)
            return;

        WeaponInfo weaponInfo = interactableObject.GetComponent<WeaponInfo>();
        if (weaponInfo.weapon.weaponType == WeaponType.Melee)
        {
            // Check if the weapon names are different
            if (
                currentEquippedWeapon == null
                || weaponInfo.weapon.WeaponName != currentEquippedWeapon.weapon.WeaponName
            )
            {
                // If there's a currently equipped weapon, unparent and enable its collider
                if (currentEquippedWeapon != null)
                {
                    currentEquippedWeapon.transform.SetParent(null);
                    currentEquippedWeapon.GetComponent<Collider>().enabled = true;
                }

                // Equip the new weapon
                interactableObject.transform.SetParent(playerHand);
                interactableObject.transform.localPosition = Vector3.zero;
                interactableObject.transform.localRotation = Quaternion.identity;
                interactableObject.GetComponent<Collider>().enabled = false;

                PlayerCurrentWeapon playerCurrentWeapon = GetComponent<PlayerCurrentWeapon>();
                playerCurrentWeapon.EquipWeapon(weaponInfo.weapon);

                Inventory inventory = GetComponent<Inventory>();
                inventory.PopulateStats();

                // Update the current equipped weapon
                currentEquippedWeapon = weaponInfo;
            }
        }
        else if (weaponInfo.weapon.weaponType == WeaponType.Collectibles)
        {
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            if (weaponInfo.weapon.WeaponName == "Health")
            {
                weaponInfo.weapon.ApplyHealing(playerHealth);
            }
            else
            {
                weaponInfo.weapon.ApplyMana(playerHealth);
            }
            Destroy(interactableObject);
        }
    }

    void OpenShop()
    {
        if (interactableObject == null)
            return;

        Shop shop = interactableObject.GetComponent<Shop>();
        shop.OpenShop();
    }
}
