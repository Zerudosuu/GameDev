using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;

public class SetAnimationClipTask : ActionTask
{
    public BBParameter<Animator> animator;
    public string clipName = "Chasing";

    protected override void OnExecute()
    {
        var anim = animator.GetValue(); // Get the Animator value from the BBParameter
        if (anim == null)
        {
            Debug.LogError(this.name + ": Animator is not assigned!");
            return;
        }

        anim.Play(clipName);
    }
}
