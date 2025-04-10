using UnityEngine;

public abstract class StateMachine
{
    protected IState CurrentState { get; set; }

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        Debug.Log($"New State: {CurrentState.GetType().Name}");
        CurrentState?.Enter();
    }

    public void HandleInput()
    {
        CurrentState?.HandleInput();
    }

    public void Update() 
    {
        CurrentState?.Update();
    }

    public void PhysicsUpdate()
    {
        CurrentState?.PhysicsUpdate();
    }
}