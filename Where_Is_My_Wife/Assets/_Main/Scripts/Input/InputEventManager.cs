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
        public Action<Vector2> UseItemAction { get; set; }
        public Action HookStartAction { get; set; }
        public Action HookEndAction { get; set; }
        public Action LookUpAction { get; set; }
        public Action<bool> LookDownAction { get; set; }
        public Action PunchAction { get; set; }

        public Action PauseStartAction { get; set; }
        
        public Action<int> HorizontalStartedAction { get; set; }
        public Action<int> HorizontalCanceledAction { get; set; }
        public Action SubmitStartAction { get; set; }
        public Action CancelStartAction { get; set; }
        
        ControllerType _currentControllerType;
        private string[] controllers;
    
        private PlayerInputActions _playerInputActions;
        
        private Vector2 _moveVector = Vector2.zero;

        private float _axisControllerDetectionThreshold = 0.1f;
        private float _horizontalDeadZone = 0.5f;
        private float _lookDownThreshold = 0.7f;
        
        protected override void Awake()
        {
            base.Awake();
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Enable();
            
            SubscribeToInputActions();
            ChangeControllerType(ControllerType.Keyboard);
        }
        
        private void OnDestroy()
        {
            UnsubscribeToInputActions();
            _playerInputActions.Disable();
        }

        private void Update()
        {
            CheckForControllerTypeChange();
        }

        private void FixedUpdate()
        {
            RunAction?.Invoke(_moveVector.x); 
        }

        private void SubscribeToInputActions()
        {
            _playerInputActions.Normal.Jump.performed += OnJumpPerform;
            _playerInputActions.Normal.Jump.canceled += OnJumpCancel;
            _playerInputActions.Normal.Move.performed += OnMovePerform;
            _playerInputActions.Normal.Move.canceled += OnMoveCancel;
            _playerInputActions.Normal.Dash.performed += OnDash;
            _playerInputActions.Normal.Hook.started += OnHookStart;
            _playerInputActions.Normal.Hook.canceled += OnHookCancel;
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
            _playerInputActions.Normal.Hook.canceled -= OnHookCancel;
            _playerInputActions.Normal.Punch.started -= OnPunchStarted;
            
            _playerInputActions.Special.Pause.started -= OnPauseStart;
            
            _playerInputActions.UI.Navigate.started -= OnNavigateStarted;
            _playerInputActions.UI.Navigate.canceled -= OnNavigateCanceled;
            _playerInputActions.UI.Submit.started -= OnSubmitStart;
            _playerInputActions.UI.Cancel.started -= OnCancelStart;
        }
        
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
            if (_moveVector.x != 0) 
            {
               DashAction?.Invoke(_moveVector.x);
            }
        }

        private void OnHookStart(InputAction.CallbackContext context)
        {
            HookStartAction?.Invoke();
        }

        private void OnHookCancel(InputAction.CallbackContext context)
        {
            HookEndAction?.Invoke();
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
        
        private void CheckForControllerTypeChange()
        {
            if (KeyboardInputWasMadeThisFrame())
            {
                if (_currentControllerType != ControllerType.Keyboard)
                {
                    ChangeControllerType(ControllerType.Keyboard);
                }
            }
            else if (GamepadInputWasMadeThisFrame())
            {
                ControllerType currentGamepadType = GetCurrentGamepadType();
                
                if (_currentControllerType != currentGamepadType)
                {
                    Debug.Log($"Current gamepad name: {Gamepad.current.name}");
                    ChangeControllerType(currentGamepadType);
                }
            }
        }
        
        private void ChangeControllerType(ControllerType controllerType)
        {
            _currentControllerType = controllerType;
            Debug.Log($"ControllerType: {_currentControllerType}");
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
    }
}
