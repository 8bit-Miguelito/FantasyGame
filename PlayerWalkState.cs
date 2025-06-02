using System.ComponentModel;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWalkState : PlayerFreeLookState
{
    private const float DampTime = 0.1f;
    private const float MovementSpeed = 3f;

    //Set animator hashes to prevent errors
    public readonly int MovementSpeedForwardHash = Animator.StringToHash("ForwardMovement");
    public readonly int MovementSpeedRightHash = Animator.StringToHash("RightMovement");
    public static int FreeLookBlendTreeHash = Animator.StringToHash("WalkBlendTree");

    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine) {}
    
    public override void Enter()
    {
        base.Enter();
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, 0.2f);
        stateMachine.InputReader.RunEvent += SprintToggle;
    }

    public override void Tick(float deltaTime)
    {
        Movement = CalculateMovement();

        Move(Movement * MovementSpeed, deltaTime);
        
        UpdateAnimations(deltaTime);

        HandleRotation(deltaTime);
    }

    public override void Exit()
    {  
        base.Exit();
        stateMachine.InputReader.RunEvent -= SprintToggle;
    }

    private void SprintToggle()
    {
        stateMachine.SwitchState(stateMachine.runState);
    }
    
    /*
    FIX ME: adjust values for smooth controller damping between animations 
    */
    private void UpdateAnimations(float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue.y == 0)
        {
            stateMachine.Animator.SetFloat(MovementSpeedForwardHash, 0, DampTime, deltaTime);
        }
        else
        {
            stateMachine.Animator.SetFloat(MovementSpeedForwardHash, stateMachine.InputReader.MovementValue.y, DampTime, deltaTime);
        }
        if (stateMachine.InputReader.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(MovementSpeedRightHash, 0, DampTime, deltaTime);
        }
        else
        {
            stateMachine.Animator.SetFloat(MovementSpeedRightHash, stateMachine.InputReader.MovementValue.x, DampTime, deltaTime);
        }
    }

    //Handles rotation of character to rotate in direction player is walking
    private void HandleRotation(float deltaTime) 
    {   
        Vector3 LookAtDirection = stateMachine.MainCamTransform.forward;
        LookAtDirection.y = 0f;
        
        Quaternion currentRotation = stateMachine.transform.rotation;
        Quaternion desiredRotation = Quaternion.LookRotation(LookAtDirection);
        stateMachine.transform.rotation = Quaternion.Slerp(currentRotation, desiredRotation, MovementOffset * deltaTime);
    }
}