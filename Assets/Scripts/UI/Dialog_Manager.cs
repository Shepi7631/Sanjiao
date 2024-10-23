using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;

public class Dialog_Manager : MonoBehaviour
{
   
    public TextAsset dialogDataFile;  //�Ի��ı��ļ���csv��ʽ   
    public SpriteRenderer figure;//��ɫͼ��
    public TMP_Text nameText;//��ɫ�����ı�
    public TMP_Text dialogText; //�Ի������ı�
    public List<Sprite> sprites = new List<Sprite>();//��ɫͼƬ�б�
    public Button nextbutton;//continue��ť
    public GameObject optionButton;//ѡ�ť
    public Transform buttonGroup;//ѡ�ť�����з�ʽ
    Dictionary<string,Sprite> imageDic=new Dictionary<string, Sprite>();//��ɫ���ֶ�Ӧ�����ֵ�
    public int dialogIndex;//���浱ǰ�Ի�����ֵ
    public List<dialogstate> state=new List<dialogstate>();//�������ǵĶԻ�״̬
    public string[] dialogRows;  //�Ի��ı����зָ�
    
   
        
    public void OnClickNext()
    {
        ShowDialogRow();
    }//���°�ť�󣬻���ʾ��һ���ı�

    private void Awake()
    {
        imageDic["Player_Idle_0"] = sprites[0];
        imageDic["Player_Idle_2"] = sprites[1];
        dialogstate mainrole = new dialogstate();
        mainrole.flagid = "0";//���������ʱ�򣬳�ʼ����Ҫ���ݶԻ�������ȷ����
        mainrole.effect = "0";
        mainrole.name = "����";
        state.Add(mainrole);

    }//����ʼ��������Ϣ

    public void UpdateText(string _name, string _text)
    {
        nameText.text = _name;
        dialogText.text = _text;

    }//�����ı�

    public void UpdateImage(string _name)
    {
        figure.sprite = imageDic[_name];

    }//����ͼ��

    public void ReadText(TextAsset _textAsset)
    {
        dialogRows = _textAsset.text.Split('\n');
  
         Debug.Log("load success");
    }//��ȡ�Ի��ļ�

    public void ShowDialogRow()//չʾ�Ի��ļ���Ӧ�ĶԻ�������
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
                        if (person.name == "����")
                        {
                            cells[4]=person.effect;
                            person.effect = "0";
                        }
                    }

                }
                Debug.Log("������ת��");
                Debug.Log(cells[4]);
                UpdateText(cells[7], cells[3]);//cells[7]��������cells[3]�ǶԻ�����
                UpdateImage(cells[2]);
                dialogIndex =int.Parse( cells[4]);//cells[4]����ת���
                break;
            }
            else if (cells[0]=="&"&& int.Parse(cells[1]) == dialogIndex)
            {
                nextbutton.gameObject.SetActive(false);
                GenerateOption(i);
            }
            else if (cells[0] == "END" && int.Parse(cells[1]) == dialogIndex)
            {
                UpdateText(cells[7], cells[3]);//cells[7]��������cells[3]�ǶԻ�����
                UpdateImage(cells[2]);
                dialogIndex = int.Parse(cells[4]);//cells[4]����ת���
                break;
            }
        }

    }

    public void GenerateOption(int _index)
    {
        string[] cells = dialogRows[_index].Split(',');
        if (cells[0] == "&") 
        {
            GameObject button = Instantiate(optionButton, buttonGroup);//�󶨰�ťʵ��

            button.GetComponentInChildren<TMP_Text>().text = cells[3];//cells[3]�ǶԻ�����
            button.GetComponent<Button>().onClick.AddListener(
                delegate 
                {
                    OptionEffect(cells[5], cells[7]);
                    OnOptionClick(int.Parse(cells[4])); //cells[4]����Ҫ��ת�����
                   
                }
            );
            GenerateOption(_index + 1);
        }
    
     }//����ѡ�ť

    public void OnOptionClick(int _id)
    {
        dialogIndex = _id;
        ShowDialogRow();
        for(int i=0;i<buttonGroup.childCount;i++)
        {
            Destroy(buttonGroup.GetChild(i).gameObject);
            
         }
        nextbutton.gameObject.SetActive(true);
    }//ѡ����¼�
    
    public void OptionEffect(string _effect,string _name)
    {
        foreach (var person in state)
        {
            if (person.name =="����")//����ж����ɫ�Ļ���Ҫ�����Ǹ�Ϊ_name
            {
                person.effect = _effect;
                Debug.Log(_name);
             }
        }
      
    }//����Ч�����¼�
     private void Start()
    {
        ReadText(dialogDataFile);
        ShowDialogRow();
    }

}
