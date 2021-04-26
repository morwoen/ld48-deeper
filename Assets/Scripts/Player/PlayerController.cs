using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FSM;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public InputManager inputManager;

    [SerializeField] public Animator animator;

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
        InitialiseMoveDirection();
        HandleGravity();
        fsm.OnUpdate();
        Debug.Log("FSM Current State: " + fsm.activeState);
        //Debug.Log($"isGrounded: {controller.isGrounded}");
        //Debug.Log($"jumpInput: {inputManager.jumpInput}");
        controller.Move(MoveDirection * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        fsm.OnFixedUpdate();
    }

    public float currentSpeed
    {
        get;
        private set;
    }

    public void InitialiseMoveDirection()
    {
        MoveDirection = Vector3.zero;
    }

    void HandleGravity()
    {
        if (controller.isGrounded && velocity.y < 0) {
          velocity.y = 0;
          animator.SetBool("IsGrounded", true);
        }
        if (!controller.isGrounded)
        {
            animator.SetBool("IsGrounded", false);
        }

        velocity.y += gravity * Time.deltaTime;

        MoveDirection.y = velocity.y;

        //Debug.Log($"Controller is grounded bool: {controller.isGrounded}");
  }
}
