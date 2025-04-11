public abstract class PlayerStoppingState : PlayerGroundState
{
    protected PlayerStoppingState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        ReusableData.MovementSpeedModifier = 0f;
    }
} 