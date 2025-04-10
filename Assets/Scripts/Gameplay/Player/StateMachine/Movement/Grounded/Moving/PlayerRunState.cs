using UnityEngine.InputSystem;

public class PlayerRunState : PlayerMovingState
{
    public PlayerRunState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        PlayerStateReusableData.MovementSpeedModifier = Player.PlayerData.playerGroundedData.playerRunData.speedModifier;
    }
    
    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);
        StateMachine.ChangeState(EPlayerStateType.Walk);
    }
}