using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class StateMachine : MonoBehaviour
{

    private State currentState;

    public void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.Tick(Time.deltaTime);
        }
        else
        {
            Debug.Log("Null reference");
        }
    }

}