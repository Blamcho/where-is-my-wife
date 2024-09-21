using System;
using DG.Tweening;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Player.StateMachine;

namespace WhereIsMyWife.Player.State
{
    public class PlayerWallHangState : PlayerState, IWallHangState, IWallHangStateEvents
    {
        public PlayerWallHangState() : base(PlayerStateMachine.PlayerState.WallHang) { }
        
        public IWallHangStateEvents WallHangStateEvents => this;
        public IWallHangState WallHangState => this;
        
        public Action StartWallHang { get; set; }
        public Action<float> WallHangVelocity { get; set; }
        public Action<float> WallJumpStart { get; set; }
        public Action Turn { get; set; }
        
        private Tween _slideTween;
        
        private float _slideTweenSpeed = 0;
        private bool _isLookingRightAtStart;
        
        protected override void SubscribeToObservables()
        {
            _playerStateInput.DashStart += _ => Dash();
            _playerStateInput.JumpStart += Jump;
            _playerStateInput.Land += TurnAndCancelWallHang;
            _playerStateInput.WallHangEnd += TurnAndCancelWallHang;
        }

        protected override void UnsubscribeToObservables()
        {
            _playerStateInput.DashStart -= _ => Dash();
            _playerStateInput.JumpStart -= Jump;
            _playerStateInput.Land -= TurnAndCancelWallHang;
            _playerStateInput.WallHangEnd -= TurnAndCancelWallHang;
        }

        public override void EnterState()
        {
            base.EnterState();
            
            _isLookingRightAtStart = _playerStateIndicator.IsLookingRight;
            StartWallHang?.Invoke();
            
            StartSlideSpeedCurve();
        }

        private void StartSlideSpeedCurve()
        {
            _slideTweenSpeed = 0;
            
            _slideTween = DOTween.To(() => _slideTweenSpeed, x => _slideTweenSpeed = x, 
                    -_properties.Movement.WallSlideMaxVelocity, 
                    _properties.Movement.WallSlideTimeToMaxVelocity)
                .SetEase(Ease.InOutSine);
        }

        public override void ExitState()
        {
            base.ExitState();

            KillSlideSpeedCurve();
        }

        private void KillSlideSpeedCurve()
        {
            if (_slideTween != null && _slideTween.IsActive())
            {
                _slideTween.Kill();
            }
        }

        public override void UpdateState()
        {
            if (PlayerIsGoingOppositeDirectionOfWall())
            {
                TurnAndCancelWallHang();
            }
            
            WallHangVelocity?.Invoke(GetSlideSpeed());
        }

        private float GetSlideSpeed()
        {
            if (_playerStateIndicator.IsLookingDown)
            {
                return -_properties.Movement.WallSlideFastVelocity;
            }

            return _slideTweenSpeed;
        }
        
        private bool PlayerIsGoingOppositeDirectionOfWall()
        {
            if (!_playerStateIndicator.IsAccelerating)
            {
                return false;
            }
            
            return _isLookingRightAtStart != _playerStateIndicator.IsRunningRight;
        }

        private void TurnAndCancelWallHang()
        {
            Turn?.Invoke();
            CancelWallHang();
        }

        private void CancelWallHang()
        {
            NextState = PlayerStateMachine.PlayerState.Movement;
        }

        private void Dash()
        {
            Turn?.Invoke();
            NextState = PlayerStateMachine.PlayerState.Dash;
        }

        private void Jump(float jumpForce)
        {
            Turn?.Invoke();
            WallJumpStart?.Invoke(jumpForce / 1.5f);
            NextState = PlayerStateMachine.PlayerState.WallJump;
        }
    }
}