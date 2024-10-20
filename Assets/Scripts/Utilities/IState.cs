using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
}
