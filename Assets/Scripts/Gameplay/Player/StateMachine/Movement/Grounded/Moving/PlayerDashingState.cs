using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashingState : PlayerMovingState
{
    private readonly PlayerDashData _playerDashData;
    private float _startTime;
    private int _consecutiveDashesUsed;
    private bool _shouldKeepRotating;
    
    public PlayerDashingState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
        _playerDashData = Player.PlayerData.playerGroundedData.playerDashData;
    }

    public override void Enter()
    {
        base.Enter();   
        ReusableData.MovementSpeedModifier = _playerDashData.SpeedModifier;
        ReusableData.RotationData = _playerDashData.RotationData;
        AddForceOnTransitionFromStationaryState();
        _shouldKeepRotating = ReusableData.MovementInput != Vector2.zero;
        UpdateConsecutiveDashes();
        _startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        SetBaseRotationData();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!_shouldKeepRotating)
        {
            return;
        }
        RotateTowardsTargetRotation();
    }

    public override void OnAnimationTransitionEvent()
    {
        if (ReusableData.MovementInput == Vector2.zero)
        {
            StateMachine.ChangeState(EPlayerStateType.HardStop);
            return;
        }
        StateMachine.ChangeState(EPlayerStateType.Sprint);
    }

    private void AddForceOnTransitionFromStationaryState()
    {
        if (ReusableData.MovementInput != Vector2.zero)
        {
            return;
        }

        Vector3 characterRotationDirection = Player.transform.forward;
        characterRotationDirection.y = 0f;
        UpdateTargetRotation(characterRotationDirection, false);
        Player.Rigidbody.linearVelocity = characterRotationDirection * GetMovementSpeed();
    }

    private void UpdateConsecutiveDashes()
    {
        if (!IsConsecutive())
        {
            _consecutiveDashesUsed = 0;
        }
        _consecutiveDashesUsed++;
        if (_consecutiveDashesUsed == _playerDashData.ConsecutiveDashesLimitAmount)
        {
            _consecutiveDashesUsed = 0;
            Player.PlayerInput.DisableActionFor(Player.PlayerInput.InputPad.Player.Dash, _playerDashData.DashLimitReachedCooldown);
        }
    }

    private bool IsConsecutive()
    {
        return Time.time < _startTime + _playerDashData.TimeToBeConsideredConsecutive;
    }
    
    protected override void OnDashStarted(InputAction.CallbackContext context)
    {
    }

    protected override void HandleStopMovementInput(bool stop)
    {
        base.HandleStopMovementInput(stop);
        if (!stop)
        {
            _shouldKeepRotating = true;
        }
    }
}