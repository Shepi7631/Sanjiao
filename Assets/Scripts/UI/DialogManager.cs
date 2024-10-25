using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;
using UnityEditor;

public class DialogManager : SingletonBase<DialogManager>
{
    public List<TextAsset> dialogDataFile;  //�Ի��ı��ļ���csv��ʽ   
    public SpriteRenderer figure;//��ɫͼ��
    public SpriteRenderer backGround;//�Ի���
    public TMP_Text nameText;//��ɫ�����ı�
    public TMP_Text dialogText; //�Ի������ı�
    public List<Sprite> sprites = new List<Sprite>();//��ɫͼƬ�б�
    Dictionary<string, Sprite> imageDic = new Dictionary<string, Sprite>();//��Ӧ���������ɫͼƬ���ֵ�
    public Button nextbutton;//����������ť
    public GameObject optionButton;//ѡ�ť
    public Transform buttonGroup;//ѡ�ť�����з�ʽ
    public int dialogIndex;//���浱ǰ�Ի�����ֵ
    public List<dialogstate> state = new List<dialogstate>();//�����ɫ״̬
    public dialogstate currentCharacter;
    public string[] dialogRows;  //�洢�ָ�ĶԻ��ı�

    public void OnClickNext()
    {
        ShowDialogRow();
    }//�󶨰�ť����������ʾ��һ���ı�

    private void Awake()
    {
        //������
        imageDic["����"] = sprites[0];//��������1
        imageDic["NULL"] = sprites[1];//��������2
        imageDic["Ů��"] = sprites[2];
        imageDic["С��1"] = sprites[3];
        imageDic["С��2"] = sprites[4];
        imageDic["����"] = sprites[5];
        imageDic["����"] = sprites[6];
        imageDic["ҩ���ϰ�"] = sprites[7];
        imageDic["��ҩ����"] = sprites[8];
        imageDic["������"] = sprites[9];
        imageDic["����"] = sprites[6];
        imageDic["��ͷ"] = sprites[5];
        imageDic["�ϰ���"] = sprites[2];
        imageDic["��ñ��"] = sprites[7];

        //����ʼ���Ľ�ɫ����list��
        dialogstate mainrole = new dialogstate();

        mainrole.effect = "0";
        mainrole.name = "����";
        mainrole.dialogTextIndex = 6;
        state.Add(mainrole);

    }//��ʼ��������Ϣ

    private void Start()
    {
        ReadText(dialogDataFile[state[0].dialogTextIndex]);

        nextbutton.gameObject.SetActive(false);

        dialogText.gameObject.SetActive(false);

        nameText.gameObject.SetActive(false);

        figure.gameObject.SetActive(false);

        backGround.gameObject.SetActive(false);
        // ShowDialogRow();//�޸��������ĺ���
    }
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

