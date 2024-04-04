using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score { get; private set; }

    public int pullCount { get; private set; }

    private void Start() { }

    public void IncrementScore()
    {
        score += 1;
    }

    public void Pull()
    {
        pullCount++;
    }

    public void NextLevel()
    {
        // Get the index of the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calculate the index of the next scene
        int nextSceneIndex = currentSceneIndex + 1;

        // If the next scene index exceeds the total number of scenes, wrap around to the first scene
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        // Load the next scene
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
