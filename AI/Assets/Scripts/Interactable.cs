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
    public Transform targetHand;
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
        // If target hand is not assigned, return
        if (targetHand == null)
        {
            Debug.LogWarning("Target hand is not assigned.");
            return;
        }

        // Set the parent of the item to the target hand
        transform.SetParent(targetHand);

        // Reset the local position and rotation of the item
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
