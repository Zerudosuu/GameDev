using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Skill : MonoBehaviour
{
    private PlayerInputAction playerActionsAsset;
    InputAction skill;

    public Transform handSlot1; // Reference to the first hand slot
    public Transform handSlot2; // Reference to the second hand slot
    private bool skillAvailable = true;

    public float skillDuration = 5f;

    public bool isSkillActive;

    Animator animator;

    PlayerHealth playerHealth;

    public float skillCost = 30f;

    private void Awake()
    {
        playerActionsAsset = new PlayerInputAction();
        skill = playerActionsAsset.Player.Skill;
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        isSkillActive = false;
    }

    private void OnEnable()
    {
        skill.performed += DoSkill;

        playerActionsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        skill.performed -= DoSkill;

        playerActionsAsset.Player.Disable();
    }

    private void DoSkill(InputAction.CallbackContext context)
    {
        print("yehey");

        if (skillAvailable && playerHealth.CurrentMana >= skillCost)
        {
            StartCoroutine(PerformSkill());
        }
    }

    IEnumerator PerformSkill()
    {
        skillAvailable = false; // Disable the skill temporarily

        // Check if a weapon is present in the first hand slot
        if (handSlot1.childCount > 0)
        {
            isSkillActive = true;
            playerHealth.ManaReduction(skillCost);

            animator.SetBool("Skilling", true);

            // Get the weapon from the first hand slot
            GameObject weapon = handSlot1.GetChild(0).gameObject;

            // Instantiate a copy of the weapon in the second hand slot
            GameObject weaponInstance = Instantiate(weapon, handSlot2);

            // Optionally, you can adjust the position and rotation of the copied weapon to fit the second hand slot

            // Wait for the skill duration
            yield return new WaitForSeconds(skillDuration);

            animator.SetBool("Skilling", false);

            // Destroy the weapon instance in the second hand slot
            Destroy(weaponInstance);

            // Move the original weapon back to the first hand slot
            weapon.transform.SetParent(handSlot1);

            // Reset the position and rotation of the original weapon
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning("No weapon in the first hand slot.");
        }

        // Re-enable the skill after the duration
        skillAvailable = true;
        isSkillActive = false;
    }
}
