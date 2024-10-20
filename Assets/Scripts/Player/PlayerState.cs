using System.Collections;
using System.Collections.Generic;
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
