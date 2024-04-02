using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    PlayerMovement playerMovement;
    public Slider playerHealthBar;
    public int maxHealth;
    public float currentHealth;

    // Start is called before the first frame update
    public void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        InitializeHealth();
    }

    public void InitializeHealth()
    {
        playerHealthBar.maxValue = maxHealth;
        playerHealthBar.value = maxHealth;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        playerHealthBar.value = currentHealth;
    } // Update is called once per frame

    public void UpdateHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth - playerMovement.DamageTaken;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        playerHealthBar.maxValue = maxHealth;
        playerHealthBar.value = currentHealth;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            CharacterHander characterHander = GetComponent<CharacterHander>();

            characterHander.CheckifPlayerIsDead(true);
        }
    }
}
