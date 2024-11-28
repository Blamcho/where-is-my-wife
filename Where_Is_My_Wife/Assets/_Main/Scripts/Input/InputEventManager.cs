using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using WIMW.Input;

namespace WhereIsMyWife.Managers
{
    /// <summary>
    ///  The player input arrives here and raises events for other classes to react via an <see cref="IPlayerInputEvent"/>
    /// </summary>
    public class InputEventManager : Singleton<InputEventManager>, IPlayerInputEvent, IUIInputEvent
    {
        public IPlayerInputEvent PlayerInputEvent => this;
        public IUIInputEvent UIInputEvent => this;
        
        public Action JumpStartAction { get; set; }
        public Action JumpEndAction { get; set; }
        public Action<float> RunAction { get; set; }
        public Action<float> DashAction { get; set; }
        public Action HookStartAction { get; set; }
        public Action<bool> LookDownAction { get; set; }
        public Action PunchAction { get; set; }

        public Action PauseStartAction { get; set; }
        
        public Action<int> HorizontalStartedAction { get; set; }
        public Action<int> HorizontalCanceledAction { get; set; }
        public Action SubmitStartAction { get; set; }
        public Action CancelStartAction { get; set; }

        public event Action<ControllerType> ChangeControllerTypeAction;
        
        public ControllerType CurrentControllerType { get; private set; }
    
        private PlayerInputActions _playerInputActions;
        
        private Vector2 _moveVector = Vector2.zero;
        
        private float _horizontalDeadZone = 0.5f;
        private float _lookDownThreshold = 0.7f;
        
        #if !XBOX
        private float _axisControllerDetectionThreshold = 0.1f;
        #endif

        #if XBOX
        private bool _isPressingDash = false;
        private bool _isPressingHook = false;
        #endif
        
        protected override void Awake()
        {
            base.Awake();
            
            #if !XBOX
                _playerInputActions = new PlayerInputActions();
                _playerInputActions.Enable();
                
                SubscribeToInputActions();
                ChangeControllerType(ControllerType.Keyboard);
            #endif
            
            #if XBOX
                ChangeControllerType(ControllerType.Xbox);
            #endif
        }
        
        private void OnDestroy()
        {
            #if !XBOX
                UnsubscribeToInputActions();
                _playerInputActions.Disable();
            #endif
        }

        
        private void Update()
        {
            #if XBOX
                if (Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    OnJumpPerform(default);
                }
                if (Input.GetKeyUp(KeyCode.JoystickButton0))
                {
                    OnJumpCancel(default);
                }
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    _moveVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                    
                    ApplyHorizontalDeadZone();
                    NormalizeHorizontalAxis();
                
                    LookDownAction?.Invoke(_moveVector.y < -_lookDownThreshold);
                
                    int horizontalValue = 0;
                
                    if (Mathf.Abs(_moveVector.normalized.x) > _horizontalDeadZone)
                    {
                        horizontalValue = (int)Mathf.Sign(_moveVector.x);
                    }
                
                    HorizontalStartedAction?.Invoke(horizontalValue);
                }
                if (Input.GetAxis("Horizontal") == 0)
                {
                    HorizontalStartedAction?.Invoke(0);
                    
                    if (Input.GetAxis("Vertical") == 0)
                    {
                        OnMoveCancel(new InputAction.CallbackContext());
                    }
                }
                if (Input.GetAxis("RT") != 0 && !_isPressingDash)
                {
                    _isPressingDash = true;
                    OnDash(new InputAction.CallbackContext());
                }
                else if (Input.GetAxis("RT") == 0 && _isPressingDash)
                {
                    _isPressingDash = false;
                }
                if (Input.GetAxis("LT") != 0 && !_isPressingHook)
                {
                    _isPressingHook = true;
                    OnHookStart(new InputAction.CallbackContext());
                }
                else if (Input.GetAxis("LT") == 0 && _isPressingHook)
                {
                    _isPressingHook = false;
                }
                if (Input.GetKeyDown(KeyCode.JoystickButton2))
                {
                    OnPunchStarted(new InputAction.CallbackContext());
                }
                if (Input.GetKeyDown(KeyCode.JoystickButton7))
                {
                    OnPauseStart(new InputAction.CallbackContext());
                }
                if (Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    OnSubmitStart(new InputAction.CallbackContext());
                }
                if (Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    OnCancelStart(new InputAction.CallbackContext());
                }
            #endif
            
            #if !XBOX
                CheckForControllerTypeChange();
            #endif
        }
        
        private void FixedUpdate()
        {
            RunAction?.Invoke(_moveVector.x); 
        }

        #if !XBOX
            private void SubscribeToInputActions()
            {
                _playerInputActions.Normal.Jump.performed += OnJumpPerform;
                _playerInputActions.Normal.Jump.canceled += OnJumpCancel;
                _playerInputActions.Normal.Move.performed += OnMovePerform;
                _playerInputActions.Normal.Move.canceled += OnMoveCancel;
                _playerInputActions.Normal.Dash.performed += OnDash;
                _playerInputActions.Normal.Hook.started += OnHookStart;
                _playerInputActions.Normal.Punch.started += OnPunchStarted;

                _playerInputActions.Special.Pause.started += OnPauseStart;
                
                _playerInputActions.UI.Navigate.started += OnNavigateStarted;
                _playerInputActions.UI.Navigate.canceled += OnNavigateCanceled;
                _playerInputActions.UI.Submit.started += OnSubmitStart;
                _playerInputActions.UI.Cancel.started += OnCancelStart;
            }

