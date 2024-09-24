using System;
using DG.Tweening;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Player.StateMachine;

namespace WhereIsMyWife.Player.State
{
    public class PlayerWallJumpState : PlayerState, IWallJumpState, IWallJumpStateEvents
    {
        public PlayerWallJumpState() : base(PlayerStateMachine.PlayerState.WallJump) { }
        
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
            _playerStateInput.DashStart += Dash;
        }

        protected override void UnsubscribeToObservables()
        {
            _playerStateInput.GravityScale -= InvokeGravityScale;
            _playerStateInput.FallSpeedCap -= InvokeFallSpeedCap;
            _playerStateInput.Land -= EndWallJump;
            _playerStateInput.WallHangStart -= WallHang;
            _playerStateInput.DashStart -= Dash;
        }

        public override void EnterState()
        {
            base.EnterState();
            
            _minTimeHasPassed = false;
            _directionMultiplier = _playerStateIndicator.IsLookingRight ? 1 : -1;
            StartJumpSpeedCurve();
        }

        public override void ExitState()
        {
            base.ExitState();
            _horizontalSpeedTween.Kill();
        }

        public override void UpdateState()
        {
            if (_playerStateIndicator.IsAccelerating && _minTimeHasPassed)
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
        
        private void Dash(float _)
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