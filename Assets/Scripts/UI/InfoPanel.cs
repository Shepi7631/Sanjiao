using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    private Text infoText;
    private Image image;

    private void Awake()
    {
        infoText = GetComponentInChildren<Text>();
        image = GetComponent<Image>();
    }
    public void ShowInfo(string text, float duration)
    {
        DOTween.Sequence()
            .Append(infoText.DOText(text, duration / 2))
            .Append(infoText.DOText("", duration / 2));
    }
}
