using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public abstract class InteractableItem : MonoBehaviour
{
    public bool canInteract = true;
    public string itemName;
    public abstract void Interact();
}
