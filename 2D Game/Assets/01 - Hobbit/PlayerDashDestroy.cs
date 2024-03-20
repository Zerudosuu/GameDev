using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashDestroy : MonoBehaviour
{
    public float DestroyTime;

    void Start()
    {
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update() { }
}
