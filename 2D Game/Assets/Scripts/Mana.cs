using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    public Slider ManaBar;
    public int MaxMana;
    public int currentMana;

    // Start is called before the first frame update
    void Start()
    {
        ManaBar.maxValue = MaxMana;
        ManaBar.value = MaxMana;
        currentMana = MaxMana;
    }

    public void ReduceMana(int reduceMana)
    {
        currentMana -= reduceMana;
        currentMana = Mathf.Clamp(currentMana, 0, MaxMana); // Ensure mana doesn't go below 0 or exceed MaxMana
        UpdateManaBar();
    } // Update is called once per frame

    public void UpdateManaBar()
    {
        ManaBar.value = currentMana;
    }
}
