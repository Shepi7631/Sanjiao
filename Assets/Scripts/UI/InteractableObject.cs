using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool isPlayerInside = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("inside");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("offside");
        }
    }

    void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            NPC npc = GetComponent<NPC>();
            if (npc != null)
            {
                //int newDialogTextIndex = 1; // 假设的新对话文件索引
                //int newDialogIndex = 0;     // 假设的新对话起始位置
                                
                npc.triggle(1,0);  // 触发对话
                Debug.Log("has changed");
            }
        }
    }
}

