using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogText : MonoBehaviour
{
    public List<TextAsset> dialogFiles; // �洢���жԻ��ļ����б�

    // ����������ȡ�Ի��ļ�
    public TextAsset GetDialogFile(int index)
    {
        if (index >= 0 && index < dialogFiles.Count)
        {
            return dialogFiles[index];
        }
        else
        {
            Debug.LogError("Invalid dialog file index: " + index);
            return null;
        }
    }
}
