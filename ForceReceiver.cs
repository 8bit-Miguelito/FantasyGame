using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForceReceiver : MonoBehaviour
{

    [SerializeField] private CharacterController controller;
    public float verticalVelocity;
    private float gravity = -9.81f;

    public Vector3 Movement => Vector3.up * verticalVelocity;

    public void Update()
    {
        if (controller.isGrounded && verticalVelocity < 0) {
            verticalVelocity = gravity;
        } else {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }
}
