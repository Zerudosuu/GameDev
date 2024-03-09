using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour
{
    public float waitTime;
    public GameObject Spawner;

    public GameObject IntroPanel;

    // Start is called before the first frame update
    void Start()
    {
        StartIntro();
        Invoke("EndIntro", waitTime);
    }

    void StartIntro()
    {
        IntroPanel.SetActive(true);
        Spawner.SetActive(false);
    }

    void EndIntro()
    {
        IntroPanel.SetActive(false);
        Spawner.SetActive(true);
    }
};
