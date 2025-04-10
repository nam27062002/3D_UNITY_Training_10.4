public abstract class GroundStateMachine : PlayerState
{
    protected GroundStateMachine(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
    }
    
    protected override void HandleStopMovementInput(bool previousStateIsStopping, bool stop)
    {
        base.HandleStopMovementInput(previousStateIsStopping, stop);
        if (stop && !previousStateIsStopping)
        {
            StateMachine.ChangeState(EPlayerStateType.Idle);
        }
    }
}