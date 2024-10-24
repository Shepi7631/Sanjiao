using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : InteractableItem
{
    public Transform startPos;
    public Transform endPos;
    public float drivingDuration;
    public bool right;

    private void Awake()
    {
        if (endPos.position.x >= startPos.position.x) right = true;
        else right = false;
    }

    public override void Interact()
    {
        transform.position = startPos.position;
        curState = ItemStateType.Close;
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOMove(endPos.position, drivingDuration));
    }
}
