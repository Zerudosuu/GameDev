using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject floatingText;
    public GameObject CriticalText;
    public GameObject manaOrbPrefab; // Prefab for mana orb
    public GameObject coinPrefab; // Prefab for coin

    public float BaseHealth;
    public float CurrentHealth;

    public float BaseArmor; // New attribute for base armor
    public float CurrentArmor; // Current armor

    public Animator animator;
    public float DeadDuration;

    public bool isDead;

    public float manaOrbDropRate = 0.3f; // Drop rate for mana orbs (30%)
    public float coinDropRate = .3f; // Drop rate for coins (20%)

    void Start()
    {
        CurrentHealth = BaseHealth;
        CurrentArmor = BaseArmor; // Initialize CurrentArmor
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage, bool isCritical)
    {
        if (floatingText && !isCritical)
            ShowFloatingText(damage);
        else
            ShowCriticalText(damage);

        // Calculate damage after armor reduction
        float remainingDamage = damage;

        // Reduce armor first
        if (CurrentArmor > 0)
        {
            if (CurrentArmor >= damage)
            {
                CurrentArmor -= damage;
                remainingDamage = 0;
            }
            else
            {
                remainingDamage -= CurrentArmor;
                CurrentArmor = 0;
            }
        }

        // Apply remaining damage to health
        CurrentHealth -= remainingDamage;
        if (CurrentHealth <= 0)
        {
            isDead = true;
            DropItems();
        }
    }

    private void ShowCriticalText(float damage)
    {
        GameObject floatingTextInstance = Instantiate(
            CriticalText,
            transform.position,
            Quaternion.identity
        );
        floatingTextInstance.transform.localRotation = Quaternion.LookRotation(
            Camera.main.transform.forward
        );
        floatingTextInstance.GetComponent<TextMesh>().text = damage.ToString();
    }

    private void ShowFloatingText(float damage)
    {
        GameObject floatingTextInstance = Instantiate(
            floatingText,
            transform.position,
            Quaternion.identity
        );
        floatingTextInstance.transform.localRotation = Quaternion.LookRotation(
            Camera.main.transform.forward
        );
        floatingTextInstance.GetComponent<TextMesh>().text = damage.ToString();
    }

    private void DropItems()
    {
        float randomValue = Random.value; // Random value between 0 and 1

        if (randomValue < manaOrbDropRate)
        {
            Instantiate(manaOrbPrefab, transform.position, Quaternion.identity);
        }
        else if (randomValue < manaOrbDropRate + coinDropRate)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }
}
