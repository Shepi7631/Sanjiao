using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : SingletonBase<EventManager>
{
    public static event Action<bool> OnChangeInteractInfoEvent;
    public static void ChangeInteractInfoEvent(bool state) { OnChangeInteractInfoEvent?.Invoke(state); }

    public static event Action<float, float> OnMiningGameStartEvent;
    public static void MiningGameStartEvent(float badLen, float goodLen) { OnMiningGameStartEvent?.Invoke(badLen, goodLen); }

    public static event Action<bool> OnMiningGameEndEvent;
    public static void MiningGameEndEvent(bool state) { OnMiningGameEndEvent?.Invoke(state); }
}
