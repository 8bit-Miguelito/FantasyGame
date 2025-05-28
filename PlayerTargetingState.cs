using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTargetingState : PlayerCombatState
{
    private float MovementSpeed = 5f;
    private float DampTime = 0.1f;
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    //Enter state
    //Subscribe to input events
    public override void Enter()
    {
        base.Enter();
        stateMachine.InputReader.TargetEvent += TargetToggle;
        stateMachine.InputReader.RunEvent += OnRun;
        stateMachine.InputReader.AttackEvent += OnAttack;
    }

    public override void Tick(float deltaTime)
    {
        Movement = CalculateMovement();

        Move(Movement * MovementSpeed, deltaTime);

        FaceTarget();

        UpdateAnimations(deltaTime);

        //Check if player or target walk out range
        if (!stateMachine.Targeter.targetInRange())
        {
            stateMachine.SwitchState(stateMachine.walkState);
        }
    }

    public override void Exit()
    {
        stateMachine.Targeter.CancelTarget();

        stateMachine.InputReader.TargetEvent -= TargetToggle;
        stateMachine.InputReader.RunEvent -= OnRun;
        stateMachine.InputReader.AttackEvent -= OnAttack;
    }

    private void FaceTarget()
    {
        Vector3 LookAtDirection = stateMachine.Targeter.currentTarget.GetTargetTransform().position - stateMachine.transform.position;
        LookAtDirection.y = 0f;
        stateMachine.transform.rotation = Quaternion.LookRotation(LookAtDirection);
    }

    private void UpdateAnimations(float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue.y == 0)
        {
            stateMachine.Animator.SetFloat(TargetingForwardHash, 0, DampTime, deltaTime);
        }
        else
        {
            stateMachine.Animator.SetFloat(TargetingForwardHash, stateMachine.InputReader.MovementValue.y, DampTime, deltaTime);
        }
        if (stateMachine.InputReader.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(TargetingRightHash, 0, DampTime, deltaTime);
        }
        else
        {
            stateMachine.Animator.SetFloat(TargetingRightHash, stateMachine.InputReader.MovementValue.x, DampTime, deltaTime);
        }
    }

    private void TargetToggle()
    {
        stateMachine.SwitchState(stateMachine.walkState);
    }

    private void OnRun()
    {
        stateMachine.SwitchState(stateMachine.runState);
    }

    private void OnAttack()
    {
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, stateMachine.currentAttack));
    }
}
