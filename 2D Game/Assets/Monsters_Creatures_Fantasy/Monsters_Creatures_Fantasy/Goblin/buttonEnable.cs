using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonEnable : MonoBehaviour
{
    // public TextMeshProUGUI Title;
    // public Button button;
    // public Button button2;
    public GameObject Container;

    private Animator animator;

    void Start()
    {
        // Title.gameObject.SetActive(false);
        // button.gameObject.SetActive(false);
        // button2.gameObject.SetActive(false);

        animator = Container.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("entered", true);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
