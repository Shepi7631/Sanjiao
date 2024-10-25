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
    �ξ�0 = 0,
    �ڿ���Ϸ = 1,
    �ξ�1 = 2,
    ��͵� = 3,
    ���� = 4,
    ���ſ� = 5,
    ҩ������ֳ� = 6,
    ���� = 7,
    �������ճ� = 8,
    �ξ�2 = 9,
    �� = 10,
    ���� = 11
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
    private float miningGameTimer = 0;
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
        if (playerController.gameType == GameType.Dream && curFlow != GameFlow.�ξ�0)
        {
            ageTimer += Time.deltaTime;
            if (ageTimer >= maxTimer)
            {
                playerController.AgeForward();
                ageTimer = 0;
                curCount++;
                if (curCount >= maxCount) Remake();
            }
            UIManager.Instance.timerText.text = "˥�ϵ���ʱ��" + ageTimer.ToString("f2") + "/" + maxTimer;
            UIManager.Instance.countText.text = "˥�ϴ�����" + curCount.ToString() + "/" + maxCount;
            UIManager.Instance.goldText.text = "�ռ�Ʒ����" + playerController.Gold + "/" + GoldLimit;
        }
        else if (playerController.gameType == GameType.Dig)
        {
            miningGameTimer += Time.deltaTime;
            if (miningGameTimer >= 60f)
            {
                miningGameTimer = 0;
                NextLevel(GameFlow.����);
            }
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
        DOTween.Sequence()
        .Append(fadePanel.DOColor(new Color(0, 0, 0, 1), 0f))
        .AppendCallback(() =>
        {
            playerController.SetPos(playerBirthPoses[curFlow]);
            playerController.playerInput.Disable();
            switch (curFlow)
            {
                case GameFlow.�ξ�0:
                    playerController.gameType = GameType.Dream;
                    playerController.ChangeAge(AgeType.Children);
                    UIManager.Instance.timerText.enabled = true;
                    UIManager.Instance.countText.enabled = true;
                    UIManager.Instance.goldText.enabled = true;
                    infoPanel.enabled = false;
                    break;
                case GameFlow.�ڿ���Ϸ:
                    playerController.gameType = GameType.Dig;
                    playerController.ChangeAge(AgeType.Young);
                    MiningGameManager.Instance.InitGame(-75, 75, 8, 16);
                    UIManager.Instance.timerText.enabled = false;
                    UIManager.Instance.countText.enabled = false;
                    UIManager.Instance.goldText.enabled = false;
                    infoPanel.enabled = true;
                    break;
                case GameFlow.�ξ�1:
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
                case GameFlow.��͵�:
                    playerController.gameType = GameType.Normal;
                    playerController.ChangeAge(AgeType.Young);
                    UIManager.Instance.timerText.enabled = false;
                    UIManager.Instance.countText.enabled = false;
                    UIManager.Instance.goldText.enabled = false;
                    infoPanel.enabled = false;
                    break;
                case GameFlow.����:
                    playerController.gameType = GameType.Normal;
                    playerController.ChangeAge(AgeType.Young);
                    UIManager.Instance.timerText.enabled = false;
                    UIManager.Instance.countText.enabled = false;
                    UIManager.Instance.goldText.enabled = false;
                    infoPanel.enabled = false;
                    break;
                case GameFlow.���ſ�:
                    playerController.gameType = GameType.Normal;
                    playerController.ChangeAge(AgeType.Young);
                    UIManager.Instance.timerText.enabled = false;
                    UIManager.Instance.countText.enabled = false;
                    UIManager.Instance.goldText.enabled = false;
                    infoPanel.enabled = false;
                    break;
                case GameFlow.ҩ������ֳ�:
                    playerController.gameType = GameType.Normal;
                    playerController.ChangeAge(AgeType.Young);
                    UIManager.Instance.timerText.enabled = false;
                    UIManager.Instance.countText.enabled = false;
                    UIManager.Instance.goldText.enabled = false;
                    infoPanel.enabled = false;
                    break;
                case GameFlow.����:
                    playerController.gameType = GameType.Normal;
                    playerController.ChangeAge(AgeType.Young);
                    UIManager.Instance.timerText.enabled = false;
                    UIManager.Instance.countText.enabled = false;
                    UIManager.Instance.goldText.enabled = false;
                    infoPanel.enabled = false;
                    break;
                case GameFlow.�������ճ�:
                    playerController.gameType = GameType.Normal;
                    playerController.ChangeAge(AgeType.Young);
                    UIManager.Instance.timerText.enabled = false;
                    UIManager.Instance.countText.enabled = false;
                    UIManager.Instance.goldText.enabled = false;
                    infoPanel.enabled = false;
                    break;
                case GameFlow.�ξ�2:
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
                case GameFlow.��:
                    playerController.gameType = GameType.Normal;
                    playerController.ChangeAge(AgeType.Young);
                    UIManager.Instance.timerText.enabled = false;
                    UIManager.Instance.countText.enabled = false;
                    UIManager.Instance.goldText.enabled = false;
                    infoPanel.enabled = false;
                    break;
            }
            playerController.Gold = 0;
            if (playerController.gameType != GameType.Normal) playerController.playerInput.Enable();
            else if (playerController.gameType == GameType.Normal)
                DOTween.Sequence()
                .AppendInterval(1f)
                .AppendCallback(() => DialogManager.Instance.Triggle());
        })
        .AppendInterval(1f)
        .Append(fadePanel.DOColor(new Color(0, 0, 0, 0), 1f));
    }
}

