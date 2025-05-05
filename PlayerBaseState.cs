using UnityEngine;

public abstract class PlayerBaseState {

    public virtual void EnterState(){}

    public virtual void UpdateState() {}

    public virtual void ExitState() {}

}