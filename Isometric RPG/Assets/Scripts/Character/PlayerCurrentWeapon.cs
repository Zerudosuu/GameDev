using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrentWeapon : MonoBehaviour
{
    public Weapon weapon;
    Stats playerStats;

    void Start()
    {
        playerStats = GetComponent<Stats>();
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
        UpdateStats();
    }

    private void UpdateStats()
    {
        if (playerStats != null && weapon != null)
        {
            playerStats.UpdateStats(weapon);
        }
    }
}
