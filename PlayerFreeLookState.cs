using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class PlayerFreeLookState : PlayerStateBase
{

    protected Vector3 Movement;

    public static float MovementOffset = 10f;

    //constructor
    public PlayerFreeLookState(PlayerStateMachine stateMachine)
        : base(stateMachine) { }


    public override void Enter()
    {
        //Subscribe to input events
        stateMachine.InputReader.TargetEvent += OnTargetToggle;
    }

    public override void Exit()
    {
        //Unsubscribe to inputs upon exit
        stateMachine.InputReader.TargetEvent -= OnTargetToggle;
    }

    private void OnTargetToggle()
    {
        //Check for nearby enemies
        Debug.Log("Checking for enemies");
        stateMachine.Targeter.ScanForEnemies();
        //If no enemies found, return
        if (stateMachine.Targeter.currentTarget == null) return;
        //Switch states if enemies found
        stateMachine.SwitchState(stateMachine.targetingState);
    }

}
