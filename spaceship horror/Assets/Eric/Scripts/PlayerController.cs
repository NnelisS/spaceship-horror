using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputHandler;

[RequireComponent(typeof(Movement), typeof(PlayerCamera), typeof(PlayerHealth))]
public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public PlayerCamera playerCamera = null;
    [HideInInspector]
    public Movement movement = null;
    [HideInInspector]
    public PlayerHealth health = null;

    PlayerInput controls;
    PlayerInput.MovementActions inputActions;

    public bool hiding = false;


    void Awake()
    {
        playerCamera = GetComponent<PlayerCamera>();
        movement = GetComponent<Movement>();
        health = GetComponent<PlayerHealth>();

        #region Bind Inputs

        controls = new PlayerInput();
        inputActions = controls.Movement;
        
        inputActions.Jump.performed += ctx => movement.Jump();
        inputActions.Sprint.performed += ctx => movement.Sprint(true);
        inputActions.Sprint.canceled += ctx => movement.Sprint(false);

        #region Basic Movement Inputs
        inputActions.Look.performed += ctx => playerCamera.UpdateCamera(ctx.ReadValue<Vector2>());

        inputActions.HorizontalMovement.performed += ctx => movement.UpdateHorizontal(ctx.ReadValue<float>());
        inputActions.VerticalMovement.performed += ctx => movement.UpdateVertical(ctx.ReadValue<float>());

        inputActions.HorizontalMovement.canceled += ctx => movement.UpdateHorizontal(ctx.ReadValue<float>());
        inputActions.VerticalMovement.canceled += ctx => movement.UpdateVertical(ctx.ReadValue<float>());
        #endregion

        #endregion
    }


    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDestroy()
    {
        controls.Disable();
    }

}
