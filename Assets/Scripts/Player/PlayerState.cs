using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerState : IState
{
    protected PlayerStateMachine playerStateMachine;
    protected PlayerController playerController;
    protected Animator animator;
    protected PlayerInput playerInput;

    public PlayerState(PlayerStateMachine stateMachine, PlayerController playerController, Animator animator, PlayerInput playerInput)
    {
        this.playerStateMachine = stateMachine;
        this.playerController = playerController;
        this.animator = animator;
        this.playerInput = playerInput;
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
}

public class PlayerState_Idle : PlayerState
{
    public PlayerState_Idle(PlayerStateMachine stateMachine, PlayerController playerController, Animator animator, PlayerInput playerInput) : base(stateMachine, playerController, animator, playerInput)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        animator.Play("Idle");
        playerController.SetVelocityX(0);
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (playerController.IsGround && playerController.Jump && playerController.CanJump) playerStateMachine.SwitchState(PlayerStateType.Jump);
        else if (playerInput.Dash && playerController.CanDash) playerStateMachine.SwitchState(PlayerStateType.Dash);
        else if (playerController.Falling) playerStateMachine.SwitchState(PlayerStateType.Fall);
        else if (playerInput.IsMoving) playerStateMachine.SwitchState(PlayerStateType.Run);
        else if (playerInput.Fire) playerController.Interact();
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}

public class PlayerState_Run : PlayerState
{
    public PlayerState_Run(PlayerStateMachine stateMachine, PlayerController playerController, Animator animator, PlayerInput playerInput) : base(stateMachine, playerController, animator, playerInput)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        animator.Play("Run");
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (playerController.IsGround && playerController.Jump && playerController.CanJump) playerStateMachine.SwitchState(PlayerStateType.Jump);
        else if (playerInput.Dash && playerController.CanDash) playerStateMachine.SwitchState(PlayerStateType.Dash);
        else if (playerController.Falling) playerStateMachine.SwitchState(PlayerStateType.Fall);
        else if (!playerInput.IsMoving) playerStateMachine.SwitchState(PlayerStateType.Idle);

    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        playerController.SetVelocityX(playerController.MoveSpeed * playerInput.MoveX);
    }
}

public class PlayerState_Jump : PlayerState
{
    public PlayerState_Jump(PlayerStateMachine stateMachine, PlayerController playerController, Animator animator, PlayerInput playerInput) : base(stateMachine, playerController, animator, playerInput)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        animator.Play("Jump");
        playerController.JumpOver();
        playerController.SetVelocityY(playerController.JumpSpeed);
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (playerInput.Dash && playerController.CanDash) playerStateMachine.SwitchState(PlayerStateType.Dash);
        else if (playerController.Falling) playerStateMachine.SwitchState(PlayerStateType.Fall);
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        playerController.SetVelocityX(playerController.MoveSpeed * playerInput.MoveX);
    }
}

public class PlayerState_Fall : PlayerState
{
    public PlayerState_Fall(PlayerStateMachine stateMachine, PlayerController playerController, Animator animator, PlayerInput playerInput) : base(stateMachine, playerController, animator, playerInput)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        animator.Play("Fall");
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (playerController.Jump && playerController.CanJump) playerStateMachine.SwitchState(PlayerStateType.Jump);
        else if (playerInput.Dash && playerController.CanDash) playerStateMachine.SwitchState(PlayerStateType.Dash);
        else if (playerController.IsGround) playerStateMachine.SwitchState(PlayerStateType.Land);
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        playerController.SetVelocityX(playerController.MoveSpeed * playerInput.MoveX);
    }
}

public class PlayerState_Land : PlayerState
{
    private float timer = 0;
    public PlayerState_Land(PlayerStateMachine stateMachine, PlayerController playerController, Animator animator, PlayerInput playerInput) : base(stateMachine, playerController, animator, playerInput)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        animator.Play("Land");
        timer = 0;
        playerController.SetVelocityX(0);
        playerController.ResetJump();
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        timer += Time.deltaTime;
        if (playerController.HaveJumpBuffer) playerStateMachine.SwitchState(PlayerStateType.Jump);
        else if (timer >= playerController.LandTime) playerStateMachine.SwitchState(PlayerStateType.Idle);
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}

