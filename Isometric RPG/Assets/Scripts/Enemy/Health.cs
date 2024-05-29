using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float BaseHealth;
    public float CurrentHealth;

    public Animator animator;
    public float DeadDuration;

    public bool isDead;

    void Start()
    {
        CurrentHealth = BaseHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            isDead = true;
        }
    }
}
