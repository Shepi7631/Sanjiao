using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog_Manager : MonoBehaviour
{
    //�Ի��ı��ļ���csv��ʽ
    public TextAsset dialogDataFile;

    //��ɫͼ��
    public SpriteRenderer figure;

    //��ɫ�����ı�
    public TMP_Text nameText;

    //�Ի������ı�
    public TMP_Text dialogText;

    //��ɫͼƬ�б�
    public List<Sprite> sprites = new List<Sprite>();

    //��ɫ���ֶ�Ӧ�����ֵ�
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
    }//��ͼƬ��������
    private void Start()
    {
        UpdateText("����","���ǲ����ı�");
        UpdateImage("Player_Idle_2");
    }

}
