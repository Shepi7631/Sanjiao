using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogstate : MonoBehaviour
{
    // Start is called before the first frame update
    public string name;
    public string flagid;//用于存储本次对话开始的flagid，flagid在对话触发以后开始初始化
    public string effect;//用来存储对话带来的flag的变动
}
