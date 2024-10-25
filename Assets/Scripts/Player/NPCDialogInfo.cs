using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[Serializable]
public struct ConversationInfo
{
    public int textIndex;
    public int startPos;
}
[CreateAssetMenu(menuName = "NPCInfo")]

public class NPCDialogInfo : ScriptableObject
{
    public Sprite sprite;

    public string npcName;

    [SerializeField]
    public List<ConversationInfo> InfoList;
}
