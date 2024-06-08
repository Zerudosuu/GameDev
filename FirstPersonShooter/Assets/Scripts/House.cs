using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public int HouseHealth;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyScript>().enemyType == EnemyType.Good) { }
        else if (other.gameObject.GetComponent<EnemyScript>().enemyType == EnemyType.Bad)
        {
            HouseHealth--;
            Destroy(other.gameObject);
        }
    }
}
