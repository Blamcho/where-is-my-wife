using System;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Managers.Properties;
using WhereIsMyWife.Player.State;
using Zenject;

namespace WhereIsMyWife.Managers
{
    public partial class PlayerManager : Singleton<PlayerManager>
    {
        [Inject] private IPlayerProperties _properties;

        private IPlayerInputEvent _playerInputEvent;
        
        [Inject] private IRunningMethods _runningMethods;
        [Inject] private IJumpingMethods _jumpingMethods;
        
        // Timers
        private float _lastOnGroundTime = 0;
        private float _lastPressedJumpTime = 0;
        
        public void Start()
        {
            SubscribeToObservables();

            _playerInputEvent = InputEventManager.Instance.PlayerInputEvent;
            
            GravityScale?.Invoke(_properties.Gravity.Scale);
        }

         public void Update()
        {
            TickTimers();
            GroundCheck();
            WallCheck();
            JumpChecks();
            GravityShifts();
        }

        private void UpdateIsRunningRight(float runDirection)
        {
            if (runDirection > 0)
            {
                IsRunningRight = true;
            }
            
            else if (runDirection < 0)
            {
                IsRunningRight = false;
            }
        }
        private void TickTimers()
        {
            _lastOnGroundTime -= Time.deltaTime;
            _lastPressedJumpTime -= Time.deltaTime;
        }
        
        private void GroundCheck()
        {
            if (GetGroundCheckOverlapBox() && !IsJumping)
            {
                _lastOnGroundTime = _properties.Jump.CoyoteTime;
                IsRunFalling = false;
            }

            if (!IsJumping && _lastOnGroundTime < _properties.Jump.CoyoteTime)
            {
                IsRunFalling = true;
            }
        }

        private Collider2D GetGroundCheckOverlapBox()
        {
            return Physics2D.OverlapBox(_controllerData.GroundCheckPosition, _properties.Check.GroundCheckSize, 0,
                _properties.Check.GroundLayer);
        }

        private void WallCheck()
        {
            if (GetWallHangCheck())
            {
                if ((IsJumping || IsRunFalling))
                {
                    IsOnWallHang = true;
                    WallHangStart();
                }
            }

            else
            {
                IsOnWallHang = false;
                WallHangEnd?.Invoke();
            }
        }

        private bool GetWallHangCheck()
        {
            return (Physics2D.OverlapBox(_controllerData.WallHangCheckUpPosition, _properties.Check.WallHangCheckSize,
                0, _properties.Check.GroundLayer)
                &&
                Physics2D.OverlapBox(_controllerData.WallHangCheckDownPosition, _properties.Check.WallHangCheckSize,
                    0, _properties.Check.GroundLayer)
                &&
                IsAccelerating
                );
        }

        private void JumpChecks()
        {
            JumpingCheck();
            LandCheck();

            if (CanJump() && _lastPressedJumpTime > 0)
            {
                IsJumping = true;
                IsJumpCut = false;
                IsJumpFalling = false;
                IsRunFalling = false;
                
                Jump();
            }
        }

        private void JumpingCheck()
        {
            if (IsJumping && _controllerData.RigidbodyVelocity.y < 0)
            {
                IsJumping = false;
                IsJumpFalling = true;
            }
        }

        private void LandCheck()
        {
            if (_lastOnGroundTime > 0 && !IsJumping)
            {
                IsJumpCut = false;
                IsJumpFalling = false;
                Land?.Invoke();
            }
        }
        
        private void Jump()
        {
            ResetJumpTimers();
            
            JumpStart?.Invoke(_jumpingMethods.GetJumpForce(_controllerData.RigidbodyVelocity.y));
        }

        private void ResetJumpTimers()
        {
            _lastPressedJumpTime = 0;
            _lastOnGroundTime = 0;
        }
        
        private void GravityShifts()
        {
            // Make player fall faster if holding down 
            if (IsFastFalling())
            {
                SetGravityScale(_properties.Gravity.Scale * _properties.Gravity.FastFallMultiplier);
                SetFallSpeedCap(_properties.Gravity.MaxFastFallSpeed);
            }
            
            // Scale gravity up if jump button released
            else if (IsJumpCut)
            {
                SetGravityScale(_properties.Gravity.Scale  * _properties.Gravity.JumpCutMultiplier);
                SetFallSpeedCap(_properties.Gravity.MaxBaseFallSpeed);
            }

            // Higher gravity when near jump height apex
            else if (IsInJumpHang())
            {
                SetGravityScale(_properties.Gravity.Scale  * _properties.Gravity.JumpHangMultiplier);
            }

            // Higher gravity if falling
            else if (IsJumpFalling)
            {
                SetGravityScale(_properties.Gravity.Scale  * _properties.Gravity.BaseFallMultiplier);
                SetFallSpeedCap(_properties.Gravity.MaxBaseFallSpeed);
            }

            // Reset gravity
            else
            {
                SetGravityScale(_properties.Gravity.Scale);
            }
        }

