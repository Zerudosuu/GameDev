using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UiManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI EnemyLabel;

    public GameObject[] Enemies;
    public int EnemyCount;

    void Start()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        EnemyCount = Enemies.Length;

        EnemyLabel.text = "Enemies: " + EnemyCount;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemyCount();
    }

    public void UpdateEnemyCount()
    {
        EnemyLabel.text = "Enemies: " + EnemyCount;
    }
}
