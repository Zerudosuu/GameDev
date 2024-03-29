using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum InteractableType
{
    Enemy,
    Item
}

public class Interactable : MonoBehaviour
{
    public Actor myactor { get; private set; }
    public InteractableType interactableType;

    void Awake()
    {
        if (interactableType == InteractableType.Enemy)
        {
            myactor = GetComponent<Actor>();
        }
    }

    public void InteractWithItem()
    {
        Destroy(gameObject);
    }
}
