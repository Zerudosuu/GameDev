using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update+
    [SerializeField]
    TextMeshProUGUI scoreText;

    [SerializeField]
    TextMeshProUGUI PullCount;

    GameManager gameManager;

    private GameObject[] coins;
    public int CoinCount;

    void Start()
    {
        gameManager = GameObject
            .FindGameObjectWithTag("GameController")
            .GetComponent<GameManager>();

        coins = GameObject.FindGameObjectsWithTag("Coin");
        CoinCount = coins.Length;

        scoreText.text =
            "Box(Answers) to Collect " + gameManager.score.ToString() + "/" + CoinCount;
    }

    // Update is called once per frame
    public void UpdateScore()
    {
        scoreText.text =
            "Box(Answers) to Collect " + gameManager.score.ToString() + "/" + CoinCount;
    }

    public void CountPull()
    {
        PullCount.text = "Pull Count: " + gameManager.pullCount.ToString();
    }
}
