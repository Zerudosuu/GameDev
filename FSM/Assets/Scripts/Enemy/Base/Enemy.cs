using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    [field: SerializeField]
    public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public Rigidbody2D RB { get; set; }
    public bool isFacingRight { get; set; } = true;

    public bool IsAggroed { get; set; }
    public bool IsWithStrinikingDistance { get; set; }

    #region Idle Variables

    public Rigidbody2D BulletPrefab;
    public float RandomMovementRange = 5f;
    public float RandomMovementSpeed = 1f;

    #endregion

    #region State Machine Variables


    public EnemyStateMachine StateMachine { get; set; }

    public EnemyIdleState IdleState { get; set; }

    public EnemyAttackState AttackState { get; set; }

    public EnemyChaseState ChaseState { get; set; }

    #endregion

    void Awake()
    {
        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }

    private void Start()
    {
        CurrentHealth -= MaxHealth;
        RB = GetComponent<Rigidbody2D>();
        StateMachine.Initialize(IdleState);
    }

    void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    #region  Health/Die Functions

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    #endregion
    #region  Movement Functions
    public void MoveEnemy(Vector2 velocity)
    {
        RB.velocity = velocity;
        CheckForLeftorRightFacing(velocity);
    }

    public void CheckForLeftorRightFacing(Vector2 velocity)
    {
        if (isFacingRight && velocity.x < 0f)
        {
            Vector3 rotator = new Vector3(transform.position.x, 180f, transform.position.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
        else if (!isFacingRight && velocity.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.position.x, 0f, transform.position.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }

    #endregion

    #region Animaton Triggers

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        EnemyDamage,
        PlayFootStepSounds
    }

    #endregion

    #region Distance Check
    public void SetAggroStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;
    }

    public void SetStrikingDistanceBool(bool isWithStrinikingDistance)
    {
        IsWithStrinikingDistance = isWithStrinikingDistance;
    }

    #endregion
}
