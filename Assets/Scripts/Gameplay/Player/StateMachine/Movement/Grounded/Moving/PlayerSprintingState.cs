using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintingState : PlayerMovingState
{
    private PlayerSprintData _playerSprintData;
    private bool _keepSprinting;
    private float _startTime;
    
    public PlayerSprintingState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
        _playerSprintData = Player.PlayerData.playerGroundedData.playerSprintData;
    }

    public override void Enter()
    {
        base.Enter();
        ReusableData.MovementSpeedModifier = _playerSprintData.SpeedModifier;
        _startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        _keepSprinting = false;
    }

    
    public override void Update()
    {
        base.Update();
        if (_keepSprinting) return;
        if (Time.time < _startTime + _playerSprintData.SprintToRunTime)
        {
            return;
        }

        StopSprinting();
    }

    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();
        Player.PlayerInput.PlayerAction.Sprint.performed += OnSprintPerformed;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();
        Player.PlayerInput.PlayerAction.Sprint.performed -= OnSprintPerformed;
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        _keepSprinting = true;
    }

    private void StopSprinting()
    {
        if (ReusableData.MovementInput == Vector2.zero)
        {
            StateMachine.ChangeState(EPlayerStateType.Idle);
            return;
        }
        StateMachine.ChangeState(EPlayerStateType.Run);
    }
} 