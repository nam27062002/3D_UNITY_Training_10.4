public class PlayerMediumStoppingState : PlayerStoppingState
{
    public PlayerMediumStoppingState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        ReusableData.MovementDecelerationForce = _stopData.MediumDecelerationForce;
    }
}