        for (int i = 0; i < dialogRows.Length; i++)
        {
            string[] cells = dialogRows[i].Split(',');
            if (cells[0] == "#" && int.Parse(cells[1]) == dialogIndex)
            {
                if (cells[6] == "1")//���Ϊ���ʱ���޸���ת����ֵ�����������ǣ�
                {

                    foreach (var person in state)
                    {
                        if (person.name == "����")
                        {
                            cells[4] = person.effect;//���ǵ�effect���޸�������ĺ������Ѿ�ִ����
                            person.effect = "";
                        }
                    }

                }
                UpdateText(cells[7], cells[3]);//cells[7]��������cells[3]�ǶԻ�����
                UpdateImage(cells[2]);
                dialogIndex = int.Parse(cells[4]);//cells[4]��Ҫ��ת������ֵ
                break;
            }
            else if (cells[0] == "&" && int.Parse(cells[1]) == dialogIndex)
            {
                nextbutton.gameObject.SetActive(false);
                GenerateOption(i);
            }
            else if (cells[0] == "END" && int.Parse(cells[1]) == dialogIndex)
            {


                /*������
                 UpdateText(cells[7], cells[3]);
                UpdateImage(cells[2]);
                dialogIndex = int.Parse(cells[4]);
                */

                nextbutton.gameObject.SetActive(false);
                dialogText.gameObject.SetActive(false);
                nameText.gameObject.SetActive(false);
                figure.gameObject.SetActive(false);
                backGround.gameObject.SetActive(false);
                dialogIndex = int.Parse(cells[5]);//��һ��flagidֵ����ȷ���´ο�ʼ��λ��
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
                if (cells[6] == "1")//���Ϊ���ʱ���޸���ת����ֵ�����������ǣ�
                {

                    foreach (var person in state)
                    {
                        //Debug.Log(person.effect);
                        if (person.name == "����")
                        {
                            cells[4] = person.effect;//���ǵ�effect���޸�������ĺ������Ѿ�ִ����
                            person.effect = "";
                        }
                    }

                }
                //Debug.Log("������ת��");
                //Debug.Log(cells[4]);
                UpdateText(cells[7], cells[3]);//cells[7]��������cells[3]�ǶԻ�����
                UpdateImage(cells[2]);
                dialogIndex = int.Parse(cells[4]);//cells[4]��Ҫ��ת������ֵ
                break;
            }
            else if (cells[0] == "&" && int.Parse(cells[1]) == dialogIndex)
            {
                nextbutton.gameObject.SetActive(false);
                GenerateOption(j);
            }
            else if (cells[0] == "END" && int.Parse(cells[1]) == dialogIndex)
            {

                /*������
                  UpdateText(cells[7], cells[3]);
                  UpdateImage(cells[2]);
                  dialogIndex = int.Parse(cells[4]);
                                */

                nextbutton.gameObject.SetActive(false);
                dialogText.gameObject.SetActive(false);
                nameText.gameObject.SetActive(false);
                figure.gameObject.SetActive(false);
                backGround.gameObject.SetActive(false);

                dialogIndex = int.Parse(cells[5]);//��һ��flagidֵ����ȷ���´ο�ʼ��λ��                
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
            GameObject button = Instantiate(optionButton, buttonGroup);//�󶨰�ťʵ��

            button.GetComponentInChildren<TMP_Text>().text = cells[3];//cells[3]�ǶԻ�����
            button.GetComponent<Button>().onClick.AddListener(
                delegate
                {
                    if (cells[5] != " ")
                        OptionEffect(cells[5], cells[7]);//�޸�state���������ǵ�����ֵ
                    OnOptionClick(int.Parse(cells[4])); //cells[4]����Ҫ��ת�����

                }
            );
            GenerateOption(_index + 1);
        }

    }//ѡ�ť���¼�
    public void OnOptionClick(int _id)
    {
        dialogIndex = _id;
        ShowDialogRow();
        for (int i = 0; i < buttonGroup.childCount; i++)
        {
            Destroy(buttonGroup.GetChild(i).gameObject);

        }
        nextbutton.gameObject.SetActive(true);
    }//ѡ����¼�

    public void OptionEffect(string _effect, string _name)
    {
        foreach (var person in state)
        {
            if (person.name == "����")//����ж����ɫ�Ļ���Ҫ�����Ǹ�Ϊ_name
            {
                person.effect = _effect;//��ѡ�������Ӱ�츳ֵ�����ǵ�effect
                Debug.Log(_name);
            }
        }

    }//����Ч�����¼�

    public void triggle()
    {

        nextbutton.gameObject.SetActive(true);

        dialogText.gameObject.SetActive(true);

        nameText.gameObject.SetActive(true);

        figure.gameObject.SetActive(true);

        backGround.gameObject.SetActive(true);

        ReadText(dialogDataFile[state[0].dialogTextIndex]);

        ShowDialogRow(dialogIndex);
    }

    public void triggle(int TextIndex, int beginindex)
    {

        nextbutton.gameObject.SetActive(true);

        dialogText.gameObject.SetActive(true);

        nameText.gameObject.SetActive(true);

        figure.gameObject.SetActive(true);

        backGround.gameObject.SetActive(true);
        state[0].dialogTextIndex = TextIndex;
        ReadText(dialogDataFile[state[0].dialogTextIndex]);
        ShowDialogRow(beginindex);
    }


    public void StartConversation(NPC npc)
    {
        nextbutton.gameObject.SetActive(true);

        dialogText.gameObject.SetActive(true);

        nameText.gameObject.SetActive(true);

        figure.gameObject.SetActive(true);

        backGround.gameObject.SetActive(true);

        ReadText(dialogDataFile[npc.myInfo.InfoList[0].textIndex]);

        ShowDialogRow(npc.myInfo.InfoList[0].startPos);
    }
}
