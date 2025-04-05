using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InteractableItemDetector : MonoBehaviour
{
    public PlayerController playerController;
    private CircleCollider2D circleCollider2D;
    private InteractableItem curItem;
    public bool interactable = false;

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            curItem = collision.GetComponent<InteractableItem>();
            interactable = true;
            EventManager.ChangeInteractInfoEvent(interactable);
            if(curItem.itemName=="Car" && playerController.curAgeType!=AgeType.Older)
            {
                UIManager.Instance.SetTipsInfo("只有老年状态才能坐车");
            }
            else
            {
                if (curItem.curState == ItemStateType.Interact)
                    UIManager.Instance.SetTipsInfo("单击进行互动");
                else if (curItem.curState == ItemStateType.Close)
                    UIManager.Instance.SetTipsInfo("该道具暂时不能使用");
                else if (curItem.curState == ItemStateType.Used)
                    UIManager.Instance.SetTipsInfo("该道具已被使用过");
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interactable" && interactable)
        {
            interactable = false;
            EventManager.ChangeInteractInfoEvent(interactable);
            UIManager.Instance.SetTipsInfo("");
        }
    }

    public InteractableItem GetItem()
    {
        return curItem;
    }
}
