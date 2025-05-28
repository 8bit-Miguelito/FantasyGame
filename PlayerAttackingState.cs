using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAttackingState : PlayerCombatState
{

    private float previousFrameTime;

    private Attack attack;

    private bool AttackPressed;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        base.Enter();
        AttackPressed = false;
        stateMachine.InputReader.AttackEvent += OnAttack;
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        float normalizedTime = GetNormalizedTime();

        if (normalizedTime < 1)
        {
            if (AttackPressed)
            {
                Debug.Log("Try Combo Attack");
                TryComboAttack(normalizedTime);
            }
        }
            else
            {
                stateMachine.SwitchState(stateMachine.targetingState);
            }
        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
        Debug.Log("Exiting");
        stateMachine.InputReader.AttackEvent -= OnAttack;
    }

    private float GetNormalizedTime()
    {
        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }

        return 0f;
    }

    private void OnAttack()
    {
        if (!AttackPressed) AttackPressed = true;
        Debug.Log("Attack Pressed = " + AttackPressed);
    }

    private void TryComboAttack(float normalizedTime)
    {
        if (attack.CombatStateIndex == -1) return;

        if (normalizedTime < attack.CombatAttackTime) return;

        Debug.Log("New Attack will be executed");
        stateMachine.currentAttack++;
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, stateMachine.currentAttack));
    }
}
