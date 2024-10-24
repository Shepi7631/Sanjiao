using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : InteractableItem
{
    public List<Transform> targets = new List<Transform>();
    public float drivingDuration;
    public bool right;

    private void Awake()
    {
        if (targets[0].position.x >= targets[targets.Count - 1].position.x) right = true;
        else right = false;
    }

    public override void Interact()
    {
        curState = ItemStateType.Close;
        Sequence sequence = DOTween.Sequence();
        foreach (var target in targets)
        {
            sequence.Append(transform.DOMove(target.position, 1));
        }
    }
}
