using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Instruction;
    public GameObject WaveSpawner;
    public GameObject preGameWarning;
    public GameObject GameOverUI;

    public GameObject OptionMenu;

    private bool isOptionMenu = false;

    void Start()
    {
        Instruction.SetActive(true);
        WaveSpawner.SetActive(false);
        preGameWarning.SetActive(false);
        OptionMenu.SetActive(false);

        StartCoroutine(WaitForEnterKey());
    }

    private IEnumerator WaitForEnterKey()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        // Player pressed Enter, disable the Instruction panel
        PressEnterToStart();
    }

    public void PressEnterToStart()
    {
        Instruction.SetActive(false);
        preGameWarning.SetActive(true); // Activate preGameWarning
        Invoke("WarningOff", 2);
        WaveSpawner.SetActive(true);
    }

    void WarningOff()
    {
        preGameWarning.SetActive(false); // Deactivate preGameWarning after 2 seconds
    }

    public void GameOver()
    {
        GameOverUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void tryAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
    }

    public void Mainmenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOptionMenu)
            {
                OptionMenu.SetActive(false);
                isOptionMenu = false;
                Time.timeScale = 1;
            }
            else
            {
                OptionMenu.SetActive(true);
                isOptionMenu = true;
                Time.timeScale = 0;
            }
        }
    }

    public void Resume()
    {
        OptionMenu.SetActive(false);
        isOptionMenu = false;
        Time.timeScale = 1;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
