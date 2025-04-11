public abstract class PlayerMovingState : PlayerGroundState
{
    protected virtual EPlayerStateType StopStateType => EPlayerStateType.Idle;
    
    public PlayerMovingState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
    }
    
    protected override void HandleStopMovementInput(bool stop)
    {
        base.HandleStopMovementInput(stop);
        if (stop)
        {
            StateMachine.ChangeState(StopStateType);
        }
    }
}