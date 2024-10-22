using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : SingletonBase<EventManager>
{
    public static event Action<bool> OnChangeInteractInfoEvent;
    public static void ChangeInteractInfoEvent(bool state) { OnChangeInteractInfoEvent?.Invoke(state); }
}
