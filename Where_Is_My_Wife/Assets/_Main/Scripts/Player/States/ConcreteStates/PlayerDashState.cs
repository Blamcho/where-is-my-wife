using System;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Player.State;
using WhereIsMyWife.Player.StateMachine;

public class PlayerDashState : PlayerState, IDashState, IDashStateEvents
{
    public PlayerDashState() : base(PlayerStateMachine.PlayerState.Dash) { }
    
    public Action<float> Dash { get; set; }
    
    private float _timer;
    
    protected override void SubscribeToObservables()
    {
        _playerStateInput.DashStart += InvokeDash;
    }
    
    protected override void UnsubscribeToObservables()
    {
        _playerStateInput.DashStart -= InvokeDash;
    }

    private void InvokeDash(float dashVector)
    {
        Dash?.Invoke(dashVector);
    }

    public override void EnterState()
    {
        base.EnterState();
        _timer = 0;
    }

    public override void FixedUpdateState()
    {
        _timer += Time.fixedDeltaTime;

        if (_timer >= _properties.Dash.Duration)
        {
            NextState = PlayerStateMachine.PlayerState.Movement;
        }
    }

    public override void ExitState()
    {
    }
}
