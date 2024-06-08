using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class Stats : MonoBehaviour
{
    public BaseStats baseStats;
    public float playerCoins; // Track player's coins

    // Define the properties
    public float baseDamage; // Damage per attack
    public float baseMovementSpeed; // Movement speed
    public float baseCooldownSpeed; // Cooldown time for abilities/attacks
    public float baseHealth; // Maximum health points
    public float baseDefense; // Damage reduction

    // Special abilities and skills
    public float abilityPower; // Power of special abilities
    public float abilityCooldown; // Cooldown time for special abilities
    public float abilityDuration; // Duration of the ability effect

    // Resource management
    public float energy; // Energy or mana for abilities
    public float energyRegenRate; // Energy regeneration rate

    // Critical hit properties
    public float criticalChance; // Chance to hit critically
    public float criticalDamage; // Damage multiplier for critical hits

    // Additional combat modifiers
    public float attackSpeed; // Speed of attacks
    public float range; // Attack range
    public float armorPenetration; // Ability to bypass armor

    // Utility properties
    public float dodgeChance; // Chance to dodge attacks
    public float healingReceived; // Modifies how much healing is received
    public float lifeSteal; // Percentage of damage converted to health

    public float Knockback;

    VisualElement CoinContainer;
    Label coinCount;

    void Awake()
    {
        // Assign the values from the BaseStats object
        baseDamage = baseStats.baseDamage;
        baseMovementSpeed = baseStats.baseMovementSpeed;
        baseCooldownSpeed = baseStats.baseCooldownSpeed;
        baseHealth = baseStats.baseHealth;
        baseDefense = baseStats.baseDefense;
        abilityPower = baseStats.abilityPower;
        abilityCooldown = baseStats.abilityCooldown;
        abilityDuration = baseStats.abilityDuration;
        energy = baseStats.energy;
        energyRegenRate = baseStats.energyRegenRate;
        criticalChance = baseStats.criticalChance;
        criticalDamage = baseStats.criticalDamage;
        attackSpeed = baseStats.attackSpeed;
        range = baseStats.range;
        armorPenetration = baseStats.armorPenetration;
        dodgeChance = baseStats.dodgeChance;
        healingReceived = baseStats.healingReceived;
        lifeSteal = baseStats.lifeSteal;
        Knockback = baseStats.Knockback;

        // Optionally print or debug to verify
        Debug.Log("Stats initialized with base stats.");

        var root = FindObjectOfType<UIDocument>().rootVisualElement;

        CoinContainer = root.Q<VisualElement>("CoinContainer");
        coinCount = CoinContainer.Q<Label>("CoinCount");

        coinCount.text = playerCoins.ToString();
    }

    public void UpdateStats(Weapon weaponStats)
    {
        // Update all stats with the weapon stats
        baseDamage += weaponStats.Damage;
        baseDamage += weaponStats.RangedDamage; // Include ranged damage if applicable

        abilityPower += weaponStats.abilityPower;
        abilityCooldown += weaponStats.abilityCooldown;
        abilityDuration += weaponStats.abilityDuration;

        criticalChance += weaponStats.criticalChance;
        criticalDamage += weaponStats.criticalDamage;

        attackSpeed += weaponStats.attackSpeed;
        range += weaponStats.range;
        armorPenetration += weaponStats.armorPenetration;

        dodgeChance += weaponStats.dodgeChance;
        healingReceived += weaponStats.healingReceived;
        lifeSteal += weaponStats.lifeSteal;

        Knockback += weaponStats.Knockback;

        // Optionally print or debug to verify
        Debug.Log("Stats updated with weapon stats.");
    }

    public void BuyStat(string statName, float cost, float amount)
    {
        if (playerCoins >= cost)
        {
            var field = typeof(BaseStats).GetField(statName);
            if (field != null)
            {
                float currentValue = (float)field.GetValue(baseStats);
                field.SetValue(baseStats, currentValue + amount);
                UpdateFieldFromBaseStats(statName);

                // Deduct the coins
                playerCoins -= cost;
                coinCount.text = playerCoins.ToString();
                // Optionally print or debug to verify
                Debug.Log($"Bought {statName} for {cost} coins. Remaining coins: {playerCoins}");
            }
            else
            {
                Debug.LogWarning($"Stat {statName} not found.");
            }
        }
        else
        {
            Debug.LogWarning("Not enough coins to buy this stat.");
        }
    }

    private void UpdateFieldFromBaseStats(string statName)
    {
        var field = typeof(Stats).GetField(statName);
        var baseField = typeof(BaseStats).GetField(statName);

        if (field != null && baseField != null)
        {
            field.SetValue(this, baseField.GetValue(baseStats));
        }
    }

    public void AddCoins(float amount)
    {
        playerCoins += amount;
        coinCount.text = playerCoins.ToString();
    }
}
