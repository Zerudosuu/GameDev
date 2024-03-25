using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    public UnityEvent coinCollect;

    private void Start()
    {
        coinCollect.AddListener(
            GameObject
                .FindGameObjectWithTag("GameController")
                .GetComponent<GameManager>()
                .IncrementScore
        );

        coinCollect.AddListener(
            GameObject
                .FindGameObjectWithTag("UIController")
                .GetComponent<UIController>()
                .UpdateScore
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            coinCollect.Invoke();
            Destroy(gameObject);
        }
    }
}
