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
    [SerializeField] public float turnTime = 0.1f;
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
        fsm.OnUpdate();
        Debug.Log("FSM Current State: " + fsm.activeState);
        //HandleJump();
        
    }

    private void FixedUpdate()
    {
        //this.MoveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
        InitialiseMoveDirection();
        HandleGravity();
        SetMoveDirectionRelativeToCamera();
        //HandleMovement();
        //HandleCrouch();
        fsm.OnFixedUpdate();
    }

    public float currentSpeed
    {
        get;
        private set;
    }

    #region the bin
    /*    private void HandleJump()
        {
    *//*        //Debug.Log($"Jump Input: {jumpInput}");
            if (inputManager.jumpInput && controller.isGrounded)
            {
                // transition from moving to jumping
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }*//*
        }

        public void HandleMovement()
        {

    *//*
            if (canMove && inputManager.moveInputDirection.magnitude > 0)
            {
                Debug.Log("Trying to do movement");
                Vector3 direction = new Vector3(inputManager.moveInputDirection.x, 0, inputManager.moveInputDirection.y).normalized;
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                MoveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
                controller.Move(MoveDirection * speed * Time.deltaTime);
            }*//*
        }

        private void HandleCrouch()
        {
            *//*if (inputManager.crouchInput)
            {
                transform.localScale = new Vector3(transform.localScale.x, originalScale / 2, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, originalScale, transform.localScale.z);
            }*//*
        }*/
    #endregion

    public void InitialiseMoveDirection()
    {
        MoveDirection = Vector3.forward * inputManager.moveInputDirection.y + Vector3.right * inputManager.moveInputDirection.x;
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
        Debug.Log($"Controller is grounded bool: {controller.isGrounded}");
    }
}
