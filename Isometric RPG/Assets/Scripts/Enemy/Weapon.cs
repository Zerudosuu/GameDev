using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;

    public string WeaponName;
    public float Damage;

    public float RangedDamage;

    public float FireRate;

    public Transform toBeplaced;

    public float knockback;
}

public enum WeaponType
{
    Melee,
    Ranged,
    Thrown,
    Grenade,
    Explosive,
}
