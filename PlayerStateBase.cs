using UnityEngine;

public abstract class PlayerStateBase : State
{

    protected PlayerStateMachine stateMachine;

    public PlayerStateBase(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 currMovement, float deltaTime)
    {
        stateMachine.Controller.Move((currMovement + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected Vector3 CalculateMovement()
    {
        Vector3 cameraForward = stateMachine.MainCamTransform.forward;
        Vector3 cameraRight = stateMachine.MainCamTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        return cameraForward * stateMachine.InputReader.MovementValue.y + cameraRight * stateMachine.InputReader.MovementValue.x;
    }
}

