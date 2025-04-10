using UnityEngine.InputSystem;

public class PlayerRunState : GroundStateMachine
{
    public PlayerRunState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        PlayerStateReusableData.movementSpeedModifier = Player.PlayerData.playerGroundedData.playerRunData.speedModifier;
    }
    
    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);
        StateMachine.ChangeState(EPlayerStateType.Walk);
    }
}