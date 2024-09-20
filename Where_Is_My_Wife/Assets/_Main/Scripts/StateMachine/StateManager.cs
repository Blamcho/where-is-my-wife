using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
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
        EState nextStateKey = CurrentState.NextState;

        if (!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if (!IsTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
    }

    private void FixedUpdate()
    {
        EState nextStateKey = CurrentState.NextState;

        if (!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.FixedUpdateState();
        }
        else if (!IsTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
    }

    private void TransitionToState(EState nextStateKey)
    {
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[nextStateKey];
        CurrentState.EnterState();
        IsTransitioningState = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CurrentState.OnTriggerEnter2D(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CurrentState.OnTriggerStay2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CurrentState.OnTriggerExit2D(other);
    }
}
