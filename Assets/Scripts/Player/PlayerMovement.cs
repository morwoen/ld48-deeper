using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    public PlayerInput input;
    public CharacterController controller;
    public Transform cameraTransform;

    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float turnSpeed = 0.1f;
    public float turnTime = 0.1f;

    private Vector3 velocity;
    float turnSmoothVelocity;

    Vector2 moveInputDirection;
    bool moveInput;
    bool runInput;
    bool jumpInput;
    bool crouchInput;

    float originalScale;

    void Awake() {
        input = new PlayerInput();
        input.PlayerControls.Locomotion.performed += ctx => {
            moveInputDirection = ctx.ReadValue<Vector2>();
            moveInputDirection = ctx.ReadValue<Vector2>();
            moveInput = moveInputDirection.x != 0 || moveInputDirection.y != 0;
        };
        input.PlayerControls.Run.performed += ctx => runInput = ctx.ReadValueAsButton();
        input.PlayerControls.Jump.performed += ctx => jumpInput = ctx.ReadValueAsButton();
        input.PlayerControls.Crouch.performed += ctx => crouchInput = ctx.ReadValueAsButton();

    }

    void Start()
    {
        //animator = GetComponent<Animator>();
        originalScale = transform.localScale.y;
    }

    void Update()
    {
        HandleGravity();
        HandleJump();
        HandleMovement();
    }

    private void HandleJump()
    {
        //Debug.Log($"Jump Input: {jumpInput}");
        if(jumpInput && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    void OnEnable()
    {
        input.PlayerControls.Enable();
    }

    void OnDisable()
    {
        input.PlayerControls.Disable();
    }


    void HandleMovement()
    {
        if (moveInput)
        {
            // animator.SetBool(isWalking, true);
            Vector3 direction = new Vector3(moveInputDirection.x, 0, moveInputDirection.y).normalized;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
            controller.Move(moveDir * speed * Time.deltaTime);
        }
        HandleCrouch();
    }

    private void HandleCrouch()
    {
        
        if (crouchInput)
        {
            transform.localScale = new Vector3(transform.localScale.x, originalScale/2, transform.localScale.z);
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
