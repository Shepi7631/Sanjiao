using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType
{
    Idle, Run, Jump, Fall, Land, Dash
}

public class PlayerStateMachine : MonoBehaviour
{
    private Animator animator;

    private PlayerController playerController;

    private PlayerInput playerInput;

    private PlayerState curState;

    private Dictionary<PlayerStateType, PlayerState> stateDic = new();

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
        playerInput = GetComponent<PlayerInput>();

        stateDic.Add(PlayerStateType.Idle, new PlayerState_Idle(this, playerController, animator, playerInput));
        stateDic.Add(PlayerStateType.Run, new PlayerState_Run(this, playerController, animator, playerInput));
        stateDic.Add(PlayerStateType.Jump, new PlayerState_Jump(this, playerController, animator, playerInput));
        stateDic.Add(PlayerStateType.Fall, new PlayerState_Fall(this, playerController, animator, playerInput));
        stateDic.Add(PlayerStateType.Land, new PlayerState_Land(this, playerController, animator, playerInput));
        stateDic.Add(PlayerStateType.Dash, new PlayerState_Dash(this, playerController, animator, playerInput));
    }

    private void Start()
    {
        SwitchState(PlayerStateType.Idle);
    }

    private void Update()
    {
        curState.OnUpdate();
    }

    private void FixedUpdate()
    {
        curState.OnFixedUpdate();
    }

    public void SwitchState(PlayerStateType state)
    {
        if (curState != null)
        {
            curState.OnExit();
            //playerController.prePlayerState = playerController.curPlayerState;
        }
        ///playerController.curPlayerState = state;
        curState = stateDic[state];
        curState.OnEnter();
        Debug.Log(state);
    }


}
