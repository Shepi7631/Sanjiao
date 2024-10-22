using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialog_Manager : MonoBehaviour
{
   
    public TextAsset dialogDataFile;  //对话文本文件，csv格式   
    public SpriteRenderer figure;//角色图像
    public TMP_Text nameText;//角色名字文本
    public TMP_Text dialogText; //对话内容文本
    public List<Sprite> sprites = new List<Sprite>();//角色图片列表
    public Button nextbutton;//continue按钮
    public GameObject optionButton;//选项按钮
    public Transform buttonGroup;//选项按钮的排列方式
    Dictionary<string,Sprite> imageDic=new Dictionary<string, Sprite>();//角色名字对应立绘字典
    public int dialogIndex;//保存当前对话索引值

    //对话文本按行分割
    public string[] dialogRows;

    public void OnClickNext()
    {
        ShowDialogRow();
    }//按下按钮后，会显示下一个文本

    private void Awake()
    {
        imageDic["Player_Idle_0"] = sprites[0];
        imageDic["Player_Idle_2"] = sprites[1];
    }//将图片素材存入数组

    public void UpdateText(string _name, string _text)
    {
        nameText.text = _name;
        dialogText.text = _text;

    }//更新文本

    public void UpdateImage(string _name)
    {
        figure.sprite = imageDic[_name];

    }//更新图像

    public void ReadText(TextAsset _textAsset)
    {
        dialogRows = _textAsset.text.Split('\n');
  
         Debug.Log("load success");
    }//读取对话文件

    public void ShowDialogRow()//展示对话文件对应的对话与立绘
    {
        for (int i=0;i<dialogRows.Length;i++)
        {
            string[] cells = dialogRows[i].Split(',');
            if (cells[0]=="#"&& int.Parse(cells[1]) == dialogIndex)
            {
                UpdateText(cells[2], cells[3]);//cells[2]是人名，cells[3]是对话内容
                UpdateImage(cells[2]);
                dialogIndex =int.Parse( cells[4]);//cells[4]是跳转序号
                break;
            }
            else if (cells[0]=="&"&& int.Parse(cells[1]) == dialogIndex)
            {
                nextbutton.gameObject.SetActive(false);
                GenerateOption(i);
            }
            else if (cells[0] == "END" && int.Parse(cells[1]) == dialogIndex)
            {
                nextbutton.gameObject.SetActive(false);
            }
        }

    }

    public void GenerateOption(int _index)
    {
        string[] cells = dialogRows[_index].Split(',');
        if (cells[0] == "&") 
        {
            GameObject button = Instantiate(optionButton, buttonGroup);//绑定按钮实例

            button.GetComponentInChildren<TMP_Text>().text = cells[3];//cells[3]是对话内容
            button.GetComponent<Button>().onClick.AddListener(
                delegate 
                { 
                OnOptionClick(int.Parse(cells[4])); //cells[4]是需要跳转的序号
                }
            );
            GenerateOption(_index + 1);
        }
    
     }//产生选项按钮

    public void OnOptionClick(int _id)
    {
        dialogIndex = _id;
        ShowDialogRow();
        for(int i=0;i<buttonGroup.childCount;i++)
        {
            Destroy(buttonGroup.GetChild(i).gameObject);
         }
        nextbutton.gameObject.SetActive(true);
    }//选项按键事件
    
    private void Start()
    {
        ReadText(dialogDataFile);
        ShowDialogRow();
    }

}
