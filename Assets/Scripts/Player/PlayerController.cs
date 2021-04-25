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
    // public float turnSpeed = 0.1f;

    public Vector3 velocity;
    float turnSmoothVelocity;
    private StateMachine fsm;

    public bool canMove;

    float originalScale;

    void Start()
    {
        originalScale = transform.localScale.y; // TODO: Implement proper crouching

        inputManager = GetComponent<InputManager>();

        fsm = new StateMachine(this);
        fsm.AddState("Movement", new MovementState(this));

        fsm.AddState("Jumping", new JumpingState(this));

        fsm.AddTransition(new Transition(
            "Movement",
            "Jumping",
            (transition) => inputManager.jumpInput && controller.isGrounded
            ));

        fsm.AddTransition(new Transition(
            "Jumping",
            "Movement"));

        // This configures the entry point of the state machine
        fsm.SetStartState("Movement");
        // Initialises the state machine and must be called before OnLogic() is called
        fsm.OnEnter();
    }

    void Update()
    {
        fsm.OnUpdate();
        HandleJump();
        HandleMovement();
        HandleCrouch();
    }

    private void FixedUpdate()
    {
        HandleGravity();
        fsm.OnFixedUpdate();
    }

    public float currentSpeed
    {
        get;
        private set;
    }

    private void HandleJump()
    {
        //Debug.Log($"Jump Input: {jumpInput}");
        if (inputManager.jumpInput && controller.isGrounded)
        {
            // transition from moving to jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    public void HandleMovement()
    {
        if (canMove && inputManager.moveInputDirection.magnitude > 0)
        {
            Debug.Log("Trying to do movement");
            Vector3 direction = new Vector3(inputManager.moveInputDirection.x, 0, inputManager.moveInputDirection.y).normalized;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
            controller.Move(moveDir * speed * Time.deltaTime);
        }
    }

    private void HandleCrouch()
    {
        if (inputManager.crouchInput)
        {
            transform.localScale = new Vector3(transform.localScale.x, originalScale / 2, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, originalScale, transform.localScale.z);
        }
    }

    void HandleGravity()
    {
        float deltaTime = Time.deltaTime;
        velocity.y += gravity * deltaTime;
        controller.Move(velocity * deltaTime);
        if (controller.isGrounded)
        {
            velocity.y = -3f;
        }
    }
}
