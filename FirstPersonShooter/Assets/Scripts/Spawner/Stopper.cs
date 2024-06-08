using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AI;

public class Stopper : ActionTask<NavMeshAgent>
{
    // Called when the task is started
    protected override void OnExecute()
    {
        if (agent != null)
        {
            // Stop the NavMeshAgent
            agent.isStopped = true;
            agent.ResetPath();
        }
        EndAction(true);
    }
}
