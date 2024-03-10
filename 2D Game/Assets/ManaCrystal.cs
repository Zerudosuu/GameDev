using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCrystal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Mana mana = other.gameObject.GetComponent<Mana>();
            mana.currentMana += 5;

            mana.UpdateManaBar();

            Destroy(gameObject);
        }
    }
}