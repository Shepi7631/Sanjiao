using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;
using UnityEditor;

public class DialogManager : SingletonBase<DialogManager>
{
    public List<TextAsset> dialogDataFile;  //对话文本文件，csv格式   
    public Image figure;//角色图像
    public Image dialogBox;//对话框
    public Image figure_bg;//角色图像底色
    public TMP_Text nameText;//角色名字文本
    public TMP_Text dialogText; //对话内容文本
    public List<Sprite> sprites = new List<Sprite>();//角色图片列表
    Dictionary<string, Sprite> imageDic = new Dictionary<string, Sprite>();//对应立绘名与角色图片的字典
    public Button nextbutton;//“继续”按钮
    public GameObject optionButton;//选项按钮
    public Transform buttonGroup;//选项按钮的排列方式
    public int dialogIndex;//保存当前对话索引值
    public List<dialogstate> state = new List<dialogstate>();//保存角色状态
    public dialogstate currentCharacter;
    public string[] dialogRows;  //存储分割的对话文本

    public void OnClickNext()
    {
        ShowDialogRow();
    }//绑定按钮，点击后会显示下一个文本

    private void Awake()
    {
        //存立绘
        imageDic["主角"] = sprites[0];//主角立绘1
        imageDic["NULL"] = sprites[1];
        imageDic["女儿"] = sprites[2];
        imageDic["站孩"] = sprites[3];
        imageDic["蹲孩"] = sprites[4];
        imageDic["工友"] = sprites[5];
        imageDic["妻子"] = sprites[6];
        imageDic["药店老板"] = sprites[7];
        imageDic["买药的人"] = sprites[8];
        imageDic["护卫军"] = sprites[9];
        imageDic["老板娘"] = sprites[10];
        imageDic["王朵"] = sprites[11];
        imageDic["兜帽人"] = sprites[12];
        imageDic["工头"] = sprites[13];

        //将初始化的角色存入list中
        dialogstate mainrole = new dialogstate();

        mainrole.effect = "0";
        mainrole.name = "主角";
        mainrole.dialogTextIndex = 0;
        state.Add(mainrole);

    }//初始化人物信息

    private void Start()
    {
        ReadText(dialogDataFile[state[0].dialogTextIndex]);

        nextbutton.gameObject.SetActive(false);

        dialogText.gameObject.SetActive(false);

        nameText.gameObject.SetActive(false);

        figure_bg.gameObject.SetActive(false);

        dialogBox.gameObject.SetActive(false);
        // ShowDialogRow();//修改下派生的函数
    }
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

