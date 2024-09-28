using System;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Player.StateMachine;

namespace WhereIsMyWife.Player.State
{
    public class PlayerHookState : PlayerState, IHookState, IHookStateEvents
    {
        public PlayerHookState() : base(PlayerStateMachine.PlayerState.Hook) { }
        public Action<Vector2> SetVelocity { get; set; }
        public Action<Vector2> SetPosition { get; set; }
        public Action<float> GravityScale { get; set; }
        public Action StartQTE { get; set; }
        public Action StopQTE { get; set; }

        private bool _isPerformingLaunch = false;
        private Vector2 _initialPosition;

        protected override void SubscribeToObservables()
        {
            _playerStateInput.HookEnd += ExecuteHookEnd;
        }

        protected override void UnsubscribeToObservables()
        {
            _playerStateInput.HookEnd -= ExecuteHookEnd;
        }

        public override void EnterState()
        {
            base.EnterState();

            GravityScale?.Invoke(0f);
            SetVelocity?.Invoke(Vector2.zero);
            StartQTE?.Invoke();
        }

        public override void ExitState()
        {
            base.ExitState();
            StopQTE?.Invoke();
        }

        public override void FixedUpdateState()
        {
            if (_isPerformingLaunch)
            {
                // Actualizas la posición con un timer, cuando el timer acaba, invocas el SetVelocity y cambias el next state
            }
        }

        private void ExecuteHookEnd()
        {
            if (_playerStateIndicator.IsInQTEWindow)
            {
                _isPerformingLaunch = true;
            }

            else
            {
                NextState = PlayerStateMachine.PlayerState.Movement;
            }
        }
    }
}