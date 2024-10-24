using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogstate : MonoBehaviour
{
    // Start is called before the first frame update
    public new string name;
    static public string flagid;//用于存储本次对话开始的位置
    public string effect;//用来存储到达判定点后需要跳转的位置
}
