using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : InteractableItem
{

    public override void Interact()
    {
        GameManager.Instance.ageTimer = 0;
        gameObject.SetActive(false);
    }
}
