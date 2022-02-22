using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractRange : MonoBehaviour
{
    public Interactive interactiveObject;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.interactingWith = interactiveObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.interactingWith = null;
        }
    }
}
