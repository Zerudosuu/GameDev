using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    public float Damage = 10;

    void OnCollisionEnter(Collision other)
    {
        print(other.gameObject);
    }
}
