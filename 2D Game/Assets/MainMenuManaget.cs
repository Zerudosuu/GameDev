using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Find the Canvas GameObject using its tag.
        GameObject canvasObject = GameObject.FindGameObjectWithTag("Canvas");

        // If the Canvas GameObject is found, get the Animator component.
        if (canvasObject != null)
        {
            animator = canvasObject.GetComponentInParent<Animator>();

            // If the Animator component is found, set the trigger to play the animation.
            if (animator != null)
            {
                animator.SetBool("isReloading", true);
            }
            else
            {
                Debug.LogError("Animator component not found!");
            }
        }
        else
        {
            Debug.LogError("Canvas GameObject not found!");
        }
    }

    // Update is called once per frame
    void Update() { }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");

        // Reset the "isReloading" parameter to false after loading the scene.
        if (animator != null)
        {
            animator.SetBool("isReloading", false);
        }
        else
        {
            Debug.LogError("Animator component not found!");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
