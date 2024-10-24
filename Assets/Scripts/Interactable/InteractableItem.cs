using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public enum ItemStateType { Close, Interact, Used }
public abstract class InteractableItem : MonoBehaviour
{
    public ItemStateType curState;
    public string itemName;
    public abstract void Interact();
}
