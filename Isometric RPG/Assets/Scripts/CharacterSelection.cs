using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject[] characters;

    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void SelectCharacter(int index)
    {
        characters[index].gameObject.SetActive(true);
    }
}
