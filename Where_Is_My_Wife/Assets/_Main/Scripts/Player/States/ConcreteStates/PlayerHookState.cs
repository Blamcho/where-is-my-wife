using System;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Player.StateMachine;

namespace WhereIsMyWife.Player.State
{
    public class PlayerHookState : PlayerState, IHookState, IHookStateEvents
    {
        public PlayerHookState() : base(PlayerStateMachine.PlayerState.Hook) { }
        public Action<float> GravityScale { get; set; }
        public Action<float> FallSpeedCap { get; set; }
        public Action StartHook { get; set; }
        public Action<Vector2> ExecuteHook { get; set; }
        public Action<Vector2> HookQTEFailed { get; set; }

        protected override void SubscribeToObservables()
        {
            _playerStateInput.GravityScale += InvokeGravityScale;
            _playerStateInput.FallSpeedCap += InvokeFallSpeedCap;
            _playerStateInput.HookActivated += InvokeHookActivated;
            _playerStateInput.ExecuteHookLaunch += InvokeExecuteHook;
            _playerStateInput.HookEnd += InvokeHookEnd;
        }

        protected override void UnsubscribeToObservables()
        {
            _playerStateInput.GravityScale -= InvokeGravityScale;
            _playerStateInput.FallSpeedCap -= InvokeFallSpeedCap;
            _playerStateInput.HookActivated -= InvokeHookActivated;
            _playerStateInput.ExecuteHookLaunch -= InvokeExecuteHook;
            _playerStateInput.HookEnd -= InvokeHookEnd;
        }

        private void InvokeGravityScale(float gravityScale)
        {
            NextState = PlayerStateMachine.PlayerState.Movement;
            GravityScale?.Invoke(gravityScale);
        }

        private void InvokeFallSpeedCap(float fallSpeedCap)
        {
            NextState = PlayerStateMachine.PlayerState.Movement;
            FallSpeedCap?.Invoke(fallSpeedCap);
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
            HookQTEFailed?.Invoke(originalVelocity);
        }
    }
}