using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToweRepair : MonoBehaviour
{
    public float repairRate = 5f; // Rate at which the tower is repaired per second
    public KeyCode repairKey = KeyCode.R; // Key to initiate repairs

    private bool isRepairing = false;
    private Health towerHealth;
    public Mana mana;

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
            && mana.currentMana > 0
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
                text.text = "Hold R to Repair";
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
        if (towerHealth != null && mana != null)
        {
            if (mana.currentMana >= 1)
            {
                // Reduce one mana per tower health repaired
                mana.ReduceMana(0.1f);

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
