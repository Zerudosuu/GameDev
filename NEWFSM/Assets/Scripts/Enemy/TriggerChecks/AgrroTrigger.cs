using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgrroTrigger : MonoBehaviour
{
    public bool isAggroed = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isAggroed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isAggroed = false;
        }
    }
}
