using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Instruction;
    public GameObject WaveSpawner;
    public GameObject preGameWarning;
    public GameObject GameOverUI;

    void Start()
    {
        Instruction.SetActive(true);
        WaveSpawner.SetActive(false);
        preGameWarning.SetActive(false);

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
