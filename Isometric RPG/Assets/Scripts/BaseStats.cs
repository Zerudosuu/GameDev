using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseStats
{
    // Basic combat stats
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

    // Constructor to initialize with default values+
    public BaseStats()
    {
        baseDamage = 10.0f;
        baseMovementSpeed = 5.0f;
        baseCooldownSpeed = 1.0f;
        baseHealth = 100.0f;
        baseDefense = 5.0f;
        abilityPower = 20.0f;
        abilityCooldown = 5.0f;
        abilityDuration = 3.0f;
        energy = 50.0f;
        energyRegenRate = 5.0f;
        criticalChance = 0.1f;
        criticalDamage = 1.5f;
        attackSpeed = 1.0f;
        range = 1.0f;
        armorPenetration = 0.0f;
        dodgeChance = 0.05f;
        healingReceived = 1.0f;
        lifeSteal = 0.0f;
        Knockback = 0.0f;
    }

    // Method to print stats (for debugging)
    public void PrintStats()
    {
        Debug.Log($"Base Damage: {baseDamage}");
        Debug.Log($"Movement Speed: {baseMovementSpeed}");
        Debug.Log($"Cooldown Speed: {baseCooldownSpeed}");
        Debug.Log($"Health: {baseHealth}");
        Debug.Log($"Defense: {baseDefense}");
        Debug.Log($"Ability Power: {abilityPower}");
        Debug.Log($"Ability Cooldown: {abilityCooldown}");
        Debug.Log($"Ability Duration: {abilityDuration}");
        Debug.Log($"Energy: {energy}");
        Debug.Log($"Energy Regen Rate: {energyRegenRate}");
        Debug.Log($"Critical Chance: {criticalChance}");
        Debug.Log($"Critical Damage: {criticalDamage}");
        Debug.Log($"Attack Speed: {attackSpeed}");
        Debug.Log($"Range: {range}");
        Debug.Log($"Armor Penetration: {armorPenetration}");
        Debug.Log($"Dodge Chance: {dodgeChance}");
        Debug.Log($"Healing Received: {healingReceived}");
        Debug.Log($"Life Steal: {lifeSteal}");
    }
}
