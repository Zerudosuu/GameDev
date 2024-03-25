using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckNextLevel : MonoBehaviour
{
    GameManager gameManager;
    UIController uIController;

    void Start()
    {
        gameManager = GameObject
            .FindGameObjectWithTag("GameController")
            .GetComponent<GameManager>();

        uIController = GameObject
            .FindGameObjectWithTag("UIController")
            .GetComponent<UIController>();
    }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager.score == uIController.CoinCount)
            {
                gameManager.NextLevel();
            }
            else
            {
                print("Need to Collect all the Keys");
            }
        }
    }
}
