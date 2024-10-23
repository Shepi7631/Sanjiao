using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
public enum GameType { Normal, Dream, Dig }

public class PlayerController : MonoBehaviour
{
    #region 数值
    private float moveSpeed = 6;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }
    private float jumpSpeed = 18f;
    public float JumpSpeed { get => jumpSpeed; private set => jumpSpeed = value; }
    private float dashCD = 2.5f;
    private float jumpBufferTime = 0.25f;
    private float landTime = 0.125f;
    public float LandTime { get => landTime; }
    private float qteBarBadLen = 0.3f;
    private float qteBarGoodLen = 0.15f;
    public float QTEBarBadLen { get => qteBarBadLen; set => qteBarBadLen = value; }
    public float QTEBarGoodLen { get => qteBarGoodLen; set => qteBarGoodLen = value; }
    private float originGravityScale;
    #endregion

    #region 状态
    private int curMineralCount;
    public int CurMineralCount { get => curMineralCount; set => curMineralCount = value; }
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
    private bool canIteract = true;
    public bool CanIteract { get => canIteract; set => canIteract = value; }

    [SerializeField] private GameType gameType;
    //public PlayerStateType curPlayerState;
    //public PlayerStateType prePlayerState;
    #endregion

    #region 组件
    private PlayerStateMachine stateMachine;
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private GroundDetector groundDetector;
    private InteractableItemDetector interactableItemDetector;
    private Animator animator;
    private Canvas canvas;
    public QTEBar qteBar;
    #endregion


    private void Awake()
    {
        stateMachine = GetComponent<PlayerStateMachine>();
        playerInput = GetComponent<PlayerInput>();
        groundDetector = GetComponentInChildren<GroundDetector>();
        interactableItemDetector = GetComponentInChildren<InteractableItemDetector>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        originGravityScale = rb.gravityScale;
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

    public void DisableGravity()
    {
        rb.gravityScale = 0;
    }

    public void EnableGravity()
    {
        rb.gravityScale = originGravityScale;
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

    public void SetPos(Vector3 pos)
    {
        rb.position = pos;
    }

    public void SetCanvas(bool state)
    {
        canvas.enabled = state;
    }

    public void Interact()
    {
        switch (gameType)
        {
            case GameType.Normal:
                break;
            case GameType.Dream:
                if (!interactableItemDetector.interactable || !CanIteract) return;
                InteractableItem item = interactableItemDetector.GetItem();
                if (!item.canInteract) return;
                switch (item.itemName)
                {
                    case ("Car"):
                        Car car = (Car)item;
                        stateMachine.SetDrivingParameter(car);
                        stateMachine.SwitchState(PlayerStateType.Driving);
                        car.Interact();
                        break;
                    case ("PullRod"):
                        PullRod pullRod = (PullRod)item;
                        pullRod.Interact();
                        break;
                }
                break;
            case GameType.Dig:
                stateMachine.SwitchState(PlayerStateType.Dig);
                break;
        }
    }

}
