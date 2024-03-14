using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void Damage(float damageAmount);

    void Die();

    float Maxhealth { get; set; }
    float Currenthealth { get; set; }
}