        private void SetFallSpeedCap(float fallSpeedCap)
        {
            FallSpeedCap?.Invoke(fallSpeedCap);
        }

        private void SetGravityScale(float gravityScale)
        {
            GravityScale?.Invoke(gravityScale);
        }
        
        private void SubscribeToObservables()
        {
            _playerInputEvent.JumpStartAction += ExecuteJumpStartEvent;
            _playerInputEvent.JumpEndAction += ExecuteJumpEndEvent;
            _playerInputEvent.RunAction += ExecuteRunEvent;
            _playerInputEvent.DashAction += ExecuteDashStartEvent;
            _playerInputEvent.LookDownAction += ExecuteLookDownEvent;
        }
    }
    
    public partial class PlayerManager : IPlayerStateIndicator
    {
        public bool IsDead { get; private set; } = false;
        public bool IsAccelerating => _runningMethods.GetIsAccelerating();
        public bool IsRunningRight { get; private set; } = true;
        public bool IsLookingRight => _controllerData.HorizontalScale > 0;
        public bool IsLookingDown { get; private set; }
        public bool IsJumping { get; private set; } = false;
        public bool IsJumpCut { get; private set; } = false;
        public bool IsJumpFalling { get; private set; } = false;
        public bool IsOnWallHang { get; private set; } = false;
        public bool IsRunFalling { get; private set; } = false;

        public bool IsOnJumpInputBuffer()
        {
            return _lastPressedJumpTime >= 0;
        }

        public bool IsOnGround()
        {
            return _lastOnGroundTime >= 0;
        }

        public bool IsFastFalling()
        {
            return _controllerData.RigidbodyVelocity.y < 0 && IsLookingDown;
        }
        
        public bool IsInJumpHang()
        {
            return (IsJumping || IsJumpFalling) 
                   && Mathf.Abs(_controllerData.RigidbodyVelocity.y) < _properties.Jump.HangTimeThreshold;
        }

        public bool IsIdling()
        {
            return (Mathf.Abs(_controllerData.RigidbodyVelocity.x) < 0.1f 
                    && Mathf.Abs(_controllerData.RigidbodyVelocity.y) < 0.1f);
        }

        public bool CanJump()
        {
            return (_lastOnGroundTime > 0 && !IsJumping) || IsOnWallHang;
        }

        public bool CanJumpCut()
        {
            return IsJumping && _controllerData.RigidbodyVelocity.y > 0;
        }
    }
    
    public partial class PlayerManager : IPlayerStateInput
    {
        public Action<float> JumpStart { get; set; }
        public Action<float> Run { get; set; }
        public Action WallHangStart { get; set; }
        public Action WallHangEnd { get; set; }
        public Action<Vector2> DashStart { get; set; }
        public Action<float> GravityScale { get; set; }
        public Action<float> FallSpeedCap { get; set; }
        public Action Land { get; set; }

        private void ExecuteJumpStartEvent()
        {
            _lastPressedJumpTime = _properties.Jump.InputBufferTime;
        }

        private void ExecuteJumpEndEvent()
        {
            if (CanJumpCut())
            {
                IsJumpCut = true;
            }
        }
        
        private void ExecuteRunEvent(float runDirection)
        {
            UpdateIsRunningRight(runDirection);
            Run?.Invoke(_runningMethods.GetRunAcceleration(runDirection, _controllerData.RigidbodyVelocity.x));
        }

        private void ExecuteDashStartEvent(Vector2 dashDirection)
        {
            DashStart?.Invoke(dashDirection * _properties.Dash.Speed);
        }

        private void ExecuteLookDownEvent(bool isLookingDown)
        {
            IsLookingDown = isLookingDown;
        }
    }

    public partial class PlayerManager : IPlayerControllerEvent
    {
        private IPlayerControllerData _controllerData;
        
        public void SetPlayerControllerData(IPlayerControllerData playerControllerData)
        {
            _controllerData = playerControllerData;
        }
    }
    
    public partial class PlayerManager : IRespawn
    {
        private Vector3 _respawnPoint;
     
        public Action<Vector3> RespawnAction { get; set; }
        
        public void SetRespawnPoint(Vector3 respawnPoint)
        {
            _respawnPoint = respawnPoint;
        }

        public void TriggerRespawn()
        {
           RespawnAction?.Invoke(_respawnPoint);
        }

    }
}
