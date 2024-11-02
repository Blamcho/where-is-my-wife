using System;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Managers.Properties;
using WhereIsMyWife.Player.State;
using WhereIsMyWife.Player.StateMachine;
using WhereIsMyWife.UI;


namespace WhereIsMyWife.Managers
{
    /// <summary>
    /// Receives the input made by the player and process it with customized properties and then raises events via <see cref="IPlayerStateInput"/> and gives information via <see cref="IPlayerStateIndicator"/>
    /// </summary>
    public partial class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField]
        private PlayerProperties _propertiesSO;

        [SerializeField]
        private PlayerStateMachine _playerStateMachine;

        public IPlayerProperties Properties => _propertiesSO.Properties;

        private IPlayerInputEvent _playerInputEvent;

        private IRunningMethods _runningMethods = new RunningMethods().Methods;
        private IJumpingMethods _jumpingMethods = new JumpingMethods().Methods;

        public IDashStateEvents DashStateEvents => _playerStateMachine.DashStateEvents;
        public IMovementStateEvents MovementStateEvents => _playerStateMachine.MovementStateEvents;
        public IWallHangStateEvents WallHangStateEvents => _playerStateMachine.WallHangStateEvents;
        public IWallJumpStateEvents WallJumpStateEvents => _playerStateMachine.WallJumpStateEvents;
        public IHookStateEvents HookStateEvents => _playerStateMachine.HookStateEvents;
        public IPunchingStateEvents PunchingStateEvents => _playerStateMachine.PunchingStateEvents;

        // Timers
        private float _lastOnGroundTime = 0;
        private float _lastPressedJumpTime = 0;
        
        private bool _canDash = true;

        // Hook Attempt Flag
        private bool _canAttemptHook = false;
        private bool _isExecutingHook = false;
        
        private void Start()
        {
            //Time.timeScale = 0.7f;
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

        private void TriggerEnter(Collider2D collider)
        {
            if (collider.CompareTag("Hook"))
            {
                IsInHookRange = true;
                _canAttemptHook = true;
                HookPosition = collider.transform.position;
            }

            if (collider.CompareTag("DoubleJump"))
            {
                IsInDoubleJumpTrigger = true;
            }
        }

        private void TriggerExit(Collider2D collider)
        {
            if (collider.CompareTag("Hook"))
            {
                IsInHookRange = false;
            }
            
            if (collider.CompareTag("DoubleJump"))
            {
                IsInDoubleJumpTrigger = false;
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
            return Physics2D.OverlapBox(
                PlayerControllerData.GroundCheckPosition,
                Properties.Check.GroundCheckSize,
                0,
                Properties.Check.GroundCheckLayerMask
            );
        }

        private void WallCheck()
        {
            if (GetWallHangCheck())
            {
                if (ShouldStartWallHang())
                {
                    IsOnWallHang = true;
                    _isExecutingHook = false;
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
            return (
                Physics2D.OverlapBox(
                    PlayerControllerData.WallHangCheckUpPosition,
                    Properties.Check.WallHangCheckSize,
                    0,
                    Properties.Check.WallHangCheckLayerMask
                )
                && Physics2D.OverlapBox(
                    PlayerControllerData.WallHangCheckDownPosition,
                    Properties.Check.WallHangCheckSize,
                    0,
                    Properties.Check.WallHangCheckLayerMask
                )
            );
        }

        private bool ShouldStartWallHang()
        {
            return (IsJumping || IsRunFalling) && IsAccelerating;
        }
        
        private void JumpChecks()
        {
            JumpingCheck();
            LandCheck();

            if (_lastPressedJumpTime > 0 && CanJump()) // Button press needs to happen before CanJump() for DoubleJump
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
            if (IsJumping && PlayerControllerData.RigidbodyVelocity.y < 0)
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
                _isExecutingHook = false;
                Land?.Invoke();
            }
        }

        private void Jump()
        {
            ResetJumpTimers();

            JumpStart?.Invoke(_jumpingMethods.GetJumpForce(PlayerControllerData.RigidbodyVelocity.y));
        }

        private void ResetJumpTimers()
        {
            _lastPressedJumpTime = 0;
            _lastOnGroundTime = 0;
        }

        private void GravityShifts()
        {
            // Scale gravity up if jump button released
            if (IsJumpCut || _isExecutingHook)
            {
                SetGravityScale(Properties.Gravity.Scale * Properties.Gravity.JumpCutMultiplier);
                SetFallSpeedCap(Properties.Gravity.MaxBaseFallSpeed);
            }
            // Make player fall faster if holding down
            else if (IsFastFalling())
            {
                SetGravityScale(Properties.Gravity.Scale * Properties.Gravity.FastFallMultiplier);
                SetFallSpeedCap(Properties.Gravity.MaxFastFallSpeed);
            }
            // Higher gravity when near jump height apex
            else if (IsInJumpHang())
            {
                SetGravityScale(Properties.Gravity.Scale * Properties.Gravity.JumpHangMultiplier);
            }
            // Higher gravity if falling
            else if (IsJumpFalling)
            {
                SetGravityScale(Properties.Gravity.Scale * Properties.Gravity.BaseFallMultiplier);
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
            _playerInputEvent.PunchAction += ExecutePunchStartEvent;
        }

        private void UnsubscribeToObservables()
        {
            _playerInputEvent.JumpStartAction -= ExecuteJumpStartEvent;
            _playerInputEvent.JumpEndAction -= ExecuteJumpEndEvent;
            _playerInputEvent.RunAction -= ExecuteRunEvent;
            _playerInputEvent.DashAction -= ExecuteDashStartEvent;
            _playerInputEvent.LookDownAction -= ExecuteLookDownEvent;
            _playerInputEvent.HookStartAction -= ExecuteHookStartEvent;
            _playerInputEvent.PunchAction -= ExecutePunchStartEvent;

            PlayerControllerData.TriggerEnterEvent -= TriggerEnter;
            PlayerControllerData.TriggerExitEvent -= TriggerExit;
        }
    }

    public partial class PlayerManager : IPlayerStateIndicator
    {
        public IPlayerStateIndicator PlayerStateIndicator => this;

        public bool IsDead { get; private set; } = false;
        public bool IsAccelerating => _runningMethods.GetIsAccelerating();
        public bool IsRunningRight { get; private set; } = true;
        public bool IsLookingRight => PlayerControllerData.HorizontalScale > 0;
        public bool IsLookingDown { get; private set; }
        public bool IsJumping { get; private set; } = false;
        public bool IsJumpCut { get; private set; } = false;
        public bool IsJumpFalling { get; private set; } = false;
        public bool IsOnWallHang { get; private set; } = false;
        public bool IsRunFalling { get; private set; } = false;
        public bool IsInHookRange { get; private set; } = false;
        public bool IsInQTEWindow { get; private set; } = false;
        public Vector2 HookPosition { get; private set; }
        public Vector2 HookLaunchImpulse { get; private set; }

        public float DashSpeed { get; private set; } = 0f;
        public bool IsInDoubleJumpTrigger { get; private set; } = false;

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
            return PlayerControllerData.RigidbodyVelocity.y < 0 && IsLookingDown;
        }

        public bool IsInJumpHang()
        {
            return (IsJumping || IsJumpFalling)
                && Mathf.Abs(PlayerControllerData.RigidbodyVelocity.y)
                    < Properties.Jump.HangTimeThreshold;
        }

        public bool IsIdling()
        {
            return (
                Mathf.Abs(PlayerControllerData.RigidbodyVelocity.x) < 0.1f
                && Mathf.Abs(PlayerControllerData.RigidbodyVelocity.y) < 0.1f
            );
        }

        public bool CanJump()
        {
            if (IsInDoubleJumpTrigger)
            {
                IsInDoubleJumpTrigger = false;
                return true;
            }
            
            return (_lastOnGroundTime > 0 && !IsJumping) || IsOnWallHang;
        }

        public bool CanJumpCut()
        {
            return IsJumping && PlayerControllerData.RigidbodyVelocity.y > 0;
        }

        private void SetIsInQTEWindow(bool isInQTEWindow)
        {
            IsInQTEWindow = isInQTEWindow;
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
        public Action HookStart { get; set; }
        public Action PunchStart { get; set; }
        public Action PunchEnd { get; set; }

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
            float acceleration = _runningMethods.GetRunAcceleration(
                runDirection,
                PlayerControllerData.RigidbodyVelocity.x
            );
            Run?.Invoke(acceleration);
        }

        private void ExecuteDashStartEvent(float dashDirection)
        {
            if (InAir() && _canDash)
            {
                DashSpeed = dashDirection * Properties.Dash.Speed;
                _isExecutingHook = false;
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
            if (_canAttemptHook)
            {
                if (IsInHookRange)
                {
                    HookLaunchImpulse = GetHookLaunchImpulse();
                    _isExecutingHook = true;
                    GravityShifts();
                    HookStart?.Invoke();
                }
            }
        }

        private Vector2 GetHookLaunchImpulse()
        {
            return (HookPosition - PlayerControllerData.RigidbodyPosition).normalized * Properties.Hook.ThrustForce;
        }

        private void ExecutePunchStartEvent()
        {
            PunchStart?.Invoke();
        }
    }

    public partial class PlayerManager : IPlayerControllerEvent
    {
        public IPlayerControllerEvent PlayerControllerEvent => this;

        public IPlayerControllerData PlayerControllerData { get; private set; }

        public void SetPlayerControllerData(IPlayerControllerData playerControllerData)
        {
            PlayerControllerData = playerControllerData;
            
            PlayerControllerData.TriggerEnterEvent += TriggerEnter;
            PlayerControllerData.TriggerExitEvent += TriggerExit;
        }
    }

    public partial class PlayerManager : IRespawn
    {
        public IRespawn Respawn => this;

        private Vector3 _respawnPoint;

        public Action DeathAction { get; set; }
        public Action<Vector3> RespawnStartAction { get; set; }
        public Action RespawnCompleteAction { get; set; }

        public void SetRespawnPoint(Vector3 respawnPoint)
        {
            _respawnPoint = respawnPoint;
        }
        
        public void TriggerDeath()
        {
            DeathAction?.Invoke();  
        }

        public void TriggerRespawnStart()
        {
            _isExecutingHook = false;
            RespawnStartAction?.Invoke(_respawnPoint);
        }

        public void TriggerRespawnComplete()
        {
            RespawnCompleteAction?.Invoke();
        }
    }
}
