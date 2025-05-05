public class PlayerStateFactory {

    PlayerStateMachine stateMachine;

    public PlayerStateFactory(PlayerStateMachine stateMachine) { this.stateMachine = stateMachine; }

    public PlayerBaseState Idle() { return new PlayerIdleState(); }

    public PlayerRunState Run() { return new PlayerRunState(); }
}