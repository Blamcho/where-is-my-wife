using System;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Player.StateMachine;

namespace WhereIsMyWife.Player.State
{
    public class PlayerHookState : PlayerState, IHookState, IHookStateEvents
    {
        public PlayerHookState() : base(PlayerStateMachine.PlayerState.Hook) { }
        public Action StartHook { get; set; }
        public Action<Vector2> ExecuteHook { get; set; }
        public Action<Vector2> HookQTEEnd { get; set; }

        protected override void SubscribeToObservables()
        {
            _playerStateInput.HookActivated += InvokeHookActivated;
            _playerStateInput.ExecuteHookLaunch += InvokeExecuteHook;
            _playerStateInput.HookEnd += InvokeHookEnd;
        }

        protected override void UnsubscribeToObservables()
        {
            _playerStateInput.HookActivated -= InvokeHookActivated;
            _playerStateInput.ExecuteHookLaunch -= InvokeExecuteHook;
            _playerStateInput.HookEnd -= InvokeHookEnd;
        }

        private void InvokeHookActivated()
        {
            StartHook?.Invoke();
        }

        private void InvokeExecuteHook(Vector2 hookVelocity)
        {
            ExecuteHook?.Invoke(hookVelocity);
        }

        private void InvokeHookEnd(Vector2 originalVelocity)
        {
            HookQTEEnd?.Invoke(originalVelocity);
            NextState = PlayerStateMachine.PlayerState.Movement;
        }
    }
}