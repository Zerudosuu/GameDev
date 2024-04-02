using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterData : ScriptableObject
{
    public Sprite sprite;
    public string characterName;
    public int health;
    public int attackDamage;
    public bool isRange;
    public float sprintSpeed;
    public float maxSpeed;

    public float JumpingPower;

    public float Range;

    public float DashingPower;

    public int DamageTaken;

    public AnimatorController animatorController;

    public void ResetDamageTaken()
    {
        DamageTaken = 0;
        isDead = false;
    }

    public bool isDead;
}
