using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogText : MonoBehaviour
{
    public List<TextAsset> dialogFiles; // 存储所有对话文件的列表

    // 根据索引获取对话文件
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
