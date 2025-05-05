using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 5f;
    public float rotationSpeed = 500f;
    public float jumpForce = 1.0f;
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
        float moveAmount = 0f;
        Vector3 moveDir = Vector3.zero;

        CalculateMove(ref moveAmount, ref moveDir);
    
        //calculate velocity and velocity.y
        var velocity = CalculateVelocity(in moveDir);

        if (Input.GetKey(KeyCode.LeftShift)) {
            velocity *= 2;
            animator.SetFloat("MovementSpeed", moveAmount / 1, 0.5f, Time.deltaTime);
        } else {
            animator.SetFloat("MovementSpeed", moveAmount / 2, 0.5f, Time.deltaTime);
        }

        characterController.Move(velocity * Time.deltaTime);
        
        if (moveAmount > 0) {
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void CalculateMove(ref float moveAmount, ref Vector3 moveDir) {
        float h = Input.GetAxis("Horizontal"); //Movement input from A/D
        float v = Input.GetAxis("Vertical"); //Movement input from W/S
        // Check if moving and how much movement. Clamp between 0-1 for Blend Tree in animation
        moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Math.Abs(v));

        /*
        store moveInput in normalized vector (prevents faster movement during diagonal movement)
        i.e maginute will remain 1 in all directions. 
        moveDir rotates the vector in 3D plane and stores that direction.
        */
        var moveInput = (new Vector3(h,0,v)).normalized;  
        moveDir = cameraCont.PlanarRotation * moveInput;
    }

    //calculate velocity and velocity.y
    //return velocity
    private Vector3 CalculateVelocity(in Vector3 moveDir) {
        Vector3 velocity = moveDir * MovementSpeed;
        velocity.y = VerticalVelocity();

        return velocity;
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

