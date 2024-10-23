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

    public override void Interact()
    {
        transform.position = startPos.position;
        canInteract = false;
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOMove(endPos.position, drivingDuration))
            .AppendCallback(() =>
            {
                canInteract = true;
                Transform temp = startPos;
                startPos = endPos;
                endPos = temp;
                right = !right;
                if (right) transform.localScale = new Vector3(1, 1);
                else transform.localScale = new Vector3(-1, 1);
            });
    }
}
