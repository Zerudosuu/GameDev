using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnDeath;
    public float BaseHealth;
    public float currentHealth;
    public ParticleSystem muzzleFlash;
    private Animator animator;

    public bool isDead = false;
    public bool IsHit;

    void Start()
    {
        currentHealth = BaseHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDeath?.Invoke();
        isDead = true;
    }

    public void HandleHit()
    {
        IsHit = true;
    }
}

[System.Serializable]
public enum enemyType
{
    Bad,
    Good
}
