using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHander : MonoBehaviour
{
    public CharacterData[] Characters;

    [SerializeField]
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public int currentIndex = 0;

    PlayerMovement playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component attached to the character GameObject

        playerMovement = GetComponent<PlayerMovement>();
        UpdateCharacter();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentIndex = (currentIndex + 1) % Characters.Length;
            UpdateCharacter();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            currentIndex = (currentIndex - 1 + Characters.Length) % Characters.Length;
            UpdateCharacter();
        }
    }

    public void UpdateCharacter()
    {
        // Update the animator controller
        animator.runtimeAnimatorController = Characters[currentIndex].animatorController;

        // Update the sprite
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = Characters[currentIndex].sprite;
        }

        // Update player variables based on the current character
        if (playerMovement != null)
        {
            playerMovement.maxSpeed = Characters[currentIndex].maxSpeed;
            playerMovement.sprintSpeed = Characters[currentIndex].sprintSpeed;
            playerMovement.jumpingPower = Characters[currentIndex].JumpingPower;
            playerMovement.raycastDistance = Characters[currentIndex].Range;
            playerMovement.isRange = Characters[currentIndex].isRange;
            playerMovement.dashingPower = Characters[currentIndex].DashingPower;
            playerMovement.Damage = Characters[currentIndex].attackDamage;
            playerMovement.healthpoints = Characters[currentIndex].health;
            // Update other player variables as needed
        }
    }
}
