public class PlayerIdleState : GroundStateMachine
{

    #region IState Methods

    public PlayerIdleState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        PlayerStateReusableData.movementSpeedModifier = 0f;
        ResetVelocity();
    }
    
    #endregion
    
    protected override void HandleStopMovementInput(bool previousStateIsStopping, bool stop)
    {
        base.HandleStopMovementInput(previousStateIsStopping, stop);
        if (!stop && previousStateIsStopping)
        {
            StateMachine.ChangeState(PlayerStateReusableData.shouldWalk ? EPlayerStateType.Walk : EPlayerStateType.Run);
        }
    }
}