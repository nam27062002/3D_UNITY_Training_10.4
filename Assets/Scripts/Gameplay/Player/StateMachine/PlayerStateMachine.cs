using System.Collections.Generic;

public class PlayerStateMachine : StateMachine
{
    private readonly Dictionary<EPlayerStateType, PlayerState> _playerStates;
    public readonly PlayerStateReusableData PlayerStateReusableData;

    public PlayerStateMachine(Player player)
    {
        PlayerStateReusableData = new PlayerStateReusableData();
        _playerStates = new Dictionary<EPlayerStateType, PlayerState>()
        {
            { EPlayerStateType.Idle, new PlayerIdleState(this, player) },
            { EPlayerStateType.Walk, new PlayerWalkState(this, player) },
            { EPlayerStateType.Run, new PlayerRunState(this, player) },
            { EPlayerStateType.Dash, new PlayerDashingState(this, player) },
            { EPlayerStateType.Sprint, new PlayerSprintingState(this, player) },
            { EPlayerStateType.LightStop, new PlayerLightStoppingState(this, player) },
            { EPlayerStateType.MediumStop, new PlayerMediumStoppingState(this, player) },
            { EPlayerStateType.HardStop, new PlayerHardStoppingState(this, player) },
        };
    }

    public void ChangeState(EPlayerStateType newState)
    {
        ChangeState(_playerStates[newState]);
    }
}