public class PlayerState_Dash : PlayerState
{
    private float timer = 0;
    public PlayerState_Dash(PlayerStateMachine stateMachine, PlayerController playerController, Animator animator, PlayerInput playerInput) : base(stateMachine, playerController, animator, playerInput)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        animator.Play("Dash");
        timer = 0;
        playerInput.Disable();
        playerController.DashOver();
        if (playerController.right)
            playerController.SetVelocityX(playerController.MoveSpeed * 2.5f);
        else
            playerController.SetVelocityX(-playerController.MoveSpeed * 2.5f);
    }
    public override void OnExit()
    {
        base.OnExit();
        playerInput.Enable();
        playerController.SetVelocityX(0);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        playerController.SetVelocityY(0);
        timer += Time.deltaTime;
        if (timer >= 0.3f) playerStateMachine.SwitchState(PlayerStateType.Idle);
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}

public class PlayerState_Driving : PlayerState
{
    private Car car;
    private float timer = 0;
    public PlayerState_Driving(PlayerStateMachine stateMachine, PlayerController playerController, Animator animator, PlayerInput playerInput) : base(stateMachine, playerController, animator, playerInput)
    {
    }

    public void SetParameter(Car car) { this.car = car; }

    public override void OnEnter()
    {
        base.OnEnter();
        animator.Play("Dash");
        timer = 0;
        playerInput.Disable();
        if (car.right) playerController.SetVelocityX(float.Epsilon);
        else playerController.SetVelocityX(-float.Epsilon);
        playerController.CanIteract = false;
        playerController.DisableGravity();
        playerController.SetVelocityX(0);
        playerController.SetVelocityY(0);
    }
    public override void OnExit()
    {
        base.OnExit();
        playerInput.Enable();
        playerController.CanIteract = true;
        playerController.EnableGravity();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        timer += Time.deltaTime;
        if (timer >= car.drivingDuration) playerStateMachine.SwitchState(PlayerStateType.Idle);
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        playerController.SetPos(car.transform.position);
    }
}

public class PlayerState_Dig : PlayerState
{
    private float timer = 0;
    public PlayerState_Dig(PlayerStateMachine stateMachine, PlayerController playerController, Animator animator, PlayerInput playerInput) : base(stateMachine, playerController, animator, playerInput)
    {
    }


    public override void OnEnter()
    {
        base.OnEnter();
        animator.Play("Dig");
        timer = 0;
        playerInput.Disable();
        playerController.SetVelocityX(0);
        playerController.SetVelocityY(0);
        AudioManager.Instance.PlayEffect(AudioType.Dig);
    }
    public override void OnExit()
    {
        base.OnExit();
        playerInput.Enable();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        timer += Time.deltaTime;
        if (timer >= 0.25f)
        {
            if (MiningGameManager.Instance.DigFeedBack(playerController.transform.position.x)) playerStateMachine.SwitchState(PlayerStateType.QTE);
            else playerStateMachine.SwitchState(PlayerStateType.Idle);
        }
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}

public class PlayerState_QTE : PlayerState
{
    public PlayerState_QTE(PlayerStateMachine stateMachine, PlayerController playerController, Animator animator, PlayerInput playerInput) : base(stateMachine, playerController, animator, playerInput)
    {
    }


    public override void OnEnter()
    {
        base.OnEnter();
        animator.Play("Player_Idle");
        EventManager.OnMiningGameEndEvent += Exit;
        EventManager.MiningGameStartEvent(playerController.QTEBarBadLen, playerController.QTEBarGoodLen);
    }
    public override void OnExit()
    {
        base.OnExit();
        EventManager.OnMiningGameEndEvent -= Exit;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (playerInput.Fire)
        {
            playerController.qteBar.ClickOnce();
        }
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    private void Exit(bool state)
    {
        playerStateMachine.SwitchState(PlayerStateType.Idle);
        if (state)
        {
            playerController.CurMineralCount++;
            AudioManager.Instance.PlayEffect(AudioType.Win);
        }
        else
        {
            AudioManager.Instance.PlayEffect(AudioType.Lose);
        }

    }
}
