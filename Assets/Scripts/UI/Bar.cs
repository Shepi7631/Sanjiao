using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    private float maxValue;
    private float curValue;

    [SerializeField] private Image fast;
    [SerializeField] private Image slow;

    public void ChangeValue(float valueChange)
    {
        curValue += valueChange;
        fast.fillAmount = curValue / maxValue;
        float origin= slow.fillAmount;
        float target= fast.fillAmount;
        slow.fillAmount = Mathf.Lerp(origin, target, 0.5f);
    }

}
