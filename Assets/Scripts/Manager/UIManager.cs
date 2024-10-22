using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBase<UIManager>
{
    [SerializeField] private TextMeshProUGUI interactInfo;
    [SerializeField] private TextMeshProUGUI miningGameInfo;

    private void Awake()
    {

    }

    public void ChangeInteractInfo(bool state)
    {
        if (state) interactInfo.text = "Yes";
        else interactInfo.text = "No";
    }

    public void ChangeMiningGameInfo(string text)
    {
        miningGameInfo.text = text;
    }

    private void OnEnable()
    {
        EventManager.OnChangeInteractInfoEvent += ChangeInteractInfo;
    }

    private void OnDisable()
    {
        EventManager.OnChangeInteractInfoEvent -= ChangeInteractInfo;
    }
}
