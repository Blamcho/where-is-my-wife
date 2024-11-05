using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, IBaseState<EState>> States = new Dictionary<EState, IBaseState<EState>>();
    protected IBaseState<EState> CurrentState;
    
    protected bool IsTransitioningState = false;
    
    protected virtual void Start()
    {
        CurrentState.EnterState();
    }

    private void Update()
    {
        if (IsTransitioningState) return;
        
        EState nextStateKey = CurrentState.NextState;
        
        if (!nextStateKey.Equals(CurrentState.StateKey))
        {
            TransitionToState(nextStateKey);
            return;
        }
        
        CurrentState.UpdateState();
    }

    private void FixedUpdate()
    {
        if (IsTransitioningState) return;

        CurrentState.FixedUpdateState();
    }

    protected void TransitionToState(EState nextStateKey)
    {
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[nextStateKey];
        CurrentState.EnterState();
        IsTransitioningState = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTransitioningState) return;
        
        CurrentState.OnTriggerEnter2D(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (IsTransitioningState) return;
        
        CurrentState.OnTriggerStay2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsTransitioningState) return;
        
        CurrentState.OnTriggerExit2D(other);
    }
}
