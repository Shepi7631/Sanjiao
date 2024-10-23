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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            curItem = collision.GetComponent<InteractableItem>();
            interactable = true;
            EventManager.ChangeInteractInfoEvent(interactable);
            if (curItem.curState == ItemStateType.Interact)
                UIManager.Instance.SetTipsInfo("�������л���");
            else if (curItem.curState == ItemStateType.Close)
                UIManager.Instance.SetTipsInfo("�õ�����ʱ����ʹ��");
            else if (curItem.curState == ItemStateType.Used)
                UIManager.Instance.SetTipsInfo("�õ����ѱ�ʹ�ù�");
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