            private void UnsubscribeToInputActions()
            {
                _playerInputActions.Normal.Jump.performed -= OnJumpPerform;
                _playerInputActions.Normal.Jump.canceled -= OnJumpCancel;
                _playerInputActions.Normal.Move.performed -= OnMovePerform;
                _playerInputActions.Normal.Move.canceled -= OnMoveCancel;
                _playerInputActions.Normal.Dash.performed -= OnDash;
                _playerInputActions.Normal.Hook.started -= OnHookStart;
                _playerInputActions.Normal.Punch.started -= OnPunchStarted;
                
                _playerInputActions.Special.Pause.started -= OnPauseStart;
                
                _playerInputActions.UI.Navigate.started -= OnNavigateStarted;
                _playerInputActions.UI.Navigate.canceled -= OnNavigateCanceled;
                _playerInputActions.UI.Submit.started -= OnSubmitStart;
                _playerInputActions.UI.Cancel.started -= OnCancelStart;
            }
        #endif
        
        private void OnJumpPerform(InputAction.CallbackContext context)
        {
            JumpStartAction?.Invoke();
        }

        private void OnJumpCancel(InputAction.CallbackContext context)
        {
            JumpEndAction?.Invoke();
        }
        
        private void OnMovePerform(InputAction.CallbackContext context)
        {
            _moveVector = context.ReadValue<Vector2>();

            ApplyHorizontalDeadZone();
            NormalizeHorizontalAxis();
            
            LookDownAction?.Invoke(_moveVector.y < -_lookDownThreshold);
        }

        private void ApplyHorizontalDeadZone()
        {
            if (Mathf.Abs(_moveVector.x) < _horizontalDeadZone)
            {
                _moveVector.x = 0;
            }
        }

        private void NormalizeHorizontalAxis()
        {
            if (_moveVector.x == 0) return;
            
            _moveVector.x = Mathf.Sign(_moveVector.x);
        }

        private void OnMoveCancel(InputAction.CallbackContext context)
        {
            _moveVector = Vector2.zero;
            
            LookDownAction?.Invoke(false);
        }

        private void OnDash(InputAction.CallbackContext context)
        {
            DashAction?.Invoke(_moveVector.x);
        }

        private void OnHookStart(InputAction.CallbackContext context)
        {
            HookStartAction?.Invoke();
        }

        private void OnPunchStarted(InputAction.CallbackContext context)
        {
            PunchAction?.Invoke();
        }       
        
        private void OnPauseStart(InputAction.CallbackContext context)
        {
            PauseStartAction?.Invoke();
        }

        private void OnNavigateStarted(InputAction.CallbackContext context)
        {
            HorizontalStartedAction?.Invoke(NormalizedInputContextX(context));
        }
        
        private void OnNavigateCanceled(InputAction.CallbackContext context)
        {
            HorizontalStartedAction?.Invoke(0);
        }

        private int NormalizedInputContextX(InputAction.CallbackContext context)
        {
            if (Mathf.Abs(context.ReadValue<Vector2>().normalized.x) < _horizontalDeadZone) return 0;
            
            return (int)Mathf.Sign(context.ReadValue<Vector2>().x);
        }

        private void OnSubmitStart(InputAction.CallbackContext context)
        {
            SubmitStartAction?.Invoke();
        }
        
        private void OnCancelStart(InputAction.CallbackContext context)
        {
            CancelStartAction?.Invoke();
        }
        
        private void ChangeControllerType(ControllerType controllerType)
        {
            CurrentControllerType = controllerType; 
            ChangeControllerTypeAction?.Invoke(controllerType);
            //Debug.Log($"ControllerType: {_currentControllerType}");
        }
        
        #if !XBOX
            private void CheckForControllerTypeChange()
            {
                if (KeyboardInputWasMadeThisFrame())
                {
                    if (CurrentControllerType != ControllerType.Keyboard)
                    {
                        ChangeControllerType(ControllerType.Keyboard);
                    }
                }
                else if (GamepadInputWasMadeThisFrame())
                {
                    ControllerType currentGamepadType = GetCurrentGamepadType();
                    
                    if (CurrentControllerType != currentGamepadType)
                    {
                        //Debug.Log($"Current gamepad name: {Gamepad.current.name}");
                        ChangeControllerType(currentGamepadType);
                    }
                }
            }
            
            private bool KeyboardInputWasMadeThisFrame()
            {
                return Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame;
            }
            
            private bool GamepadInputWasMadeThisFrame()
            {
                if (Gamepad.current == null) return false;
                
                var gamepad = Gamepad.current;
                
                foreach (var control in gamepad.allControls)
                {
                    if (control is ButtonControl { wasPressedThisFrame: true })
                    {
                        return true;
                    }
                    if (control is AxisControl axis && axis.ReadValue() > _axisControllerDetectionThreshold) 
                    {
                        return true;
                    }
                }

                return false;
            }

            private ControllerType GetCurrentGamepadType()
            {
                string gamepadName = Gamepad.current.name.ToLower();
                
                if (gamepadName.Contains("xbox") || gamepadName.Contains("microsoft"))
                {
                    return ControllerType.Xbox;
                }
                if (gamepadName.Contains("playstation") || gamepadName.Contains("ps") || gamepadName.Contains("dualshock") || gamepadName.Contains("dualsense"))
                {
                    return ControllerType.Playstation;
                }
                if (gamepadName.Contains("switch") || gamepadName.Contains("nintendo") || gamepadName.Contains("joycon") || gamepadName.Contains("pro"))
                {
                    return ControllerType.Nintendo;
                }
               
                return ControllerType.Xbox; 
            }
        #endif
    }
}
