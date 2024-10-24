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
    public Text timerText;
    public Text countText;

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

    private void OnEnable()
    {
        //EventManager.OnChangeInteractInfoEvent += ChangeInteractInfo;
    }

    private void OnDisable()
    {
        //EventManager.OnChangeInteractInfoEvent -= ChangeInteractInfo;
    }
}
