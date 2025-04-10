using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState : IState
{
    protected readonly Player Player;
    protected readonly PlayerStateMachine StateMachine;

    protected PlayerStateReusableData PlayerStateReusableData => StateMachine.PlayerStateReusableData;
    //delegate
    protected DelegateList<bool> OnStopMovementInput = DelegateList<bool>.CreateWithGlobalCache();
    
    protected PlayerState(PlayerStateMachine playerStateMachine, Player player)
    {
        Player = player;
        StateMachine = playerStateMachine;
        InitializeData();
    }

    private void InitializeData()
    {
        PlayerStateReusableData.TimeToReachTargetRotation = Player.PlayerData.playerGroundedData.playerRotationData.targetRotationReachTime;
    }

    #region IState Methods

    public virtual void Enter()
    {
        OnStopMovementInput += HandleStopMovementInput;
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        OnStopMovementInput -= HandleStopMovementInput;
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void Update()
    {
        
    }

    public virtual void PhysicsUpdate()
    {
        Move();
    }

    #endregion

    #region Main Methods

    private void ReadMovementInput()
    {
        PlayerStateReusableData.MovementInput = Player.PlayerInput.PlayerAction.Move.ReadValue<Vector2>();
        OnStopMovementInput?.Invoke(PlayerStateReusableData.MovementInput == Vector2.zero);
    }

    private void Move()
    {
        if (PlayerStateReusableData.MovementInput != Vector2.zero && PlayerStateReusableData.MovementSpeedModifier != 0f)
        {
            Vector3 movementDirection = GetMovementDirection();
            float targetRotationYAngle = Rotate(movementDirection);
            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
            float movementSpeed = GetMovementSpeed();
            Vector3 currentPlayerHorizontalMovement = GetPlayerHorizontalVelocity();
            Player.Rigidbody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalMovement, ForceMode.VelocityChange);
        } 
    }
    
    private float Rotate(Vector3 direction)
    {
        float directionAngle = UpdateTargetRotation(direction);
        RotateTowardsTargetRotation();
        return directionAngle;
    }
    
    #endregion
    
    #region Reusable Methods
    private Vector3 GetMovementDirection()
    {
        return new Vector3(PlayerStateReusableData.MovementInput.x, 0f, PlayerStateReusableData.MovementInput.y);
    }

    private float GetMovementSpeed()
    {
        return Player.PlayerData.playerGroundedData.baseSpeed * PlayerStateReusableData.MovementSpeedModifier * PlayerStateReusableData.MovementOnSlopesSpeedModifier;
    }

    private Vector3 GetPlayerHorizontalVelocity()
    {
        return Player.Rigidbody.linearVelocity;
    }

    protected Vector3 GetPlayerVerticalVelocity()
    {
        return new Vector3(0f, Player.Rigidbody.linearVelocity.y, 0f);
    }
    
    private float AddCameraRotationToAngle(float angle)
    {
        angle += Player.MainCameraTransform.eulerAngles.y;
        if (angle > 360f)
        {
            angle -= 360f;
        }
        return angle;
    }
    
    private float GetDirectionAngle(Vector3 direction)
    {
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        if (directionAngle < 0f)
        {
            directionAngle += 360f;
        }

        return directionAngle;
    }
    
    private void RotateTowardsTargetRotation()
    {
        float currentYAngle = Player.Rigidbody.rotation.eulerAngles.y;
        float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle,  PlayerStateReusableData.CurrentTargetRotation.y, ref PlayerStateReusableData.DampedTargetRotationCurrentVelocity.y, PlayerStateReusableData.TimeToReachTargetRotation.y - PlayerStateReusableData.DampedTargetRotationPassedTime.y);
        PlayerStateReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;
        Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);
        Player.Rigidbody.MoveRotation(targetRotation);
    }

    private void UpdateTargetRotationData(float targetAngle)
    {
        PlayerStateReusableData.CurrentTargetRotation.y = targetAngle;
        PlayerStateReusableData.DampedTargetRotationPassedTime.y = 0f;
    }

    private float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
    {
        float directionAngle = GetDirectionAngle(direction);
        if (shouldConsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }
        if (!Mathf.Approximately(directionAngle, PlayerStateReusableData.CurrentTargetRotation.y))
        {
            UpdateTargetRotationData(directionAngle);
        }

        return directionAngle;
    }
    
    private Vector3 GetTargetRotationDirection(float targetAngle)
    {
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    protected void ResetVelocity()
    {
        Player.Rigidbody.linearVelocity = Vector3.zero;
    }
    
    protected virtual void HandleStopMovementInput(bool stop)
    {

    }

    protected virtual void AddInputActionsCallbacks()
    {
        Player.PlayerInput.PlayerAction.Sprint.started += OnWalkToggleStarted;
    }
    
    protected virtual void RemoveInputActionsCallbacks()
    {
        Player.PlayerInput.PlayerAction.Sprint.started -= OnWalkToggleStarted;
    }
    
    protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        PlayerStateReusableData.ShouldWalk = !PlayerStateReusableData.ShouldWalk;
    }
    #endregion
    
}