using System;
using System.Collections;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Player.State;
using WhereIsMyWife.Player.StateMachine;

public class PlayerDashState : PlayerState, IDashState, IDashStateEvents
{
    public PlayerDashState() : base(PlayerStateMachine.PlayerState.Dash) { }
    
    public Action<float> Dash { get; set; }
    public Action<float> GravityScale { get; set; }
    public Action<float> FallSpeedCap { get ; set; }

    private float _timer;
    
    protected override void SubscribeToObservables()
    {
        
    }
    
    protected override void UnsubscribeToObservables()
    {
        
    }

    public override void EnterState()
    {
        base.EnterState();
        _timer = 0;
        GravityScale?.Invoke(0f);
        FallSpeedCap?.Invoke(0f);
        Dash?.Invoke(_playerStateIndicator.DashSpeed);
    }

    public override void FixedUpdateState()
    {
        _timer += Time.fixedDeltaTime;

        if (_timer >= _properties.Dash.Duration)
        {
            NextState = PlayerStateMachine.PlayerState.Movement;
        }
    }
}
