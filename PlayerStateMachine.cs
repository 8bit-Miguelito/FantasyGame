using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMachine: StateMachine {

    [field: SerializeField] public InputReader InputReader { get; private set; } 
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField] public CharacterController Controller { get; private set; }

    //Access to animator
    [field: SerializeField] public Animator Animator { get; private set; }  

    //Ensures access to our Player prefab
    [field: SerializeField] public Transform Player { get; private set; }

    //Access to targeter
    [field: SerializeField] public Targeter Targeter { get; private set; }

    [field: SerializeField] public Attack[] Attacks { get; private set; }
    //Access main camera transform
    public Transform MainCamTransform { get; private set; }

    /* -----------FreeLook Substates-----------*/ 
    public PlayerWalkState walkState { get; private set; }
    public PlayerRunState runState { get; private set; }

    /* -----------Combat Substates-----------*/
    public PlayerTargetingState targetingState { get; private set; }

    public int currentAttack = 0;

    private void Awake()
    {
        //Initialize substates
        walkState = new PlayerWalkState(this);
        runState = new PlayerRunState(this);
        targetingState = new PlayerTargetingState(this);
        //Initialize
        MainCamTransform = Camera.main.transform;

        //Starting state
        SwitchState(walkState);
    }
}