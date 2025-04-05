using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBase<UIManager>
{
    //[SerializeField] private TextMeshProUGUI interactInfo;
    //[SerializeField] private TextMeshProUGUI miningGameInfo;
    [SerializeField] private Text tips;
    [SerializeField] private Text tips2;
    public Text timerText;
    public Text countText;
    public Text goldText;
    public Text conversationText;

    private void Awake()
    {

    }

    public void SetTipsInfo(string text)
    {
        tips.text = text;
    }

    //public void ChangeInteractInfo(bool state)
    //{
    //    if (state) interactInfo.text = "Yes";
    //    else interactInfo.text = "No";
    //}

    //public void ChangeMiningGameInfo(string text)
    //{
    //    miningGameInfo.text = text;
    //}

    private void EnableTip(float a, float b)
    {
        tips2.enabled = true;
    }

    private void DisableTip(bool state)
    {
        tips2.enabled = false;
    }

    private void OnEnable()
    {
        //EventManager.OnChangeInteractInfoEvent += ChangeInteractInfo;
        EventManager.OnMiningGameStartEvent += EnableTip;
        EventManager.OnMiningGameEndEvent += DisableTip;
    }

    private void OnDisable()
    {
        //EventManager.OnChangeInteractInfoEvent -= ChangeInteractInfo;
        EventManager.OnMiningGameStartEvent -= EnableTip;
        EventManager.OnMiningGameEndEvent -= DisableTip;
    }
}
