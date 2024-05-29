using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInputAction playerActionsAsset;
    private InputAction Interact;

    public Transform playerHand;
    private bool canPickUp = false;
    private GameObject weapon;

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

    void OnDisable()
    {
        Interact.performed -= DoInteract;
        playerActionsAsset.Player.Disable();
    }

    private void DoInteract(InputAction.CallbackContext context)
    {
        if (canPickUp)
            PickUp();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WeaponInfo>() != null)
        {
            canPickUp = true;
            weapon = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<WeaponInfo>() != null)
        {
            canPickUp = false;
            weapon = null;
        }
    }

    void PickUp()
    {
        // Set the weapon's parent to the player's hand transform
        weapon.transform.SetParent(playerHand);

        // Optionally reset the weapon's local position and rotation
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        weapon.GetComponent<Collider>().enabled = false;

        PlayerCurrentWeapon playerCurrentWeapon = GetComponent<PlayerCurrentWeapon>();

        playerCurrentWeapon.weapon = weapon.GetComponent<WeaponInfo>().weapon;

        // Optionally, you can add other logic such as equipping the weapon, etc.
    }
}
