using System;
using UnityEngine;
using UnityEngine.UIElements;
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
        private bool _firstPositionIsSet = false;
        private bool _allowingVelocityTime = false;
        private Vector2 _initialPosition;
        private Vector2 _updatedPosition;
        private float _transitionTimer = 0f;

        protected override void SubscribeToObservables()
        {
            _playerStateInput.HookEnd += ExecuteHookEnd;
            _playerStateInput.WallHangStart += WallHangStart;
            _playerStateInput.Land += MovementStart;
        }

        protected override void UnsubscribeToObservables()
        {
            _playerStateInput.HookEnd -= ExecuteHookEnd;
            _playerStateInput.WallHangStart -= WallHangStart;
            _playerStateInput.Land -= MovementStart;
        }

        public override void EnterState()
        {
            base.EnterState();
            _isPerformingLaunch = false;
            _firstPositionIsSet = false;
            _allowingVelocityTime = false;
            _updatedPosition = Vector2.zero;
            _transitionTimer = 0f;
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
                _transitionTimer += Time.fixedDeltaTime;
                
                if (_firstPositionIsSet)
                {
                    _updatedPosition = Vector2.Lerp(_initialPosition, _playerStateIndicator.HookPosition, _transitionTimer);
                    _firstPositionIsSet = false;
                }
                else
                {
                    _updatedPosition = Vector2.Lerp(_updatedPosition, _playerStateIndicator.HookPosition, _transitionTimer);
                }

                SetPosition?.Invoke(_updatedPosition);

                if (_transitionTimer >= _properties.Hook.TimeToReachHookPosition)
                {
                    _isPerformingLaunch = false;
                    _transitionTimer = 0f;
                    SetVelocity?.Invoke(_playerStateIndicator.HookLaunchVelocity);
                    _allowingVelocityTime = true;
                }
            }
            if (_allowingVelocityTime)
            {
                _transitionTimer += Time.fixedDeltaTime;

                if (_transitionTimer >= _properties.Hook.TimeAllowedToPerformLaunch)
                {
                    _allowingVelocityTime = false;
                    _transitionTimer = 0f;
                    NextState = PlayerStateMachine.PlayerState.Movement;
                }
            }
        }

        private void ExecuteHookEnd(Vector2 _newInitialPosition)
        {
            if (_playerStateIndicator.IsInQTEWindow)
            {
                _initialPosition = _newInitialPosition;
                _firstPositionIsSet = true;
                _isPerformingLaunch = true;
                StopQTE?.Invoke();
            }
            else
            {
                NextState = PlayerStateMachine.PlayerState.Movement;
            }
        }

        private void WallHangStart()
        {
            _allowingVelocityTime = false;
            NextState = PlayerStateMachine.PlayerState.WallHang;
        }

        private void MovementStart()
        {
            _allowingVelocityTime = false;
            NextState = PlayerStateMachine.PlayerState.Movement;
        }
    }
}