public class PlayerMovingState : PlayerGroundState
{
    public PlayerMovingState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
    }
    
    protected override void HandleStopMovementInput(bool stop)
    {
        base.HandleStopMovementInput(stop);
        if (stop)
        {
            StateMachine.ChangeState(EPlayerStateType.Idle);
        }
    }
}