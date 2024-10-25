using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDetector : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;
    private NPC curNPC;
    public bool interactable = false;

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "NPC")
        {
            curNPC = collision.GetComponent<NPC>();
            interactable = true;
            UIManager.Instance.conversationText.text = "左键单击与NPC进行交流";

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "NPC" && interactable)
        {
            interactable = false;
            UIManager.Instance.conversationText.text = "";
        }
    }

    public NPC GetNPC()
    {
        return curNPC;
    }


}
