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
            DialogManager npc = GetComponent<DialogManager>();
            if (npc != null)
            {
                //int newDialogTextIndex = 1; // ������¶Ի��ļ�����
                //int newDialogIndex = 0;     // ������¶Ի���ʼλ��
                                
                npc.Triggle(1,0);  // �����Ի�
                Debug.Log("has changed");
            }
        }
    }
}

