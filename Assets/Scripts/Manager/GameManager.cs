using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : SingletonBase<GameManager>
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Image fadePanel;
    [SerializeField] private Image infoPanel;
    private float miningGameTimer = 0;
    public List<Vector3> playerBirthPos;


    public float ageTimer;

    private float maxTimer;

    private int curCount;

    private int maxCount;


    private int curLevel = 0;

    private void Awake()
    {
        curLevel = PlayerPrefs.GetInt("Level");
        DOTween.Sequence()
            .Append(fadePanel.DOColor(new Color(0, 0, 0, 1), 0.5f))
            .Append(fadePanel.DOColor(new Color(0, 0, 0, 0), 0.5f));
    }

    private void Start()
    {
        playerController.SetPos(playerBirthPos[curLevel]);
        ChangeLevel();
    }

    private void Update()
    {
        if (curLevel == 2)
        {
            ageTimer += Time.deltaTime;
            if (ageTimer >= maxTimer)
            {
                playerController.AgeForward();
                ageTimer = 0;
                curCount++;
                if (curCount >= maxCount) Remake();
            }
        }
        else if (curLevel == 1)
        {
            miningGameTimer += Time.deltaTime;
            if (miningGameTimer >= 60f)
            {
                miningGameTimer = 0;
                NextLevel();
            }
        }
        UIManager.Instance.timerText.text = "衰老倒计时：" + ageTimer.ToString("f2") + "/" + maxTimer.ToString();
        UIManager.Instance.countText.text = "衰老次数：" + curCount.ToString() + "/" + maxCount.ToString();
    }

    public void NextLevel()
    {
        playerController.SetPos(playerBirthPos[++curLevel]);
        ChangeLevel();

        PlayerPrefs.SetInt("Level", curLevel);
        PlayerPrefs.Save();
    }

    public void Remake()
    {
        SceneManager.LoadScene(0);
    }

    public void SetCurLevel(int level)
    {
        curLevel = level;
        PlayerPrefs.SetInt("Level", curLevel);
        PlayerPrefs.Save();
        Remake();
    }

    private void ChangeLevel()
    {
        switch (curLevel)
        {
            case 0:
                playerController.gameType = GameType.Dream;
                playerController.ChangeAge(AgeType.Children);
                infoPanel.enabled = false;
                break;
            case 1:
                playerController.gameType = GameType.Dig;
                playerController.ChangeAge(AgeType.Young);
                MiningGameManager.Instance.InitGame(-75, 75, 8, 16);
                infoPanel.enabled = true;
                break;
            case 2:
                playerController.gameType = GameType.Dream;
                playerController.ChangeAge(AgeType.Young);
                ageTimer = 0;
                maxTimer = 15;
                curCount = 0;
                maxCount = 5;
                infoPanel.enabled = false;
                break;
        }
        playerController.Gold = 0;
    }
}

