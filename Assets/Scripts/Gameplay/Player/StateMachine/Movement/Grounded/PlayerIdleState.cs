public class PlayerIdleState : PlayerGroundState
{

    #region IState Methods

    public PlayerIdleState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        PlayerStateReusableData.MovementSpeedModifier = 0f;
        ResetVelocity();
    }
    
    #endregion
    
    protected override void HandleStopMovementInput(bool stop)
    {
        base.HandleStopMovementInput(stop);
        if (!stop)
        {
            StateMachine.ChangeState(PlayerStateReusableData.ShouldWalk ? EPlayerStateType.Walk : EPlayerStateType.Run);
        }
    }
}