using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Windows;
using Cinemachine;
public enum GameType { Normal, Dream, Dig }
public enum AgeType { Children, Young, Older }

public class PlayerController : MonoBehaviour
{
    #region 数值
    private float moveSpeed = 8;
    public float MoveSpeed
    {
        get
        {
            if (curAgeType == AgeType.Children) return moveSpeed * 0.75f;
            else if (curAgeType == AgeType.Young) return moveSpeed;
            else return moveSpeed * 0.5f;
        }
        private set => moveSpeed = value;
    }
    private float jumpSpeed = 20f;
    public float JumpSpeed
    {
        get
        {
            if (curAgeType == AgeType.Children) return jumpSpeed * 0.75f;
            else if (curAgeType == AgeType.Young) return jumpSpeed;
            else return 0;
        }
        private set => jumpSpeed = value;
    }

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
    public AgeType curAgeType = AgeType.Children;
    private Vector3 originScale = new Vector3(1, 1, 1);
    public Vector3 CurScale
    {
        get
        {
            if (curAgeType == AgeType.Children) return 0.4f * originScale;
            else if (curAgeType == AgeType.Young) return 0.8f * originScale;
            else return 0.6f * originScale;
        }
    }

    public bool Jump => playerInput.Jump;
    private bool canJump = false;
    public bool CanJump
    {
        get
        {
            if (curAgeType == AgeType.Older) return false;
            else return canJump;
        }
        private set => canJump = value;
    }

    private float jumpBuffer = 0;
    public float JumpBuffer { get => jumpBuffer; private set => jumpBuffer = value; }
    public bool HaveJumpBuffer
    {
        get => JumpBuffer > 0;
    }
    public bool right = true;
    public bool IsGround => groundDetector.IsGround;
    public bool Falling => !IsGround && rb.velocity.y < 0;
    public bool Rising => rb.velocity.y > 0;
    private float curDashCD = 0;
    public float CurDashCD { get => curDashCD; private set => curDashCD = value; }
    public bool CanDash
    {
        get => CurDashCD < 0;
    }
    private bool canIteract = true;
    public bool CanIteract { get => canIteract; set => canIteract = value; }
    public int Gold { get => gold; set => gold = value; }

    public GameType gameType;
    private int gold = 0;
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
    public List<GameObject> specialList = new List<GameObject>();
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
            transform.localScale = new Vector3(-CurScale.x, CurScale.y, CurScale.z);
            right = false;
        }
        else if (x > 0)
        {
            transform.localScale = CurScale;
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
                if (item.curState != ItemStateType.Interact) return;
                switch (item.itemName)
                {
                    case ("Car"):
                        if (curAgeType != AgeType.Older) break;
                        Car car = (Car)item;
                        stateMachine.SetDrivingParameter(car);
                        stateMachine.SwitchState(PlayerStateType.Driving);
                        car.Interact();
                        break;
                    case ("PullRod"):
                        PullRod pullRod = (PullRod)item;
                        pullRod.Interact();
                        break;
                    case ("Clock"):
                        Clock clock = (Clock)item;
                        AgeBackward();
                        clock.Interact();
                        break;
                    case ("FinishLine"):
                        FinishLine finishLine = (FinishLine)item;
                        if (Gold >= finishLine.goldLimit)
                            finishLine.Interact();
                        break;
                    case ("Gold"):
                        Gold gold = (Gold)item;
                        Gold++;
                        gold.Interact();
                        break;
                }
                break;
            case GameType.Dig:
                stateMachine.SwitchState(PlayerStateType.Dig);
                break;
        }
    }

    public void AgeForward()
    {
        if (curAgeType == AgeType.Older) GameManager.Instance.Remake();
        else curAgeType++;

        transform.localScale = CurScale;
        stateMachine.SwitchState(PlayerStateType.Idle);
    }

    public void AgeBackward()
    {
        if (curAgeType == AgeType.Children) curAgeType = AgeType.Children;
        else curAgeType--;

        transform.localScale = CurScale;
        stateMachine.SwitchState(PlayerStateType.Idle);
    }

    public void ChangeAge(AgeType ageType)
    {
        curAgeType = ageType;

        transform.localScale = CurScale;
        stateMachine.SwitchState(PlayerStateType.Idle);
    }

}
