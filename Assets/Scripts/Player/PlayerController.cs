using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FSM;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;
    public InputManager inputManager;

    [SerializeField] public float speed = 6f;
    [SerializeField] public float gravity = -9.81f;
    [SerializeField] public float jumpHeight = 3f;
    [SerializeField] public float turnSpeed = 500f;

    [HideInInspector]
    public Vector3 velocity;
    public float turnSmoothVelocity;
    private StateMachine fsm;

    public bool canMove;
    public bool toggleCrouch;

    public float originalScale;

    public Vector3 MoveDirection;

    void Start()
    {
        originalScale = transform.localScale.y; // TODO: Implement proper crouching

        inputManager = GetComponent<InputManager>();

        fsm = new StateMachine(this);
        fsm.AddState("Movement", new MovementState(this));

        #region JUMPING
        fsm.AddState("Jumping", new JumpingState(this));
        fsm.AddTransition(new Transition(
            "Movement",
            "Jumping",
            (transition) => inputManager.jumpInput && controller.isGrounded
            ));
        fsm.AddTransition(new Transition(
            "Jumping",
            "Movement"));
        #endregion
        #region CROUCHING
        fsm.AddState("Crouching", new CrouchingState(this));
        fsm.AddTransition(new Transition(
            "Movement",
            "Crouching",
            (transition) => inputManager.crouchInput
            ));
        fsm.AddTransition(new Transition(
            "Crouching",
            "Movement"));
        #endregion
        // This configures the entry point of the state machine
        fsm.SetStartState("Movement");
        // Initialises the state machine and must be called before OnLogic() is called
        fsm.OnEnter();
    }

    void Update()
    {
        if (inputManager.jumpInput && controller.isGrounded) {
            Debug.Log("!!!Should be jumping!!!");
        }

        Debug.Log($"isGrounded: {controller.isGrounded}");
        Debug.Log($"jumpInput: {inputManager.jumpInput}");

        fsm.OnUpdate();
        //Debug.Log("FSM Current State: " + fsm.activeState);
        //HandleJump();
    }

    private void FixedUpdate()
    {
        InitialiseMoveDirection();
        HandleGravity();
        SetMoveDirectionRelativeToCamera();
        fsm.OnFixedUpdate();

    }

    public float currentSpeed
    {
        get;
        private set;
    }

    public void InitialiseMoveDirection()
    {
        MoveDirection = Vector3.forward * inputManager.movementInput.y + Vector3.right * inputManager.movementInput.x;
        MoveDirection.Normalize();
    }

    public void SetMoveDirectionRelativeToCamera()
    {
        Vector3 projectedCameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up);
        Quaternion rotationToCamera = Quaternion.LookRotation(projectedCameraForward, Vector3.up);

        MoveDirection = rotationToCamera * MoveDirection;
    }

    void HandleGravity()
    {
        float deltaTime = Time.deltaTime;
        velocity.y += gravity * deltaTime;
        controller.Move(velocity * deltaTime);
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -3f;
        }
        //Debug.Log($"Controller is grounded bool: {controller.isGrounded}");
    }
}
