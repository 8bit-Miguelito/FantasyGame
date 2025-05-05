using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class TestPlayerScript : MonoBehaviour
{

    public float movementSpeed = 2.0f;
    public float rotationOffset = 1.0f;
    public float runMultiplier = 3.0f;

    PlayerInput playerInput;

    Vector2 movementInput;
    Vector2 cameraDirection;
    Vector3 currentMovement;
    Vector3 RunVelocity;
    bool isMovement;
    bool isRunPressed;

    float gravity = -9.8f;
    float groundedGravity = -0.05f;

    CharacterController characterController;
    Animator animator;
    CameraFollow cameraFollow;

    bool isJumpPressed = false;
    bool canJumpAgain = true;
    float initialJumpVelocity;
    float maxJumpHeight = 10.0f;
    float MaxJumpTime = 0.5f;
    bool isJumping = false;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();

        playerInput.CharacterControls.Move.started += OnMovementInput;
        playerInput.CharacterControls.Move.canceled += OnMovementInput;
        playerInput.CharacterControls.Move.performed += OnMovementInput;

        playerInput.CharacterControls.Run.started += OnRun;
        playerInput.CharacterControls.Run.canceled += OnRun;

        playerInput.CharacterControls.Jump.started += OnJump;
        playerInput.CharacterControls.Jump.canceled += OnJump;

        SetJumpVariables();

    }

    void SetJumpVariables() 
    {
        float timeToApex = MaxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void Jump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed && canJumpAgain) {
            isJumping = true;
            canJumpAgain = false;
            currentMovement.y = initialJumpVelocity * 0.5f;
            RunVelocity.y = initialJumpVelocity * 0.5f;
        }
    }
    

    void OnJump(InputAction.CallbackContext context) 
    {
        isJumpPressed = context.ReadValueAsButton();

        if (!isJumpPressed) {
            canJumpAgain = true;
        }
    }

    void OnMovementInput (InputAction.CallbackContext context) 
    {
        movementInput = context.ReadValue<Vector2>();
        currentMovement.x = movementInput.x  * movementSpeed;
        currentMovement.z = movementInput.y * movementSpeed;
        RunVelocity.x = movementInput.x * runMultiplier;
        RunVelocity.z = movementInput.y * runMultiplier;
        isMovement = movementInput.x != 0 || movementInput.y != 0;
    }

    void OnRun(InputAction.CallbackContext context) 
    {
        isRunPressed = context.ReadValueAsButton();
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        HandleAnimation();

        if (isRunPressed) {
            characterController.Move(RunVelocity * Time.deltaTime);
        } else {
            characterController.Move(currentMovement * Time.deltaTime);
        }

        HandleGravity();
        Jump();

    }

    void HandleRotation() {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currRotation = transform.rotation;
        if (positionToLookAt != Vector3.zero) {
            Quaternion desiredRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currRotation, desiredRotation, rotationOffset * Time.deltaTime);
        }
    }

    void HandleGravity() {
        if (!characterController.isGrounded) {
            currentMovement.y += gravity * Time.deltaTime;
            RunVelocity.y += gravity * Time.deltaTime;
        } else {
            isJumping = false;
            currentMovement.y = groundedGravity;
            RunVelocity.y = groundedGravity;
        }
    }

    void HandleAnimation() 
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");

        if (isMovement && !isWalking) {
            animator.SetBool("isWalking", true);
        }

        if (!isMovement && isWalking) {
            animator.SetBool("isWalking", false);
        }

        if ((isMovement && isRunPressed) && !isRunning) {
            animator.SetBool("isRunning", true);
        }

        if ((!isMovement || !isRunPressed) && isRunning) {
            animator.SetBool("isRunning", false);
        }

    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();

    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    } 
}
