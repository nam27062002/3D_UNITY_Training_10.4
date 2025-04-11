using Unity.VisualScripting;

public abstract class PlayerStoppingState : PlayerGroundState
{
    protected PlayerStopData _stopData;
    protected PlayerStoppingState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
        _stopData = Player.PlayerData.playerGroundedData.playerStopData;
    }

    public override void Enter()
    {
        base.Enter();
        ReusableData.MovementSpeedModifier = 0f;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        RotateTowardsTargetRotation();
        if (!IsMovingHorizontally())
        {
            return;
        }
        DecelerateHorizontally();
    }

    public override void OnAnimationTransitionEvent()
    {
        StateMachine.ChangeState(EPlayerStateType.Idle);
    }
    
    protected override void HandleStopMovementInput(bool stop)
    {
        base.HandleStopMovementInput(stop);
        if (!stop)
        {
            OnMove();
        }
    }
} 