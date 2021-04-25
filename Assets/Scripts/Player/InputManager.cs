using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerInput input;

    public bool runInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool crouchInput { get; private set; }

    public Vector2 moveInputDirection
    {
        get;
        private set;
    }

    void Awake()
    {
        input = new PlayerInput();
        input.PlayerControls.Locomotion.performed += ctx => {
            moveInputDirection = ctx.ReadValue<Vector2>().normalized;
        };
        input.PlayerControls.Run.performed += ctx => runInput = ctx.ReadValueAsButton();    //TODO: Remove run input if we don't implement running
        input.PlayerControls.Jump.performed += ctx => jumpInput = ctx.ReadValueAsButton();
        input.PlayerControls.Crouch.performed += ctx => crouchInput = ctx.ReadValueAsButton();
    }

    void OnEnable()
    {
        input.PlayerControls.Enable();
    }

    void OnDisable()
    {
        input.PlayerControls.Disable();
    }
}
