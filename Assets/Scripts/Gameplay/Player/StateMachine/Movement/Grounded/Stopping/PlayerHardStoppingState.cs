public class PlayerHardStoppingState : PlayerStoppingState
{
    public PlayerHardStoppingState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        ReusableData.MovementDecelerationForce = _stopData.HardDecelerationForce;
    }

    protected override void OnMove()
    {
        if (ReusableData.ShouldWalk)
        {
            return;
        }
        StateMachine.ChangeState(EPlayerStateType.Run);
    }
}