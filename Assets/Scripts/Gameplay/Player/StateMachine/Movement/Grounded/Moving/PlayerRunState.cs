using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : PlayerMovingState
{
    private float _startTime;
    private readonly PlayerSprintData _playerSprintData;
    protected override EPlayerStateType StopStateType => EPlayerStateType.MediumStop;
    public PlayerRunState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
        _playerSprintData = player.PlayerData.playerGroundedData.playerSprintData;
    }
    
    public override void Enter()
    {
        base.Enter();
        ReusableData.MovementSpeedModifier = Player.PlayerData.playerGroundedData.playerRunData.speedModifier;
        _startTime = Time.time;
    }
    
    public override void Update()
    {
        base.Update();
        if (!ReusableData.ShouldWalk)
        {
            return;
        }

        if (Time.time < _startTime + _playerSprintData.RunToWalkTime)
        {
            return;
        }

        StopRunning();
    }
    
    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);
        StateMachine.ChangeState(EPlayerStateType.Walk);
    }

    private void StopRunning()
    {
        if (ReusableData.MovementInput == Vector2.zero)
        {
            StateMachine.ChangeState(EPlayerStateType.Idle);
            return;
        }
        StateMachine.ChangeState(EPlayerStateType.Walk);
    }
}