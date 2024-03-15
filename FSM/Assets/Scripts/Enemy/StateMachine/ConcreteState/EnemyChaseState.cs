using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Transform _playerTransform;

    private float _MovementSpeed = 1.75f;

    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine)
        : base(enemy, enemyStateMachine)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Chase State");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        Vector2 moveDirection = (_playerTransform.position - enemy.transform.position).normalized;

        enemy.MoveEnemy(moveDirection * _MovementSpeed);

        if (enemy.IsWithStrinikingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }

        if (!enemy.IsAggroed)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
