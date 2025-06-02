using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCombatState : PlayerStateBase
{

    protected Vector3 Movement;
    protected static int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    protected static int TargetingForwardHash = Animator.StringToHash("ForwardMovement");
    protected static int TargetingRightHash = Animator.StringToHash("RightMovement");

    public PlayerCombatState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, 0.2f);
    }

    public override void Exit()
    {
        
    }
    
}
