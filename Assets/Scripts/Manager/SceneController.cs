using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void GetStart()
    {
        PlayerPrefs.SetInt("Level", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }
}
