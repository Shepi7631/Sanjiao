using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItemDetector : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;
    private InteractableItem curItem;
    public bool interactable = false;

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            curItem = collision.GetComponent<InteractableItem>();
            interactable = true;
            EventManager.ChangeInteractInfoEvent(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interactable" && interactable)
        {
            interactable = false;
            EventManager.ChangeInteractInfoEvent(interactable);
        }
    }

    public InteractableItem GetItem()
    {
        return curItem;
    }
}
