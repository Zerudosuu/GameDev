using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;

public class SetAnimationClipTask : ActionTask
{
    public BBParameter<Animator> animator;
    public BBParameter<float> duration;
    public string clipName = "Chasing";

    private float startTime;

    public bool waitActionFinish;

    protected override void OnExecute()
    {
        var anim = animator.GetValue(); // Get the Animator value from the BBParameter
        if (anim == null)
        {
            Debug.LogError(this.name + ": Animator is not assigned!");
            return;
        }
        startTime = Time.time;

        anim.Play(clipName);
    }

    protected override void OnUpdate()
    {
        if (Time.time - startTime >= duration.GetValue())
        {
            animator.GetValue().StopPlayback();
            EndAction(true);
        }
    }
}
