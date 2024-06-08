using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform spawnPoint; // Set this to the player's start position in the Inspector
    public Stats playerStats; // Reference to the player's stats script
    public PlayerCurrentWeapon playerCurrentWeapon; // Reference to the player's current weapon script
    public float respawnDelay = 2.0f; // Time delay before respawning

    private void Awake()
    {
        if (playerStats == null)
            playerStats = GetComponent<Stats>();

        if (playerCurrentWeapon == null)
            playerCurrentWeapon = GetComponent<PlayerCurrentWeapon>();
    }

    public void Die()
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Reset player position to spawn point
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        // Reset player stats to base stats
        // playerStats.ResetToBaseStats();

        // Destroy current weapon if any
        if (playerCurrentWeapon.weapon != null)
        {
            WeaponInfo weaponInfo =
                playerCurrentWeapon.weapon.toBePlaced.GetComponent<WeaponInfo>();
            if (weaponInfo != null)
            {
                weaponInfo.transform.SetParent(null); // Unparent the weapon
                Destroy(weaponInfo.gameObject); // Destroy the weapon's GameObject
                playerCurrentWeapon.weapon = null;
            }
        }

        // Reset player health, energy, etc.
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ResetHealth();
        }

        // Optionally print or debug to verify
        Debug.Log("Player respawned and stats reset.");
    }
}
