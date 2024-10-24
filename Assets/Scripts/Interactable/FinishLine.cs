using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : InteractableItem
{
    public int goldLimit;
    public override void Interact()
    {
        AudioManager.Instance.PlayEffect(AudioType.Open);
        GameManager.Instance.NextLevel();
    }
}
