using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    UiManager uiManager;
    public GameObject Panel;
    public GameObject GameoverPanel;

    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIController").GetComponent<UiManager>();
        GameoverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (uiManager.EnemyCount == 0)
        {
            NextLevel();
        }
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

    public void StartGame()
    {
        Panel.SetActive(false);
    }

    public void EndGame()
    {
        GameoverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(2);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
