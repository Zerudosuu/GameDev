using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    CanvasIntoManager canvasIntoManager;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        canvasIntoManager = GameObject
            .FindGameObjectWithTag("UIController")
            .GetComponent<CanvasIntoManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canvasIntoManager.gameStarted)
        {
            animator.SetBool("gameStarted", false);
        }
        else if (canvasIntoManager.gameStarted)
        {
            animator.SetBool("gameStarted", true);
        }
    }
}
