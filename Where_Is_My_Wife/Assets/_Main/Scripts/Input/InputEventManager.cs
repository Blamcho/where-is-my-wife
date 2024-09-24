using System;
using UnityEngine;
using UnityEngine.InputSystem;
using WIMW.Input;

namespace WhereIsMyWife.Managers
{
    /// <summary>
    ///  The player input arrives here and raises events for other classes to react via an <see cref="IPlayerInputEvent"/>
    /// </summary>
    public class InputEventManager : Singleton<InputEventManager>, IPlayerInputEvent
    {
        public IPlayerInputEvent PlayerInputEvent => this;
        
        public Action JumpStartAction { get; set; }
        public Action JumpEndAction { get; set; }
        public Action<float> RunAction { get; set; }
        public Action<float> DashAction { get; set; }
        public Action<Vector2> UseItemAction { get; set; }
        public Action HookStartAction { get; set; }
        public Action HookEndAction { get; set; }
        public Action LookUpAction { get; set; }
        public Action<bool> LookDownAction { get; set; }
    
        ControllerType _currentControllerType;
        private string[] controllers;
    
        private PlayerInputActions _playerInputActions;
        
        private Vector2 _moveVector = Vector2.zero;
        
        private float _horizontalDeadZone = 0.5f;
        private float _lookDownThreshold = 0.7f;
        
        protected override void Awake()
        {
            base.Awake();
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Enable();
            SubscribeToInputActions();
            CheckForCurrentController();
        }

        private void OnDestroy()
        {
            UnsubscribeToInputActions();
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
        }
        
        private void UnsubscribeToInputActions()
        {
            _playerInputActions.Normal.Jump.performed -= OnJumpPerform;
            _playerInputActions.Normal.Jump.canceled -= OnJumpCancel;
            _playerInputActions.Normal.Move.performed -= OnMovePerform;
            _playerInputActions.Normal.Move.canceled -= OnMoveCancel;
            _playerInputActions.Normal.Dash.performed -= OnDash;
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
            DashAction?.Invoke(_moveVector.x);
        }
    
        public void Dispose()
        {
            _playerInputActions.Disable();
        }

        // TODO: Change the way it detects the input was from a different controller type
        private void CheckForControllerTypeChange()
        {
            if (Input.anyKeyDown)
            {
                if (InputWasFromJoystick())
                {
                    if (_currentControllerType == ControllerType.Keyboard)
                    {
                        CheckForCurrentController();
                    }
                }
                else if (_currentControllerType != ControllerType.Keyboard)
                {
                    ChangeControllerType(ControllerType.Keyboard);
                }                    
            }
        }

        // TODO: Change the way it detects the input was from a gamepad
        private bool InputWasFromJoystick()
        {
            for (int i = 0; i < 20; i++)
            {
                if (Input.GetKeyDown((KeyCode)350 + i))
                {
                    return true;
                }
            }
            
            return false;
        }
        
        private void CheckForCurrentController()
        {
            controllers = Input.GetJoystickNames();
            
            for (int i = 0; i < controllers.Length; i++)
            {
                Debug.Log(controllers[i]);
            }

            if (OnlyKeyboardIsConnected())
            {
                ChangeControllerType(ControllerType.Keyboard);
            }

            else
            {
                ConnectController();
            }
        }

        private void ChangeControllerType(ControllerType controllerType)
        {
            _currentControllerType = controllerType;
            Debug.Log($"ControllerType: {_currentControllerType}");
        }

        private void ConnectController()
        {
            for (int index = 0; index < controllers.Length; index++)
            {
                if (IsControllerValid(index))
                {
                    if (IsXboxConnected(index))
                    {
                        ChangeControllerType(ControllerType.Xbox);
                    }
                    else if (IsPlaystationConnected(index))
                    {
                        ChangeControllerType(ControllerType.Playstation);
                    }
                    else if (IsNintendoConnected(index))
                    {
                        ChangeControllerType(ControllerType.Nintendo);
                    }
                    else
                    {
                        ChangeControllerType(ControllerType.Xbox);
                    }
                }
            }
        }

        private bool IsControllerValid(int index)
        {
            return controllers[index] != "";
        }

        private bool IsNintendoConnected(int index)
        {
            return controllers[index].Contains("Pro") || controllers[index].Contains("Core") || controllers[index].Contains("Switch");
        }

        private bool IsPlaystationConnected(int index)
        {
            return controllers[index].Contains("playstation") || controllers[index].Contains("PS");
        }

        private bool IsXboxConnected(int index)
        {
            return controllers[index].Contains("Xbox");
        }

        private bool OnlyKeyboardIsConnected()
        {
            return controllers == null || controllers.Length == 0 || (controllers.Length == 1 && controllers[0] == "");
        }
    }
}
