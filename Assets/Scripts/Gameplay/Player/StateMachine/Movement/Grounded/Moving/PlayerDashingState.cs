using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashingState : PlayerMovingState
{
    private readonly PlayerDashData _playerDashData;
    private float _startTime;
    private int _consecutiveDashesUsed;
    
    public PlayerDashingState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
        _playerDashData = Player.PlayerData.playerGroundedData.playerDashData;
    }

    public override void Enter()
    {
        base.Enter();   
        PlayerStateReusableData.MovementOnSlopesSpeedModifier = _playerDashData.SpeedModifier;
        AddForceOnTransitionFromStationaryState();
        UpdateConsecutiveDashes();
        _startTime = Time.time;
    }

    private void AddForceOnTransitionFromStationaryState()
    {
        if (PlayerStateReusableData.MovementInput != Vector2.zero)
        {
            return;
        }

        Vector3 characterRotationDirection = Player.transform.forward;
        characterRotationDirection.y = 0f;
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
}