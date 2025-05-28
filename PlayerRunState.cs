using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRunState : PlayerFreeLookState
{
    //Movement variables
    private float RunSpeed = 8f;

    //Animation variables
    private const float DampTime = 0.2f;
    private readonly int RunningBlendTreeHash = Animator.StringToHash("RunningBlendTree");
    private readonly int RunningSpeedHash = Animator.StringToHash("RunningSpeed");

    public PlayerRunState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        stateMachine.Animator.CrossFadeInFixedTime(RunningBlendTreeHash, 0.2f);
        stateMachine.InputReader.RunEvent += SprintToggle;
    }

    public override void Tick(float deltaTime)
    {
        Movement = CalculateMovement();

        Move(Movement * RunSpeed, deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero) {
            stateMachine.Animator.SetFloat(RunningSpeedHash, 0f, DampTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(RunningSpeedHash, 1f, DampTime, deltaTime);

        HandleRotation(Movement, deltaTime);
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.InputReader.RunEvent -= SprintToggle;
    }

    private void SprintToggle() 
    {
        stateMachine.SwitchState(stateMachine.walkState);
    }

    //Handles rotation of character to rotate in direction player is walking
    protected void HandleRotation(Vector3 LookAtDirection, float deltaTime) 
    {   
        Quaternion currentRotation = stateMachine.transform.rotation;
        Quaternion desiredRotation = Quaternion.LookRotation(LookAtDirection);
        stateMachine.transform.rotation = Quaternion.Slerp(currentRotation, desiredRotation, MovementOffset * deltaTime);
    }
}
