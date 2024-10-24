using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : InteractableItem
{
    public override void Interact()
    {
        gameObject.SetActive(false);
    }
}
