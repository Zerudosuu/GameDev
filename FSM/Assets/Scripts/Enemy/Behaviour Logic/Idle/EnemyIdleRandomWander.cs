using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Idle-Random Wander",
    menuName = "Enemy Logic/Idle Logic/Random Wander"
)]
public class EnemyIdleRandomWander : EnemyIdleSObBase
{
    [SerializeField]
    public float RandomMovementRange = 5f;

    [SerializeField]
    public float RandomMovementSpeed = 1f;

    private Vector3 _targetPos;
    private Vector3 _direction;

    [SerializeField]
    private Transform PA;

    [SerializeField]
    private Transform PB;

    [SerializeField]
    private bool _waiting;

    [SerializeField]
    private float _waitTimer;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        PA = enemy.PointA;
        PB = enemy.PointB;
        _targetPos = PA.position;
        _waiting = false;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (enemy.IsAggroed)
        {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
            return;
        }

        if (_waiting)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                _waiting = false;
                _targetPos = GetRandomPointBetween(PA.position, PB.position);
            }
        }
        else
        {
            _direction = (_targetPos - enemy.transform.position).normalized;
            enemy.MoveEnemy(_direction * RandomMovementSpeed);

            if ((enemy.transform.position - _targetPos).sqrMagnitude < 0.01f)
            {
                _waiting = true;
                _waitTimer = 3f; // Wait for 3 seconds at the current target position
            }
        }
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }

    private Vector3 GetRandomPointBetween(Vector3 pointA, Vector3 pointB)
    {
        float randomX = Random.Range(pointA.x, pointB.x);
        float randomY = Random.Range(pointA.y, pointB.y);
        return new Vector3(randomX, randomY, pointA.z); // Keeping the Z coordinate the same as point A
    }
}
