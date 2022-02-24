using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputHandler;

[RequireComponent(typeof(Movement), typeof(PlayerCamera), typeof(PlayerHealth))]
public class PlayerController : MonoBehaviour
{
    PlayerCamera playerCamera = null;
    Movement movement = null;
    [HideInInspector]
    public PlayerHealth health = null;

    PlayerInput controls;
    PlayerInput.MovementActions inputActions;

    [SerializeField] LayerMask ignoreLayers;

    public float interactRadius = 10;
    public bool hiding = false;
    public HideObject hidingInside = null;

    CharacterController characterController;


    void Awake()
    {
        playerCamera = GetComponent<PlayerCamera>();
        movement = GetComponent<Movement>();
        health = GetComponent<PlayerHealth>();
        characterController = GetComponent<CharacterController>();

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

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Hide();
        }
    }

    public void Hide()
    {
        if (!hiding) {
            RaycastHit hit;
            Physics.Raycast(transform.position + new Vector3(0, characterController.height, 0), playerCamera.playerCamera.transform.forward, out hit, interactRadius, ignoreLayers);

            if (hit.collider != null) {
                HideObject _object = hit.collider.gameObject.GetComponent<HideObject>();
                if (_object != null) {
                    hiding = true;
                    transform.position = _object.cameraPos - new Vector3(0, characterController.height, 0) * 2;
                    transform.eulerAngles = new Vector3(0, _object.transform.eulerAngles.y, 0);
                    hidingInside = _object;
                    _object.hidingInside = true;
                    playerCamera.Hide();
                    movement.Hide();
                }
            }
        }
        else {
            movement.Hide();
            playerCamera.Hide();
            movement.controller.Move(transform.forward * 2);
            hiding = false;
            hidingInside.hidingInside = false;
            hidingInside = null;
        }
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
