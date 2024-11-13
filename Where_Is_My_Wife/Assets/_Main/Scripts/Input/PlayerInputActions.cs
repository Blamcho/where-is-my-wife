//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/_Main/Assets/Input/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace WIMW.Input
{
    public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerInputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Normal"",
            ""id"": ""70a95be3-a496-48ef-8e2c-9c18e511e68c"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""ebd29844-6443-4df6-b776-b2b7cfe4ac1c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""8ed5da81-684c-4609-97c4-4a02e71af800"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""cf62e6bf-b4de-4e8c-a33b-33abb3785c6e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Hook"",
                    ""type"": ""Button"",
                    ""id"": ""df902fb0-fadb-445f-a1a7-eb828d8db686"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Punch"",
                    ""type"": ""Button"",
                    ""id"": ""452f5cb8-ee2a-4b4f-9eac-d01b505c0d17"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""57faf394-135a-46a0-abb3-4f34681f7903"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.125,max=0.125)"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f2b5d208-c691-4987-b4f5-22c42958c5a6"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""15f9ff29-98d2-4769-a456-9d2cbf080493"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b7bed574-369e-45ad-b8b4-b6693501b424"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""be58291f-f64e-400b-985d-f35511e74979"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""02bcfed3-3777-4012-b4fd-3e70de68837e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6976c26c-73cb-42c0-aa6c-0bc8e321d023"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""cb4d9c15-b916-4f78-9218-1630e09c75bb"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4fe97475-5c51-4c36-b99a-ca6cb0ed20a0"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8f437f3-50aa-4c84-bd73-1fd831c55a15"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d69615cd-eecb-4b7b-a0bc-08e293e1dccb"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fad5dad9-aa0e-45fb-bf76-c0ea04756bb7"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": ""AxisDeadzone"",
                    ""groups"": """",
                    ""action"": ""Hook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""34d7c13f-da55-4a84-af6a-7d2557850272"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c0ffceba-40af-41cf-81ad-8fb54330ee5f"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Punch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f90e0f5-c37e-48a6-9d26-b4e38a24b49e"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Punch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Special"",
            ""id"": ""b54467d5-a16c-4151-aa2d-e224ed06a435"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""c39fcbf6-b1fc-4acc-ab60-71a3b43f8d3a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2a08f035-0d3c-438b-bdde-ad7a123c264b"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2bfe5bd7-546f-40c5-bcc3-e0aab9b72a8b"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""6fdef99f-daab-44a8-8a16-37b422d4961a"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""2b1d5f6d-6404-4dfc-ad84-fe4caa279954"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""d0bdc1d5-47bf-4215-ad24-81e6bc134ac8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""238e4cd9-b02f-4aaa-81ae-1fe15e1c5d11"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""865bf3dd-42e3-49ef-a44b-9d83f1abf5eb"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04b770ed-f155-4354-9de0-49c837005eef"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""96b60f0e-d76c-45d6-8dbc-1587fdef5769"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0c18b525-53ae-4d13-84b9-34866faed79e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""843666c8-2123-49b0-8a1b-230b1d1a448d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ce304ddb-4a91-41c3-b2fc-d935e9377a3b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1f7bbc86-0f02-472b-9031-d5b9244b12c0"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""84444c09-008d-40d7-8c1f-f86585eb1169"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""5fe6e8bc-366d-43b1-b0e5-053159a41fb2"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f2964dff-304d-414d-8ebf-b9a9382c9abf"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6602a174-4351-45b5-8d67-e9e8d4635393"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0909c11f-ba7b-4a9a-972a-31fab664c8b7"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""32b31932-67a2-42ba-84bf-9a42fe4176be"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be0b78c2-5518-4212-9385-eb8c2edd7145"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""271fd9ed-b5ce-4a7e-8411-05016bb33ae1"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ccdb528-5c8f-4b41-89eb-f88b744c79d5"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""02678354-3f9d-4412-b4cd-9f2df1a436cc"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Normal
            m_Normal = asset.FindActionMap("Normal", throwIfNotFound: true);
            m_Normal_Move = m_Normal.FindAction("Move", throwIfNotFound: true);
            m_Normal_Jump = m_Normal.FindAction("Jump", throwIfNotFound: true);
            m_Normal_Dash = m_Normal.FindAction("Dash", throwIfNotFound: true);
            m_Normal_Hook = m_Normal.FindAction("Hook", throwIfNotFound: true);
            m_Normal_Punch = m_Normal.FindAction("Punch", throwIfNotFound: true);
            // Special
            m_Special = asset.FindActionMap("Special", throwIfNotFound: true);
            m_Special_Pause = m_Special.FindAction("Pause", throwIfNotFound: true);
            // UI
            m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
            m_UI_Navigate = m_UI.FindAction("Navigate", throwIfNotFound: true);
            m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
            m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Normal
        private readonly InputActionMap m_Normal;
        private List<INormalActions> m_NormalActionsCallbackInterfaces = new List<INormalActions>();
        private readonly InputAction m_Normal_Move;
        private readonly InputAction m_Normal_Jump;
        private readonly InputAction m_Normal_Dash;
        private readonly InputAction m_Normal_Hook;
        private readonly InputAction m_Normal_Punch;
        public struct NormalActions
        {
            private @PlayerInputActions m_Wrapper;
            public NormalActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_Normal_Move;
            public InputAction @Jump => m_Wrapper.m_Normal_Jump;
            public InputAction @Dash => m_Wrapper.m_Normal_Dash;
            public InputAction @Hook => m_Wrapper.m_Normal_Hook;
            public InputAction @Punch => m_Wrapper.m_Normal_Punch;
            public InputActionMap Get() { return m_Wrapper.m_Normal; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(NormalActions set) { return set.Get(); }
            public void AddCallbacks(INormalActions instance)
            {
                if (instance == null || m_Wrapper.m_NormalActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_NormalActionsCallbackInterfaces.Add(instance);
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Hook.started += instance.OnHook;
                @Hook.performed += instance.OnHook;
                @Hook.canceled += instance.OnHook;
                @Punch.started += instance.OnPunch;
                @Punch.performed += instance.OnPunch;
                @Punch.canceled += instance.OnPunch;
            }

            private void UnregisterCallbacks(INormalActions instance)
            {
                @Move.started -= instance.OnMove;
                @Move.performed -= instance.OnMove;
                @Move.canceled -= instance.OnMove;
                @Jump.started -= instance.OnJump;
                @Jump.performed -= instance.OnJump;
                @Jump.canceled -= instance.OnJump;
                @Dash.started -= instance.OnDash;
                @Dash.performed -= instance.OnDash;
                @Dash.canceled -= instance.OnDash;
                @Hook.started -= instance.OnHook;
                @Hook.performed -= instance.OnHook;
                @Hook.canceled -= instance.OnHook;
                @Punch.started -= instance.OnPunch;
                @Punch.performed -= instance.OnPunch;
                @Punch.canceled -= instance.OnPunch;
            }

            public void RemoveCallbacks(INormalActions instance)
            {
                if (m_Wrapper.m_NormalActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(INormalActions instance)
            {
                foreach (var item in m_Wrapper.m_NormalActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_NormalActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public NormalActions @Normal => new NormalActions(this);

        // Special
        private readonly InputActionMap m_Special;
        private List<ISpecialActions> m_SpecialActionsCallbackInterfaces = new List<ISpecialActions>();
        private readonly InputAction m_Special_Pause;
        public struct SpecialActions
        {
            private @PlayerInputActions m_Wrapper;
            public SpecialActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Pause => m_Wrapper.m_Special_Pause;
            public InputActionMap Get() { return m_Wrapper.m_Special; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(SpecialActions set) { return set.Get(); }
            public void AddCallbacks(ISpecialActions instance)
            {
                if (instance == null || m_Wrapper.m_SpecialActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_SpecialActionsCallbackInterfaces.Add(instance);
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }

            private void UnregisterCallbacks(ISpecialActions instance)
            {
                @Pause.started -= instance.OnPause;
                @Pause.performed -= instance.OnPause;
                @Pause.canceled -= instance.OnPause;
            }

            public void RemoveCallbacks(ISpecialActions instance)
            {
                if (m_Wrapper.m_SpecialActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(ISpecialActions instance)
            {
                foreach (var item in m_Wrapper.m_SpecialActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_SpecialActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public SpecialActions @Special => new SpecialActions(this);

        // UI
        private readonly InputActionMap m_UI;
        private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();
        private readonly InputAction m_UI_Navigate;
        private readonly InputAction m_UI_Submit;
        private readonly InputAction m_UI_Cancel;
        public struct UIActions
        {
            private @PlayerInputActions m_Wrapper;
            public UIActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Navigate => m_Wrapper.m_UI_Navigate;
            public InputAction @Submit => m_Wrapper.m_UI_Submit;
            public InputAction @Cancel => m_Wrapper.m_UI_Cancel;
            public InputActionMap Get() { return m_Wrapper.m_UI; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
            public void AddCallbacks(IUIActions instance)
            {
                if (instance == null || m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
            }

            private void UnregisterCallbacks(IUIActions instance)
            {
                @Navigate.started -= instance.OnNavigate;
                @Navigate.performed -= instance.OnNavigate;
                @Navigate.canceled -= instance.OnNavigate;
                @Submit.started -= instance.OnSubmit;
                @Submit.performed -= instance.OnSubmit;
                @Submit.canceled -= instance.OnSubmit;
                @Cancel.started -= instance.OnCancel;
                @Cancel.performed -= instance.OnCancel;
                @Cancel.canceled -= instance.OnCancel;
            }

            public void RemoveCallbacks(IUIActions instance)
            {
                if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IUIActions instance)
            {
                foreach (var item in m_Wrapper.m_UIActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public UIActions @UI => new UIActions(this);
        public interface INormalActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnDash(InputAction.CallbackContext context);
            void OnHook(InputAction.CallbackContext context);
            void OnPunch(InputAction.CallbackContext context);
        }
        public interface ISpecialActions
        {
            void OnPause(InputAction.CallbackContext context);
        }
        public interface IUIActions
        {
            void OnNavigate(InputAction.CallbackContext context);
            void OnSubmit(InputAction.CallbackContext context);
            void OnCancel(InputAction.CallbackContext context);
        }
    }
}
