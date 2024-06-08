using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;

    public string WeaponName;
    public float Damage; // Base damage for melee or primary damage for ranged

    public float RangedDamage; // Additional ranged damage if applicable

    public float FireRate; // Attacks per second

    public Transform toBePlaced; // Transform where the weapon will be placed

    public float knockback; // Knockback force

    // Additional attributes
    public float abilityPower; // Power of special abilities
    public float abilityCooldown; // Cooldown time for special abilities
    public float abilityDuration; // Duration of the ability effect

    public float criticalChance; // Chance to hit critically
    public float criticalDamage; // Damage multiplier for critical hits

    public float attackSpeed; // Speed of attacks
    public float range; // Attack range
    public float armorPenetration; // Ability to bypass armor

    public float dodgeChance; // Chance to dodge attacks
    public float healingReceived; // Modifies how much healing is received
    public float lifeSteal; // Percentage of damage converted to health

    public float Knockback;

    #region  Healing/Mana
    public float HealingAmount = 10f;
    public float ManaAmount = 10f;
    #endregion



    // Method to apply healing effect
    public void ApplyHealing(PlayerHealth health)
    {
        health.Heal(HealingAmount);
    }

    public void ApplyMana(PlayerHealth playerMana)
    {
        playerMana.ManaUp(ManaAmount);
    }
}

public enum WeaponType
{
    Melee,
    Ranged,
    Equipment,

    Collectibles,
}
