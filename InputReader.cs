using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReader : MonoBehaviour, PlayerInput.ICharacterControlsActions
{

    PlayerInput playerInputs;

    public Vector2 MovementValue { get; private set; }

    public event Action JumpEvent; 
    public event Action RunEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action AttackEvent;
        // Start is called before the first frame update
    private void Start()
    {
        playerInputs = new PlayerInput();
        playerInputs.CharacterControls.SetCallbacks(this);
        playerInputs.CharacterControls.Enable();
    }

    private void OnDestroy()
    {
        playerInputs.CharacterControls.Disable();
    }

    public void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        JumpEvent?.Invoke();
    }

    public void OnLook(UnityEngine.InputSystem.InputAction.CallbackContext context) {}

    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnRun(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!context.performed) { return ;}
        RunEvent?.Invoke();
    }

    public void OnDodge(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        DodgeEvent?.Invoke();
    }

    public void OnTargetToggle(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        TargetEvent?.Invoke();

    }

    public void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        AttackEvent?.Invoke();
    }
}