        for (int i = 0; i < dialogRows.Length; i++)
        {
            string[] cells = dialogRows[i].Split(',');
            if (cells[0] == "#" && int.Parse(cells[1]) == dialogIndex)
            {
                if (cells[6] == "1")//检测为真的时候，修改跳转索引值（这里是主角）
                {

                    foreach (var person in state)
                    {
                        if (person.name == "主角")
                        {
                            cells[4] = person.effect;//主角的effect的修改在另外的函数中已经执行了
                            person.effect = "";
                        }
                    }

                }
                UpdateText(cells[7], cells[3]);//cells[7]是人名，cells[3]是对话内容
                UpdateImage(cells[2]);
                dialogIndex = int.Parse(cells[4]);//cells[4]是要跳转的索引值
                break;
            }
            else if (cells[0] == "&" && int.Parse(cells[1]) == dialogIndex)
            {
                nextbutton.gameObject.SetActive(false);
                GenerateOption(i);
            }
            else if (cells[0] == "END" && int.Parse(cells[1]) == dialogIndex)
            {


                /*测试用
                 UpdateText(cells[7], cells[3]);
                UpdateImage(cells[2]);
                dialogIndex = int.Parse(cells[4]);
                */

                nextbutton.gameObject.SetActive(false);
                dialogText.gameObject.SetActive(false);
                nameText.gameObject.SetActive(false);
                figure.gameObject.SetActive(false);
                dialogBox.gameObject.SetActive(false);
                figure_bg.gameObject.SetActive(false);
                dialogIndex = int.Parse(cells[5]);//赋一个flagid值用于确定下次开始的位置
                Debug.Log("flagid changed");

                break;
            }
        }

    }
    public void ShowDialogRow(int i)
    {
        for (int j = 1; j < dialogRows.Length; j++)
        {

            string[] cells = dialogRows[j].Split(',');
            if (int.Parse(cells[1]) < i)
                continue;

            if (cells[0] == "#" && int.Parse(cells[1]) == dialogIndex)
            {
                if (cells[6] == "1")//检测为真的时候，修改跳转索引值（这里是主角）
                {

                    foreach (var person in state)
                    {
                        //Debug.Log(person.effect);
                        if (person.name == "主角")
                        {
                            cells[4] = person.effect;//主角的effect的修改在另外的函数中已经执行了
                            person.effect = "";
                        }
                    }

                }
                //Debug.Log("将会跳转到");
                //Debug.Log(cells[4]);
                UpdateText(cells[7], cells[3]);//cells[7]是人名，cells[3]是对话内容
                UpdateImage(cells[2]);
                dialogIndex = int.Parse(cells[4]);//cells[4]是要跳转的索引值
                break;
            }
            else if (cells[0] == "&" && int.Parse(cells[1]) == dialogIndex)
            {
                nextbutton.gameObject.SetActive(false);
                GenerateOption(j);
            }
            else if (cells[0] == "END" && int.Parse(cells[1]) == dialogIndex)
            {

                /*测试用
                  UpdateText(cells[7], cells[3]);
                  UpdateImage(cells[2]);
                  dialogIndex = int.Parse(cells[4]);
                                */

                nextbutton.gameObject.SetActive(false);
                dialogText.gameObject.SetActive(false);
                nameText.gameObject.SetActive(false);
                figure.gameObject.SetActive(false);
                dialogBox.gameObject.SetActive(false);
                figure_bg.gameObject.SetActive(false);
                dialogIndex = int.Parse(cells[5]);//赋一个flagid值用于确定下次开始的位置                
                Debug.Log(cells[5]);

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
                    if (cells[5] != " ")
                        OptionEffect(cells[5], cells[7]);//修改state队列里主角的索引值
                    OnOptionClick(int.Parse(cells[4])); //cells[4]是需要跳转的序号

                }
            );
            GenerateOption(_index + 1);
        }

    }//选项按钮绑定事件
    public void OnOptionClick(int _id)
    {
        dialogIndex = _id;
        ShowDialogRow();
        for (int i = 0; i < buttonGroup.childCount; i++)
        {
            Destroy(buttonGroup.GetChild(i).gameObject);

        }
        nextbutton.gameObject.SetActive(true);
    }//选项按键事件

    public void OptionEffect(string _effect, string _name)
    {
        foreach (var person in state)
        {
            if (person.name == "主角")//如果有多个角色的话需要把主角改为_name
            {
                person.effect = _effect;//将选项带来的影响赋值给主角的effect
                Debug.Log(_name);
            }
        }

    }//产生效果的事件

    [ContextMenu("Dosomething")]
    public void triggle()
    {

        nextbutton.gameObject.SetActive(true);

        dialogText.gameObject.SetActive(true);

        nameText.gameObject.SetActive(true);

        figure.gameObject.SetActive(true);
        figure_bg.gameObject.SetActive(true);

        dialogBox.gameObject.SetActive(true);

        ReadText(dialogDataFile[state[0].dialogTextIndex]);

        ShowDialogRow(dialogIndex);
    }

    public void triggle(int TextIndex, int beginindex)
    {

        nextbutton.gameObject.SetActive(true);

        dialogText.gameObject.SetActive(true);

        nameText.gameObject.SetActive(true);

        figure.gameObject.SetActive(true);

        dialogBox.gameObject.SetActive(true);

        figure_bg.gameObject.SetActive(true);
        state[0].dialogTextIndex = TextIndex;

        ReadText(dialogDataFile[state[0].dialogTextIndex]);
        ShowDialogRow(beginindex);
    }


}
