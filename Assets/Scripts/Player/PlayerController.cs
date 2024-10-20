using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    #region 数值
    private float moveSpeed = 5;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }
    private float jumpSpeed = 6;
    public float JumpSpeed { get => jumpSpeed; private set => jumpSpeed = value; }
    private float dashCD = 2.5f;
    private float jumpBufferTime = 0.25f;
    private float landTime = 0.125f;
    public float LandTime { get => landTime; }
    #endregion

    #region 状态
    public bool Jump => playerInput.Jump;
    private bool canJump = false;
    public bool CanJump { get => canJump; private set => canJump = value; }

    private float jumpBuffer = 0;
    public float JumpBuffer { get => jumpBuffer; private set => jumpBuffer = value; }
    public bool HaveJumpBuffer
    {
        get => JumpBuffer > 0;
    }
    public bool right = true;
    public bool IsGround => groundDetector.IsGround;
    public bool Falling => !IsGround && rb.velocity.y < 0;
    private float curDashCD = 0;
    public float CurDashCD { get => curDashCD; private set => curDashCD = value; }
    public bool CanDash
    {
        get => CurDashCD < 0;
    }
    //public PlayerStateType curPlayerState;
    //public PlayerStateType prePlayerState;
    #endregion

    #region 组件
    private PlayerStateMachine stateMachine;
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private GroundDetector groundDetector;
    private Animator animator;
    #endregion


    private void Awake()
    {
        stateMachine = GetComponent<PlayerStateMachine>();
        playerInput = GetComponent<PlayerInput>();
        groundDetector = GetComponentInChildren<GroundDetector>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CurDashCD -= Time.deltaTime;
        JumpBuffer -= Time.deltaTime;
        if (playerInput.Jump) JumpBuffer = jumpBufferTime;
    }

    public void SetVelocityX(float x)
    {
        rb.velocity = new Vector3(x, rb.velocity.y, 0);
        if (x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            right = false;
        }
        else if (x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            right = true;
        }
    }

    public void SetVelocityY(float y)
    {
        rb.velocity = new Vector3(rb.velocity.x, y, 0);
    }

    public void ResetJump()
    {
        CanJump = true;
    }

    public void JumpOver()
    {
        CanJump = false;
        JumpBuffer = 0;
    }

    public void DashOver()
    {
        CurDashCD = dashCD;
    }
}
