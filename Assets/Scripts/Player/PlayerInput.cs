using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    private Vector2 move => playerInputActions.GamePlay.Move.ReadValue<Vector2>();

    public bool Jump => playerInputActions.GamePlay.Jump.WasPressedThisFrame();

    public bool Dash => playerInputActions.GamePlay.Dash.WasPressedThisFrame();

    public bool Fire => playerInputActions.GamePlay.Fire.WasPressedThisFrame();

    public bool IsMoving => move.x != 0;

    public float MoveX => move.x;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    public void Enable()
    {
        playerInputActions.Enable();

    }

    public void Disable()
    {
        playerInputActions.Disable();
    }

}
