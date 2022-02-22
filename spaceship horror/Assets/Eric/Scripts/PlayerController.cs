using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputHandler;

[RequireComponent(typeof(Movement), typeof(PlayerCamera))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerCamera playerCamera = null;
    [SerializeField] Movement movement = null;

    PlayerInput controls;
    PlayerInput.MovementActions inputActions;

    void Awake()
    {
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
