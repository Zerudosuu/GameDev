using System.Collections;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

[Category("Movement/Direct")]
[Description("Rotates and moves the agent towards the target per frame while playing an animation")]
public class RotateAndMoveTowards : ActionTask<Transform>
{
    [RequiredField]
    public BBParameter<GameObject> target;
    public BBParameter<float> rotationSpeed = 2;
    public BBParameter<float> moveSpeed = 2;
    public BBParameter<float> stopDistance = 0.1f;

    [SliderField(1, 180)]
    public BBParameter<float> angleDifference = 5;
    public BBParameter<Vector3> upVector = Vector3.up;
    public BBParameter<Animator> animator;
    public string animationClipName;
    public bool waitActionFinish;

    protected override void OnExecute()
    {
        if (animator.value != null)
        {
            animator.value.Play(animationClipName);
        }
    }

    protected override void OnUpdate()
    {
        bool rotationComplete = RotateTowardsTarget();
        bool movementComplete = MoveTowardsTarget();

        if (rotationComplete && movementComplete)
        {
            if (waitActionFinish)
            {
                EndAction();
            }
        }
    }

    private bool RotateTowardsTarget()
    {
        var direction = target.value.transform.position - agent.position;
        if (Vector3.Angle(direction, agent.forward) <= angleDifference.value)
        {
            return true;
        }

        agent.rotation = Quaternion.LookRotation(
            Vector3.RotateTowards(
                agent.forward,
                direction,
                rotationSpeed.value * Time.deltaTime,
                0
            ),
            upVector.value
        );
        return false;
    }

    private bool MoveTowardsTarget()
    {
        if ((agent.position - target.value.transform.position).magnitude <= stopDistance.value)
        {
            return true;
        }

        agent.position = Vector3.MoveTowards(
            agent.position,
            target.value.transform.position,
            moveSpeed.value * Time.deltaTime
        );
        return false;
    }

    protected override void OnStop()
    {
        if (animator.value != null)
        {
            animator.value.StopPlayback();
        }
    }
}
