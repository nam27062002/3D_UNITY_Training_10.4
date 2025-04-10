using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerMovingState
{
    public PlayerWalkState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        PlayerStateReusableData.MovementSpeedModifier = Player.PlayerData.playerGroundedData.playerWalkData.speedModifier;
    }
    
    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);
        StateMachine.ChangeState(EPlayerStateType.Run);
    }
}