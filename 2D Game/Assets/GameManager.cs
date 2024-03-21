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

    private Animator animator;

    private bool isOptionMenu = false;

    private Mana mana;

    void Start()
    {
        Instruction.SetActive(true);
        WaveSpawner.SetActive(false);
        preGameWarning.SetActive(false);
        OptionMenu.SetActive(false);

        animator = OptionMenu.GetComponent<Animator>();
        StartCoroutine(WaitForEnterKey());
        Cursor.visible = false;
        mana = GameObject.FindGameObjectWithTag("Player").GetComponent<Mana>();
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
        mana.currentMana = mana.MaxMana;
        mana.UpdateManaBar();
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
        Cursor.visible = true;
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
                Cursor.visible = false;
                OptionMenu.SetActive(false);
                isOptionMenu = false;
                Time.timeScale = 1;
            }
            else
            {
                Cursor.visible = true;
                OptionMenu.SetActive(true);
                animator.Play("PauseAnimation");
                StartCoroutine(PauseAfterAnimation());
                isOptionMenu = true;
                Time.timeScale = 0;
            }
        }
    }

    private IEnumerator PauseAfterAnimation()
    {
        // Wait for the duration of the animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Pause the game
        Time.timeScale = 0;
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
