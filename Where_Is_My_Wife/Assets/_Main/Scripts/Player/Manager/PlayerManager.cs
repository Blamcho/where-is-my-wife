using System;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Managers.Properties;
using WhereIsMyWife.Player.State;
using WhereIsMyWife.Player.StateMachine;

namespace WhereIsMyWife.Managers
{
    /// <summary>
    /// Receives the input made by the player and process it with customized properties and then raises events via <see cref="IPlayerStateInput"/> and gives information via <see cref="IPlayerStateIndicator"/>
    /// </summary>
    public partial class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private PlayerProperties _propertiesSO;
        [SerializeField] private PlayerStateMachine _playerStateMachine;

        public IPlayerProperties Properties => _propertiesSO.Properties;
        
        private IPlayerInputEvent _playerInputEvent;
        
        private IRunningMethods _runningMethods = new RunningMethods().Methods;
        private IJumpingMethods _jumpingMethods = new JumpingMethods().Methods;

        public IDashStateEvents DashStateEvents => _playerStateMachine.DashStateEvents;
        public IMovementStateEvents MovementStateEvents => _playerStateMachine.MovementStateEvents;
        public IWallHangStateEvents WallHangStateEvents => _playerStateMachine.WallHangStateEvents;
        public IWallJumpStateEvents WallJumpStateEvents => _playerStateMachine.WallJumpStateEvents;
        
        // Timers
        private float _lastOnGroundTime = 0;
        private float _lastPressedJumpTime = 0;

        private bool _canDash = true;
        
        // Hook
        private bool _hookAttempted = false;
        private bool _hookQTEWindow = false;
        private Vector3 _originalPlayerVelocity = Vector3.zero;

        private void Start()
        {
            _playerInputEvent = InputEventManager.Instance.PlayerInputEvent;
         
            SubscribeToObservables();

            _canDash = true;

            GravityScale?.Invoke(Properties.Gravity.Scale);
        }

        private void OnDestroy()
        {
            UnsubscribeToObservables();
        }

        private void Update()
        {
            TickTimers();
            GroundCheck();
            WallCheck();
            JumpChecks();
            GravityShifts();
        }

        private void HookHoldPlayerVelocity()
        {
            
        }

        private void HookResumePlayerVelocity()
        {

        }

        private void TriggerEnter(Collider2D collider)
        {
            if (collider.CompareTag("Hook"))
            {
                IsInHookRange = true;
            }
        }
        
        private void TriggerExit(Collider2D collider)
        {
            if (collider.CompareTag("Hook"))
            {
                IsInHookRange = false;
            }
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
                _canDash = true;
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
                    WallHangStart?.Invoke();
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
            _playerInputEvent.HookStartAction += ExecuteHookStartEvent;
            _playerInputEvent.HookEndAction += ExecuteHookEndEvent;

            _controllerData.TriggerEnterEvent += TriggerEnter;
            _controllerData.TriggerExitEvent += TriggerExit;
        }

        private void UnsubscribeToObservables()
        {
            _playerInputEvent.JumpStartAction -= ExecuteJumpStartEvent;
            _playerInputEvent.JumpEndAction -= ExecuteJumpEndEvent;
            _playerInputEvent.RunAction -= ExecuteRunEvent;
            _playerInputEvent.DashAction -= ExecuteDashStartEvent;
            _playerInputEvent.LookDownAction -= ExecuteLookDownEvent;
            _playerInputEvent.HookStartAction -= ExecuteHookStartEvent;
            _playerInputEvent.HookEndAction -= ExecuteHookEndEvent;

            _controllerData.TriggerEnterEvent -= TriggerEnter;
            _controllerData.TriggerExitEvent -= TriggerExit;
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
        public bool IsInHookRange { get; private set; } = false;

        public float DashSpeed { get; private set; } = 0f;

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
        public Action<float> DashStart { get; set; }
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

        private bool InAir()
        {
            return _lastOnGroundTime <= 0 && !IsOnWallHang;
        }

        private void ExecuteRunEvent(float runDirection)
        {
            UpdateIsRunningRight(runDirection);
            float acceleration = _runningMethods.GetRunAcceleration(runDirection, _controllerData.RigidbodyVelocity.x);
            Run?.Invoke(acceleration);
        }

        private void ExecuteDashStartEvent(float dashDirection)
        {
            if (InAir() && _canDash)
            {
                DashSpeed = dashDirection * Properties.Dash.Speed;
                DashStart?.Invoke(DashSpeed);
                _canDash = false;
            }
        }

        private void ExecuteLookDownEvent(bool isLookingDown)
        {
            IsLookingDown = isLookingDown;
        }

        private void ExecuteHookStartEvent()
        {
            if (IsInHookRange)
            {
                if (!_hookAttempted)
                {
                    _hookAttempted = true;
                    
                }
            }
        }

        private void ExecuteHookEndEvent()
        {
            if (IsInHookRange)
            {
                if (_hookQTEWindow)
                {

                }
            }
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
