using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;

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
    public List<dialogstate> state=new List<dialogstate>();//保存主角的对话状态
    public string[] dialogRows;  //对话文本按行分割
    
   
        
    public void OnClickNext()
    {
        ShowDialogRow();
    }//按下按钮后，会显示下一个文本

    private void Awake()
    {
        imageDic["Player_Idle_0"] = sprites[0];
        imageDic["Player_Idle_2"] = sprites[1];
        dialogstate mainrole = new dialogstate();
        mainrole.flagid = "0";//具体情况的时候，初始化是要根据对话对象来确定的
        mainrole.effect = "0";
        mainrole.name = "主角";
        state.Add(mainrole);

    }//将初始化人物信息

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
        
        for (int i =0;i<dialogRows.Length;i++)
        {
            string[] cells = dialogRows[i].Split(',');
            if (cells[0]=="#"&& int.Parse(cells[1]) == dialogIndex)
            {
                if (cells[6]=="1")
                {
                    
                    foreach (var person in state)
                    {
                        //Debug.Log(person.name);
                        //Debug.Log(person.effect);
                        if (person.name == "主角")
                        {
                            cells[4]=person.effect;
                            person.effect = "0";
                        }
                    }

                }
                Debug.Log("将会跳转到");
                Debug.Log(cells[4]);
                UpdateText(cells[7], cells[3]);//cells[7]是人名，cells[3]是对话内容
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
                UpdateText(cells[7], cells[3]);//cells[7]是人名，cells[3]是对话内容
                UpdateImage(cells[2]);
                dialogIndex = int.Parse(cells[4]);//cells[4]是跳转序号
                break;
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
                    OptionEffect(cells[5], cells[7]);
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
    
    public void OptionEffect(string _effect,string _name)
    {
        foreach (var person in state)
        {
            if (person.name =="主角")//如果有多个角色的话需要把主角改为_name
            {
                person.effect = _effect;
                Debug.Log(_name);
             }
        }
      
    }//产生效果的事件
     private void Start()
    {
        ReadText(dialogDataFile);
        ShowDialogRow();
    }

}
