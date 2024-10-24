using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBase<GameManager>
{
    [SerializeField] private PlayerController playerController;
    public List<Vector3> playerBirthPos;

    private float ageTimer;

    private int curLevel = 0;

    private void Awake()
    {
        curLevel = PlayerPrefs.GetInt("Level");
    }

    private void Start()
    {
        playerController.SetPos(playerBirthPos[curLevel]);
        ChangeLevel();

    }

    private void Update()
    {
        
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
                break;
            case 1:
                playerController.gameType = GameType.Dig;
                playerController.ChangeAge(AgeType.Young);
                MiningGameManager.Instance.InitGame(-75, 75, 8, 16);
                break;
            case 2:
                playerController.gameType = GameType.Dream;
                playerController.ChangeAge(AgeType.Children);
                break;
        }
        playerController.Gold = 0;
    }
}
