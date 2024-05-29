using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectInteract : MonoBehaviour
{
    PlayerInputAction inputActions;
    InputAction interact;

    void Start()
    {
        inputActions = new PlayerInputAction();
        interact = inputActions.Player.Interact;
    }

    void OnEnable() { }

    void OnDisable() { }

    void Update() { }

    void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
    }
}
