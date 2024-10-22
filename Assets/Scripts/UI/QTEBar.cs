using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum QTEType { Bad, Good, Perfect }

public class QTEBar : MonoBehaviour
{
    [SerializeField] private Image leftBadFill;
    [SerializeField] private Image rightBadFill;

    [SerializeField] private Image leftGoodFill;
    [SerializeField] private Image rightGoodFill;

    private float length = 60;

    private Sequence sequence;

    public float HalfLength { get => length; }
    public float CurProgress
    {
        get => curProgress;
        set { curProgress = Mathf.Clamp(value, 0, maxProgress); progressBar.value = curProgress; }
    }

    [SerializeField] private RectTransform pointerTransform;

    [SerializeField] private TextMeshProUGUI pointerInfo;

    [SerializeField] private InfoPanel infoPanel;

    [SerializeField] private Slider progressBar;

    private float clickGap = 0.1f;

    private float badLen;

    private float goodLen;

    private float curValue = 0;

    private bool canInteract = false;

    private float waitTime = 1.5f;

    private float maxProgress = 100;

    private float curProgress = 0;

    private float leftBadFillRRange;

    private float leftGoodFillRRange;

    private float rightBadFillLRange;

    private float rightGoodFillLRange;

    private void Awake()
    {
        sequence = DOTween.Sequence()
            .Append(DOTween.To(() => curValue, x => curValue = x, 1, 1).SetEase(Ease.InOutQuad))
            .SetLoops(-1, LoopType.Yoyo)
            .Pause();
    }

    private void Start()
    {
        progressBar.maxValue = maxProgress;
        Disable();
    }

    private void OnEnable()
    {
        EventManager.OnMiningGameStartEvent += QTEStart;
    }

    private void OnDisable()
    {
        //EventManager.OnMiningGameStartEvent -= QTEStart;
    }
    private void Update()
    {
        pointerTransform.anchoredPosition = new Vector2(length * (curValue - 0.5f), pointerTransform.anchoredPosition.y);
        pointerInfo.text = curValue.ToString("F2");
    }

    private void InitQTEBar()
    {
        float offset = Random.Range(-badLen, badLen);
        leftBadFillRRange = badLen + offset;
        leftGoodFillRRange = badLen + goodLen + offset;

        rightBadFillLRange = 1 - (badLen - offset);
        rightGoodFillLRange = 1 - (badLen + goodLen - offset);

        leftBadFill.fillAmount = leftBadFillRRange;
        leftGoodFill.fillAmount = leftGoodFillRRange;

        rightBadFill.fillAmount = 1 - rightBadFillLRange;
        rightGoodFill.fillAmount = 1 - rightGoodFillLRange;
    }

    private void QTEStart(float badLen, float goodLen)
    {
        Enable();
        canInteract = false;
        curValue = 0;
        CurProgress = 0;

        this.badLen = badLen;
        this.goodLen = goodLen;

        InitQTEBar();

        DOTween.Sequence()
            .AppendCallback(() => infoPanel.ShowInfo("挖到矿了", waitTime))
            .AppendInterval(waitTime)
            .AppendCallback(() => { canInteract = true; sequence.Restart(); });

    }

    private QTEType Check()
    {
        if (curValue < leftBadFillRRange) { return QTEType.Bad; }
        else if (curValue >= leftBadFillRRange && curValue < leftGoodFillRRange) { return QTEType.Good; }
        else if (curValue >= rightGoodFillLRange && curValue < rightBadFillLRange) { return QTEType.Good; }
        else if (curValue >= rightBadFillLRange) { return QTEType.Bad; }
        else { return QTEType.Perfect; }
    }

    public void ClickOnce()
    {
        if (!canInteract) return;
        AudioManager.Instance.PlayEffect(AudioType.Dig);

        sequence.Pause();
        canInteract = false;
        QTEType checkState = Check();

        switch (checkState)
        {
            case QTEType.Bad:
                DOTween.Sequence()
            .AppendCallback(() => infoPanel.ShowInfo("失败了！", waitTime))
            .AppendInterval(waitTime)
            .AppendCallback(() => { EventManager.MiningGameEndEvent(false); Disable(); });
                break;
            case QTEType.Good:
            case QTEType.Perfect:
                CurProgress += Random.Range(10, 20);
                if (checkState == QTEType.Perfect) CurProgress += Random.Range(10, 20);

                if (curProgress < maxProgress)
                {
                    DOTween.Sequence()
                        .AppendCallback(() => { infoPanel.ShowInfo(checkState.ToString(), waitTime); })
                        .AppendInterval(waitTime)
                        .AppendCallback(() => { InitQTEBar(); canInteract = true; sequence.Play(); })
                        .AppendInterval(clickGap)
                        .AppendCallback(() => { canInteract = true; });
                }
                else
                {
                    DOTween.Sequence()
                        .AppendCallback(() => infoPanel.ShowInfo("成功了！", waitTime))
                        .AppendInterval(waitTime)
                        .AppendCallback(() => { EventManager.MiningGameEndEvent(true); Disable(); });
                }
                break;
        }

    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
