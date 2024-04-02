using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterHander : MonoBehaviour
{
    public CharacterData[] Characters;

    [SerializeField]
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public int currentIndex = 0;

    PlayerMovement playerMovement;

    PlayerHealth playerHealth;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject
            .FindGameObjectWithTag("GameController")
            .GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component attached to the character GameObject

        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        UpdateCharacter();
        playerHealth.Start();
        ResetDamageTaken();
    }

    void Update()
    {
        bool allCharactersDead = true;
        foreach (CharacterData character in Characters)
        {
            if (!character.isDead)
            {
                allCharactersDead = false;
                break;
            }
        }

        if (!allCharactersDead)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                do
                {
                    currentIndex = (currentIndex + 1) % Characters.Length;
                } while (Characters[currentIndex].isDead);
                UpdateCharacter();
            }
            else if (Characters[currentIndex].isDead)
            {
                currentIndex = (currentIndex + 1) % Characters.Length;
                while (Characters[currentIndex].isDead)
                {
                    currentIndex = (currentIndex + 1) % Characters.Length;
                }
                UpdateCharacter();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                do
                {
                    currentIndex = (currentIndex - 1 + Characters.Length) % Characters.Length;
                } while (Characters[currentIndex].isDead);
                UpdateCharacter();
            }
            else if (Characters[currentIndex].isDead)
            {
                currentIndex = (currentIndex - 1 + Characters.Length) % Characters.Length;
                while (Characters[currentIndex].isDead)
                {
                    currentIndex = (currentIndex - 1 + Characters.Length) % Characters.Length;
                }
                UpdateCharacter();
            }
        }
        // Check if all characters are dead

        // If all characters are dead, trigger game over
        if (allCharactersDead)
        {
            // Call a method in GameManager to trigger game over
            gameManager.EndGame();
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
            playerMovement.DamageTaken = Characters[currentIndex].DamageTaken;
            playerHealth.currentHealth =
                Characters[currentIndex].health - Characters[currentIndex].DamageTaken;
            playerHealth.UpdateHealth(Characters[currentIndex].health);
            playerMovement.isDead = Characters[currentIndex].isDead;
            // Update other player variables as needed
        }
    }

    public void IncrementDamageTaken(int damage)
    {
        Characters[currentIndex].DamageTaken += damage;
    }

    public void ResetDamageTaken()
    {
        foreach (CharacterData data in Characters)
        {
            data.ResetDamageTaken();
        }
    }

    public void UpdateDamage()
    {
        playerMovement.DamageTaken = Characters[currentIndex].DamageTaken;
    }

    public void CheckifPlayerIsDead(bool dead)
    {
        Characters[currentIndex].isDead = dead;
    }
}
