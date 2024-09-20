using System;
using DG.Tweening;
using UniRx;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Managers;
using WhereIsMyWife.Managers.Properties;
using WhereIsMyWife.Player.StateMachine;
using Zenject;

namespace WhereIsMyWife.Player.State
{
    public class PlayerWallJumpState : PlayerState, IWallJumpState, IWallJumpStateEvents
    {
        public PlayerWallJumpState() : base(PlayerStateMachine.PlayerState.WallJump) { }
        
        public IWallJumpStateEvents WallJumpStateEvents => this;
        public IWallJumpState WallJumpState => this;
        
        public Action<float> WallJumpVelocity { get; set; }
        public Action<float> GravityScale { get; set; }
        public Action<float> FallSpeedCap { get; set; }
        
        private Tween _horizontalSpeedTween;
        
        private float _horizontalSpeed = 0;
        private int _directionMultiplier = 1;

        private bool _minTimeHasPassed = false;
        
        protected override void SubscribeToObservables()
        {
            _playerStateInput.GravityScale += InvokeGravityScale;
            _playerStateInput.FallSpeedCap += InvokeFallSpeedCap;
            _playerStateInput.Land += EndWallJump;
            _playerStateInput.WallHangStart += WallHang;
        }

        protected override void UnsubscribeToObservables()
        {
            _playerStateInput.GravityScale -= InvokeGravityScale;
            _playerStateInput.FallSpeedCap -= InvokeFallSpeedCap;
            _playerStateInput.Land -= EndWallJump;
            _playerStateInput.WallHangStart -= WallHang;
        }

        public override void EnterState()
        {
            base.EnterState();
            
            _minTimeHasPassed = false;
            _directionMultiplier = _stateIndicator.IsLookingRight ? 1 : -1;
            StartJumpSpeedCurve();
        }

        public override void ExitState()
        {
            base.ExitState();
            _horizontalSpeedTween.Kill();
        }

        public override void UpdateState()
        {
            if (_stateIndicator.IsAccelerating && _minTimeHasPassed)
            {
                EndWallJump();
            }
        }

        public override void FixedUpdateState()
        {
            WallJumpVelocity?.Invoke(_horizontalSpeed);
        }

        private void StartJumpSpeedCurve()
        {
            _horizontalSpeed = _properties.WallJump.Speed * _directionMultiplier;

            _horizontalSpeedTween = DOTween.To(() => _horizontalSpeed, x => _horizontalSpeed = x, 
                    _properties.Movement.RunMaxSpeed * _directionMultiplier, 
                    _properties.WallJump.TimeToNormalSpeed)
                .SetEase(Ease.InOutSine)
                .OnComplete(StartDecelerationCurve);
        }
        
        private void StartDecelerationCurve()
        {
            _minTimeHasPassed = true;
            _horizontalSpeedTween = DOTween.To(() => _horizontalSpeed, x => _horizontalSpeed = x, 
                    0, 
                    _properties.WallJump.TimeToZeroSpeed)
                .SetEase(Ease.InSine)
                .OnComplete(StartDecelerationCurve);
        }
        
        private void EndWallJump()
        {
            NextState = PlayerStateMachine.PlayerState.Movement;
        }

        private void WallHang()
        {
            NextState = PlayerStateMachine.PlayerState.WallHang;
        }
        
        private void Dash()
        {
            NextState = PlayerStateMachine.PlayerState.Dash;
        }

        private void InvokeGravityScale(float gravityScale)
        {
            GravityScale?.Invoke(gravityScale);
        }

        private void InvokeFallSpeedCap(float fallSpeedCap)
        {
            FallSpeedCap?.Invoke(fallSpeedCap);
        }
    }
}