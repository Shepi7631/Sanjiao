using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog_Manager : MonoBehaviour
{
    //对话文本文件，csv格式
    public TextAsset dialogDataFile;

    //角色图像
    public SpriteRenderer figure;

    //角色名字文本
    public TMP_Text nameText;

    //对话内容文本
    public TMP_Text dialogText;

    //角色图片列表
    public List<Sprite> sprites = new List<Sprite>();

    //角色名字对应立绘字典
    Dictionary<string,Sprite> imageDic=new Dictionary<string, Sprite>();
    public void UpdateText(string _name, string _text)
    {
        nameText.text = _name;
        dialogText.text = _text;

    }

    public void UpdateImage(string _name)
    {
        figure.sprite = imageDic[_name];

    }

    public void ReadText()
    {

    }
    
    private void Awake()
    {
        imageDic["Player_Idle_0"] = sprites[0];
        imageDic["Player_Idle_2"] = sprites[1];
    }//将图片存入数组
    private void Start()
    {
        UpdateText("主角","这是测试文本");
        UpdateImage("Player_Idle_2");
    }

}
