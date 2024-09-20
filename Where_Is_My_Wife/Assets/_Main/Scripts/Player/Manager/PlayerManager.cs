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
        [SerializeField] private PlayerProperties _propertiesSO;
        public IPlayerProperties Properties => _propertiesSO.Properties;
        
        private IPlayerInputEvent _playerInputEvent;
        
        private IRunningMethods _runningMethods = new RunningMethods().Methods;
        private IJumpingMethods _jumpingMethods = new JumpingMethods().Methods;
        
        private PlayerDashState _playerDashState = new PlayerDashState();
        private PlayerMovementState _playerMovementState = new PlayerMovementState();
        private PlayerWallHangState _playerWallHangState = new PlayerWallHangState();
        private PlayerWallJumpState _playerWallJumpState = new PlayerWallJumpState();
        
        public IDashState DashState { get; private set; } 
        public IDashStateEvents DashStateEvents { get; private set; }
        public IMovementState MovementState { get; private set; }
        public IMovementStateEvents MovementStateEvents { get; private set; }
        public IWallHangState WallHangState { get; private set; }
        public IWallHangStateEvents WallHangStateEvents { get; private set; }
        public IWallJumpState WallJumpState { get; private set; }
        public IWallJumpStateEvents WallJumpStateEvents { get; private set; }
            
        
        // Timers
        private float _lastOnGroundTime = 0;
        private float _lastPressedJumpTime = 0;

        protected override void Awake()
        {
            base.Awake();
            
            DashState = _playerDashState.DashState;
            DashStateEvents = _playerDashState.DashStateEvents;
            MovementState = _playerMovementState.MovementState;
            MovementStateEvents = _playerMovementState.MovementStateEvents;
            WallHangState = _playerWallHangState.WallHangState;
            WallHangStateEvents = _playerWallHangState.WallHangStateEvents;
            WallJumpState = _playerWallJumpState.WallJumpState;
            WallJumpStateEvents = _playerWallJumpState.WallJumpStateEvents;
        }

        private void Start()
        {
            SubscribeToObservables();

            _playerInputEvent = InputEventManager.Instance.PlayerInputEvent;
            
            GravityScale?.Invoke(Properties.Gravity.Scale);
        }

        private void Update()
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
                _lastOnGroundTime = Properties.Jump.CoyoteTime;
                IsRunFalling = false;
            }

            if (!IsJumping && _lastOnGroundTime < Properties.Jump.CoyoteTime)
            {
                IsRunFalling = true;
            }
        }

        private Collider2D GetGroundCheckOverlapBox()
        {
            return Physics2D.OverlapBox(_controllerData.GroundCheckPosition, Properties.Check.GroundCheckSize, 0,
                Properties.Check.GroundLayer);
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
            return (Physics2D.OverlapBox(_controllerData.WallHangCheckUpPosition, Properties.Check.WallHangCheckSize,
                0, Properties.Check.GroundLayer)
                &&
                Physics2D.OverlapBox(_controllerData.WallHangCheckDownPosition, Properties.Check.WallHangCheckSize,
                    0, Properties.Check.GroundLayer)
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
                SetGravityScale(Properties.Gravity.Scale * Properties.Gravity.FastFallMultiplier);
                SetFallSpeedCap(Properties.Gravity.MaxFastFallSpeed);
            }
            
            // Scale gravity up if jump button released
            else if (IsJumpCut)
            {
                SetGravityScale(Properties.Gravity.Scale  * Properties.Gravity.JumpCutMultiplier);
                SetFallSpeedCap(Properties.Gravity.MaxBaseFallSpeed);
            }

            // Higher gravity when near jump height apex
            else if (IsInJumpHang())
            {
                SetGravityScale(Properties.Gravity.Scale  * Properties.Gravity.JumpHangMultiplier);
            }

            // Higher gravity if falling
            else if (IsJumpFalling)
            {
                SetGravityScale(Properties.Gravity.Scale  * Properties.Gravity.BaseFallMultiplier);
                SetFallSpeedCap(Properties.Gravity.MaxBaseFallSpeed);
            }

            // Reset gravity
            else
            {
                SetGravityScale(Properties.Gravity.Scale);
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
        public IPlayerStateIndicator PlayerStateIndicator => this;
        
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
                   && Mathf.Abs(_controllerData.RigidbodyVelocity.y) < Properties.Jump.HangTimeThreshold;
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
        public IPlayerStateInput PlayerStateInput => this;
        
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
            _lastPressedJumpTime = Properties.Jump.InputBufferTime;
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
            DashStart?.Invoke(dashDirection * Properties.Dash.Speed);
        }

        private void ExecuteLookDownEvent(bool isLookingDown)
        {
            IsLookingDown = isLookingDown;
        }
    }

    public partial class PlayerManager : IPlayerControllerEvent
    {
        public IPlayerControllerEvent PlayerControllerEvent => this;
        
        private IPlayerControllerData _controllerData;
        
        public void SetPlayerControllerData(IPlayerControllerData playerControllerData)
        {
            _controllerData = playerControllerData;
        }
    }
    
    public partial class PlayerManager : IRespawn
    {
        public IRespawn Respawn => this;
        
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
