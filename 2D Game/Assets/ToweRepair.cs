using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToweRepair : MonoBehaviour
{
    public float repairRate = 5f; // Rate at which the tower is repaired per second
    public KeyCode repairKey = KeyCode.E; // Key to initiate repairs

    private bool isRepairing = false;
    private Health towerHealth;
    public Health PlayerHealth;

    public TextMeshProUGUI text;

    void Start()
    {
        towerHealth = GetComponent<Health>();
        text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (
            Input.GetKey(repairKey)
            && towerHealth.currentHealth < towerHealth.maxHealth
            && PlayerHealth.currentHealth > 0
        )
        {
            isRepairing = true;
        }
        else
        {
            isRepairing = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (isRepairing && other.CompareTag("Player")) // Assuming player has the "Player" tag
        {
            if (towerHealth.currentHealth == towerHealth.maxHealth)
            {
                text.text = "Tower health is Max";
            }
            else if (towerHealth.currentHealth <= towerHealth.maxHealth)
            {
                text.text = "Hold E to Repair";
            }

            Debug.Log("REPAIRING...");
            RepairTower();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.gameObject.SetActive(false);
        }
    }

    void RepairTower()
    {
        if (towerHealth != null && PlayerHealth != null)
        {
            if (PlayerHealth.currentHealth >= 1)
            {
                // Deduct health from the player to repair the tower
                PlayerHealth.currentHealth -= repairRate * Time.deltaTime;
                PlayerHealth.currentHealth = Mathf.Max(PlayerHealth.currentHealth, 0f); // Ensure player health doesn't go below zero
                PlayerHealth.healthBar.value = PlayerHealth.currentHealth;

                // Repair the tower
                towerHealth.currentHealth += repairRate * Time.deltaTime;
                towerHealth.currentHealth = Mathf.Min(
                    towerHealth.currentHealth,
                    towerHealth.maxHealth
                );
                towerHealth.healthBar.value = towerHealth.currentHealth;
            }
        }
    }
}
