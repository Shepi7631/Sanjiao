using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningGameManager : SingletonBase<MiningGameManager>
{
    [SerializeField] private InfoPanel infoPanel;
    [SerializeField] private Transform miningGamePos;
    private float closeDistance = 0.5f;
    private float middleDistance = 2f;
    private float farDistance = 5f;

    [SerializeField] private GameObject prefab;
    private List<float> mineralPosList = new List<float>();

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
        // �������������µĸ�������ֱ����������Ϊֹ
        while (curValue + gap < max)
        {
            curValue += gap;
            mineralPosList.Add(curValue + miningGamePos.position.x);
            gap = Random.Range(minDiff, maxDiff);
        }
        foreach (var mineralPos in mineralPosList)
        {
            Instantiate(prefab, new Vector2(mineralPos, miningGamePos.position.y), Quaternion.identity);
        }
    }

    public bool DigFeedBack(float digPos)
    {
        float distance = GetClosestMineralPos(digPos) - digPos;
        string text = "Error";
        if (distance > farDistance) text = "�ұ�Զ����";
        else if (distance <= farDistance && distance > middleDistance) text = "�ұ��о���";
        else if (distance <= middleDistance && distance > closeDistance) text = "�ұ߽�����";
        else if (distance <= closeDistance && distance > -closeDistance) return true; 
        else if (distance <= -closeDistance && distance > -middleDistance) text = "��߽�����";
        else if (distance <= -middleDistance && distance > -farDistance) text = "����о���";
        else if (distance <= -farDistance) text = "���Զ����";
        infoPanel.ShowInfo(text, 1);
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
