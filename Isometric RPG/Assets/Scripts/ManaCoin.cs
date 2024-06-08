using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCoin : MonoBehaviour
{
    public CoinMana coinMana;
}

[System.Serializable]
public enum CoinMana
{
    Mana,
    Coin
}
