public abstract class GroundStateMachine : PlayerState
{
    protected GroundStateMachine(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
    }
}