using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum QTEType { Bad, Good, Perfect }

public class QTEBar : MonoBehaviour
{
    [SerializeField] private Image leftBadFill;
    [SerializeField] private Image rightBadFill;

    [SerializeField] private Image leftGoodFill;
    [SerializeField] private Image rightGoodFill;

    private float halfLength = 30;

    private Sequence sequence;

    public float HalfLength { get => halfLength; }

    private RectTransform pointerTransform;

    private float curValue = -1;

    private bool canInteract = false;

    private void Awake()
    {
        sequence = DOTween.Sequence()
            .Append(DOTween.To(() => curValue, x => curValue = x, 1, 1).SetEase(Ease.InOutQuad))
            .SetLoops(-1, LoopType.Yoyo);
        pointerTransform = transform.Find("Pointer").GetComponent<RectTransform>();
    }

    private void Start()
    {

    }
    private void Update()
    {
        pointerTransform.anchoredPosition = new Vector2(halfLength * curValue, pointerTransform.anchoredPosition.y);
    }

    public void QTEStart(float badLen, float goodLen)
    {
        gameObject.SetActive(true);

        float offset = Random.Range(-badLen, badLen);

        leftBadFill.fillAmount = badLen + offset;
        leftGoodFill.fillAmount = badLen + goodLen + offset;

        rightBadFill.fillAmount = badLen - offset;
        rightGoodFill.fillAmount = badLen + goodLen - offset;

        DOTween.Sequence()
            .AppendInterval(1f)
            .AppendCallback(() => { canInteract = true; sequence.Restart(); });
    }

    public void Check()
    {
        if (!canInteract) return;

        gameObject.SetActive(false);
    }
}
