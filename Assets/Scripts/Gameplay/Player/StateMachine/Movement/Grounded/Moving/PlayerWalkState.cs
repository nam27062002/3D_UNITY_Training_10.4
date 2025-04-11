using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerMovingState
{
    protected override EPlayerStateType StopStateType => EPlayerStateType.LightStop;
    public PlayerWalkState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        ReusableData.MovementSpeedModifier = Player.PlayerData.playerGroundedData.playerWalkData.speedModifier;
    }
    
    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);
        StateMachine.ChangeState(EPlayerStateType.Run);
    }
}