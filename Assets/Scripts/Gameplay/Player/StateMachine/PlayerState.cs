using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState : IState
{
    protected readonly Player Player;
    protected readonly PlayerStateMachine StateMachine;

    protected PlayerStateReusableData ReusableData => StateMachine.PlayerStateReusableData;
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
        SetBaseRotationData();
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

    public virtual void OnAnimationEnterEvent()
    {
    }

    public virtual void OnAnimationExitEvent()
    {
    }

    public virtual void OnAnimationTransitionEvent()
    {
    }

    #endregion

    #region Main Methods

    private void ReadMovementInput()
    {
        ReusableData.MovementInput = Player.PlayerInput.PlayerAction.Move.ReadValue<Vector2>();
        OnStopMovementInput?.Invoke(ReusableData.MovementInput == Vector2.zero);
    }

    private void Move()
    {
        if (ReusableData.MovementInput != Vector2.zero && ReusableData.MovementSpeedModifier != 0f)
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
        return new Vector3(ReusableData.MovementInput.x, 0f, ReusableData.MovementInput.y);
    }

    protected float GetMovementSpeed()
    {
        return Player.PlayerData.playerGroundedData.baseSpeed * ReusableData.MovementSpeedModifier * ReusableData.MovementOnSlopesSpeedModifier;
    }

    protected Vector3 GetPlayerHorizontalVelocity()
    {
        Vector3 playerHorizontalVelocity = Player.Rigidbody.linearVelocity;
        playerHorizontalVelocity.y = 0f;
        return playerHorizontalVelocity;
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
    
    protected void RotateTowardsTargetRotation()
    {
        float currentYAngle = Player.Rigidbody.rotation.eulerAngles.y;
        if (Mathf.Approximately(currentYAngle, ReusableData.CurrentTargetRotation.y))
        {
            return;
        }
        float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle,  ReusableData.CurrentTargetRotation.y, ref ReusableData.DampedTargetRotationCurrentVelocity.y, ReusableData.TimeToReachTargetRotation.y - ReusableData.DampedTargetRotationPassedTime.y);
        ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;
        Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);
        Player.Rigidbody.MoveRotation(targetRotation);
    }

    private void UpdateTargetRotationData(float targetAngle)
    {
        ReusableData.CurrentTargetRotation.y = targetAngle;
        ReusableData.DampedTargetRotationPassedTime.y = 0f;
    }

    protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
    {
        float directionAngle = GetDirectionAngle(direction);
        if (shouldConsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }
        if (!Mathf.Approximately(directionAngle, ReusableData.CurrentTargetRotation.y))
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

    protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
        Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.z);
        return playerHorizontalMovement.magnitude > minimumMagnitude;
    }

    protected void DecelerateHorizontally()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
        Player.Rigidbody.AddForce(-playerHorizontalVelocity * ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
    }

    protected virtual void AddInputActionsCallbacks()
    {
        Player.PlayerInput.PlayerAction.WalkToggle.started += OnWalkToggleStarted;
    }
    
    protected virtual void RemoveInputActionsCallbacks()
    {
        Player.PlayerInput.PlayerAction.WalkToggle.started -= OnWalkToggleStarted;
    }
    
    protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        ReusableData.ShouldWalk = !ReusableData.ShouldWalk;
    }
    
    protected void SetBaseRotationData()
    {
        ReusableData.RotationData = Player.PlayerData.playerGroundedData.playerRotationData;
        ReusableData.TimeToReachTargetRotation = ReusableData.RotationData.targetRotationReachTime;
    }
    #endregion
    
}