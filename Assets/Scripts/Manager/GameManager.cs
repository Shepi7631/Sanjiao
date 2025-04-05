using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

[Serializable]
public enum GameFlow
{
    梦境0 = 0,
    挖矿游戏 = 1,
    梦境1 = 2,
    早餐店 = 3,
    家中 = 4,
    矿洞门口 = 5,
    药店和娱乐场 = 6,
    篮球场 = 7,
    垃圾回收厂 = 8,
    梦境2 = 9,
    矿洞 = 10,
    监狱 = 11
}

[Serializable]
public struct GameFlowPair
{
    public GameFlow flow;
    public Vector3 pos;
}


public class GameManager : SingletonBase<GameManager>
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Image fadePanel;
    [SerializeField] private Image infoPanel;
    public List<GameFlowPair> gameFlowPairs;
    public Dictionary<GameFlow, Vector3> playerBirthPoses = new();

    public float ageTimer;

    private float maxTimer;

    private int curCount;

    private int maxCount;

    public int GoldLimit;

    private GameFlow curFlow;

    private void Awake()
    {
        foreach (GameFlowPair pair in gameFlowPairs)
        {
            playerBirthPoses.Add(pair.flow, pair.pos);
        }

        curFlow = (GameFlow)PlayerPrefs.GetInt("Level");
    }

    private void Start()
    {
        ChangeLevel();
    }

    private void Update()
    {
        if (playerController.gameType == GameType.Dream && curFlow != GameFlow.梦境0)
        {
            ageTimer += Time.deltaTime;
            if (ageTimer >= maxTimer)
            {
                playerController.AgeForward();
                ageTimer = 0;
                curCount++;
                if (curCount >= maxCount) Remake();
            }
            UIManager.Instance.timerText.text = "衰老倒计时：" + ageTimer.ToString("f2") + "/" + maxTimer;
            UIManager.Instance.countText.text = "衰老次数：" + curCount.ToString() + "/" + maxCount;
            UIManager.Instance.goldText.text = "收集品数量" + playerController.Gold + "/" + GoldLimit;
        }
    }

    public void NextLevel(GameFlow flow)
    {
        curFlow = flow;
        ChangeLevel();
        PlayerPrefs.SetInt("Level", (int)curFlow);
        PlayerPrefs.Save();
    }

    public void NextLevel(int flow)
    {
        curFlow = (GameFlow)flow;
        ChangeLevel();

        PlayerPrefs.SetInt("Level", (int)curFlow);
        PlayerPrefs.Save();
    }

    [ContextMenu("DoSomething")]
    public void Remake()
    {
        SceneManager.LoadScene(1);

        ChangeLevel();
    }

    public void SetCurLevel(GameFlow flow)
    {
        curFlow = flow;
        PlayerPrefs.SetInt("Level", (int)curFlow);
        PlayerPrefs.Save();
        Remake();
    }

    private void ChangeLevel()
    {
        Sequence sequence = DOTween.Sequence()
         .Append(fadePanel.DOColor(new Color(0, 0, 0, 1), 0f))
         .AppendCallback(() =>
         {
             playerController.SetPos(playerBirthPoses[curFlow]);
             switch (curFlow)
             {
                 case GameFlow.梦境0:
                     playerController.gameType = GameType.Dream;
                     playerController.ChangeAge(AgeType.Children);
                     UIManager.Instance.timerText.enabled = true;
                     UIManager.Instance.countText.enabled = true;
                     UIManager.Instance.goldText.enabled = true;
                     infoPanel.enabled = false;
                     break;
                 case GameFlow.挖矿游戏:
                     playerController.gameType = GameType.Dig;
                     playerController.ChangeAge(AgeType.Young);
                     MiningGameManager.Instance.InitGame(-75, 75, 8, 16);
                     UIManager.Instance.timerText.enabled = false;
                     UIManager.Instance.countText.enabled = false;
                     UIManager.Instance.goldText.enabled = false;
                     infoPanel.enabled = true;
                     break;
                 case GameFlow.梦境1:
                     playerController.gameType = GameType.Dream;
                     playerController.ChangeAge(AgeType.Young);
                     UIManager.Instance.timerText.enabled = true;
                     UIManager.Instance.countText.enabled = true;
                     UIManager.Instance.goldText.enabled = true;
                     ageTimer = 0;
                     maxTimer = 12;
                     curCount = 0;
                     maxCount = 5;
                     GoldLimit = 3;
                     infoPanel.enabled = false;
                     break;
                 case GameFlow.早餐店:
                     playerController.gameType = GameType.Normal;
                     playerController.ChangeAge(AgeType.Young);
                     UIManager.Instance.timerText.enabled = false;
                     UIManager.Instance.countText.enabled = false;
                     UIManager.Instance.goldText.enabled = false;
                     infoPanel.enabled = false;
                     break;
                 case GameFlow.家中:
                     playerController.gameType = GameType.Normal;
                     playerController.ChangeAge(AgeType.Young);
                     UIManager.Instance.timerText.enabled = false;
                     UIManager.Instance.countText.enabled = false;
                     UIManager.Instance.goldText.enabled = false;
                     infoPanel.enabled = false;
                     break;
                 case GameFlow.矿洞门口:
                     playerController.gameType = GameType.Normal;
                     playerController.ChangeAge(AgeType.Young);
                     UIManager.Instance.timerText.enabled = false;
                     UIManager.Instance.countText.enabled = false;
                     UIManager.Instance.goldText.enabled = false;
                     infoPanel.enabled = false;
                     break;
                 case GameFlow.药店和娱乐场:
                     playerController.gameType = GameType.Normal;
                     playerController.ChangeAge(AgeType.Young);
                     UIManager.Instance.timerText.enabled = false;
                     UIManager.Instance.countText.enabled = false;
                     UIManager.Instance.goldText.enabled = false;
                     infoPanel.enabled = false;
                     break;
                 case GameFlow.篮球场:
                     playerController.gameType = GameType.Normal;
                     playerController.ChangeAge(AgeType.Young);
                     UIManager.Instance.timerText.enabled = false;
                     UIManager.Instance.countText.enabled = false;
                     UIManager.Instance.goldText.enabled = false;
                     infoPanel.enabled = false;
                     break;
                 case GameFlow.垃圾回收厂:
                     playerController.gameType = GameType.Normal;
                     playerController.ChangeAge(AgeType.Young);
                     UIManager.Instance.timerText.enabled = false;
                     UIManager.Instance.countText.enabled = false;
                     UIManager.Instance.goldText.enabled = false;
                     infoPanel.enabled = false;
                     break;
                 case GameFlow.梦境2:
                     playerController.gameType = GameType.Dream;
                     playerController.ChangeAge(AgeType.Young);
                     UIManager.Instance.timerText.enabled = true;
                     UIManager.Instance.countText.enabled = true;
                     UIManager.Instance.goldText.enabled = true;
                     ageTimer = 0;
                     maxTimer = 12;
                     curCount = 0;
                     maxCount = 6;
                     GoldLimit = 8;
                     infoPanel.enabled = false;
                     break;
                 case GameFlow.矿洞:
                     playerController.gameType = GameType.Normal;
                     playerController.ChangeAge(AgeType.Young);
                     UIManager.Instance.timerText.enabled = false;
                     UIManager.Instance.countText.enabled = false;
                     UIManager.Instance.goldText.enabled = false;
                     infoPanel.enabled = false;
                     break;
                 case GameFlow.监狱:
                     playerController.gameType = GameType.Normal;
                     playerController.ChangeAge(AgeType.Young);
                     UIManager.Instance.timerText.enabled = false;
                     UIManager.Instance.countText.enabled = false;
                     UIManager.Instance.goldText.enabled = false;
                     infoPanel.enabled = false;
                     break;
             }
             playerController.Gold = 0;

             playerController.playerInput.Enable();
             if (playerController.gameType == GameType.Normal)
             {
                 DialogManager.Instance.Triggle();
                 playerController.playerInput.Disable();
                 AudioManager.Instance.PlayBGM(AudioType.BGM);
             }
             else if (playerController.gameType == GameType.Dig)
             {
                 AudioManager.Instance.PlayBGM(AudioType.DigBGM);
             }
             else
             {
                 AudioManager.Instance.PlayBGM(AudioType.DreamBGM);
             }
         })
         .AppendInterval(1f)
         .Append(fadePanel.DOColor(new Color(0, 0, 0, 0), 1f));
    }
}

