using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType
{
    Idle, Run, Jump, Fall, Land, Dash, Driving, Dig, QTE
}

public class PlayerStateMachine : MonoBehaviour
{
    private Animator animator;

    private PlayerController playerController;

    private PlayerInput playerInput;

    private PlayerState curState;

    private Dictionary<PlayerStateType, PlayerState> stateDic = new();

    private PlayerState_Driving drivingState;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
        playerInput = GetComponent<PlayerInput>();

        drivingState = new PlayerState_Driving(this, playerController, animator, playerInput);

        stateDic.Add(PlayerStateType.Idle, new PlayerState_Idle(this, playerController, animator, playerInput));
        stateDic.Add(PlayerStateType.Run, new PlayerState_Run(this, playerController, animator, playerInput));
        stateDic.Add(PlayerStateType.Jump, new PlayerState_Jump(this, playerController, animator, playerInput));
        stateDic.Add(PlayerStateType.Fall, new PlayerState_Fall(this, playerController, animator, playerInput));
        stateDic.Add(PlayerStateType.Land, new PlayerState_Land(this, playerController, animator, playerInput));
        stateDic.Add(PlayerStateType.Dash, new PlayerState_Dash(this, playerController, animator, playerInput));
        stateDic.Add(PlayerStateType.Driving, drivingState);
        stateDic.Add(PlayerStateType.Dig, new PlayerState_Dig(this, playerController, animator, playerInput));
        stateDic.Add(PlayerStateType.QTE, new PlayerState_QTE(this, playerController, animator, playerInput));
    }

    private void Start()
    {
        SwitchState(PlayerStateType.Idle);
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

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
            playerController.prePlayerStateType = playerController.curPlayerStateType;
        }
        playerController.curPlayerStateType = state;
        curState = stateDic[state];
        curState.OnEnter();
        Debug.Log(state);
    }

    public void SetDrivingParameter(Car car)
    {
        drivingState.SetParameter(car);
    }


}
