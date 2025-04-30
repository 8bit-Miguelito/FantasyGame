using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 5f;
    public float rotationSpeed = 500f;
    public float jumpForce = 7f;
    public float MaxJumpTime = 0.5f;

    public float groundCheckRadius = 0.2f;
    public Vector3 groundCheckOffset;
    public LayerMask groundLayer;

    float gravityAcceleration;
    float initialJumpVelocity;
    float timeToApex;
    float maxJumpHeight;
    

    Quaternion targetRotation;
    CameraFollow cameraCont;

    Animator animator;
    CharacterController characterController;

    bool isJumping = false;

    private void Awake()
    {
        cameraCont = Camera.main.GetComponent<CameraFollow>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    } 
    // Update is called once per frame
    private void Update()
    {   
        float h = Input.GetAxis("Horizontal"); //Movement input from A/D
        float v = Input.GetAxis("Vertical"); //Movement input from W/S
        // Check if moving and how much movement. Clamp between 0-1 for Blend Tree in animation
        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Math.Abs(v));
        /*
        store moveInput in normalized vector (prevents faster movement during diagonal movement)
        i.e maginute will remain 1 in all directions. 
        moveDir rotates the vector in 3D plane and stores that direction.
        */
        var moveInput = (new Vector3(h,0,v)).normalized;  
        var moveDir = cameraCont.PlanarRotation * moveInput;
    
        //calculate velocity and velocity.y
        var velocity = moveDir * MovementSpeed;
        velocity.y = VerticalVelocity();

        if (Input.GetKey(KeyCode.LeftShift)) {
            characterController.Move(velocity * 2 * Time.deltaTime);
            animator.SetFloat("MovementSpeed", moveAmount / 1, 0.5f, Time.deltaTime);
        } else {
            characterController.Move(velocity * Time.deltaTime);
            animator.SetFloat("MovementSpeed", moveAmount / 2, 0.5f, Time.deltaTime);
        }
        
        if (moveAmount > 0) {
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private float VerticalVelocity() {
        if (GroundCheck()) {
            gravityAcceleration = 0.5f;
        } else {
            gravityAcceleration += Physics.gravity.y * Time.deltaTime;
        }

        return gravityAcceleration; 
    }


    bool GroundCheck() {
        if (Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer)) {
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
}

