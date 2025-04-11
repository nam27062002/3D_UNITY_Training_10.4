public class PlayerLightStoppingState : PlayerStoppingState
{
    public PlayerLightStoppingState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        ReusableData.MovementDecelerationForce = _stopData.LightDecelerationForce;
    }
}