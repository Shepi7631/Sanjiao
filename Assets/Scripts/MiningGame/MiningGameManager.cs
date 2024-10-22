using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MiningGameManager : SingletonBase<MiningGameManager>
{

    private float closeDistance = 0.5f;
    private float middleDistance = 2f;
    private float farDistance = 5f;

    [SerializeField] private GameObject prefab;
    private List<float> mineralPosList = new List<float>();

    private void Awake()
    {
        InitGame(-50, 50, 8, 16);
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    public void InitGame(float min, float max, float minDiff, float maxDiff)
    {
        float curValue = min;
        float gap = Random.Range(minDiff, maxDiff);
        // 反复尝试生成新的浮点数，直到满足条件为止
        while (curValue + gap < max)
        {
            curValue += gap;
            mineralPosList.Add(curValue);
            gap = Random.Range(minDiff, maxDiff);
        }
        foreach (var mineralPos in mineralPosList)
        {
            Instantiate(prefab, new Vector2(mineralPos, 0), Quaternion.identity);
        }
    }

    public bool DigFeedBack(float digPos)
    {
        float distance = GetClosestMineralPos(digPos) - digPos;
        string text = "Error";
        if (distance > farDistance) text = "FarRight";
        else if (distance <= farDistance && distance > middleDistance) text = "MiddleRight";
        else if (distance <= middleDistance && distance > closeDistance) text = "CloseRight";
        else if (distance <= closeDistance && distance > -closeDistance) { text = "Here"; return true; }
        else if (distance <= -closeDistance && distance > -middleDistance) text = "CloseLeft";
        else if (distance <= -middleDistance && distance > -farDistance) text = "MiddleLeft";
        else if (distance <= -farDistance) text = "FarLeft";
        UIManager.Instance.ChangeMiningGameInfo(text);
        return false;
    }

    private float GetClosestMineralPos(float digPos)
    {
        float pos = float.MaxValue;
        foreach (var mineralPos in mineralPosList)
        {
            if (Mathf.Abs(mineralPos - digPos) < Mathf.Abs(pos - digPos)) pos = mineralPos;
        }
        return pos;
    }

}
