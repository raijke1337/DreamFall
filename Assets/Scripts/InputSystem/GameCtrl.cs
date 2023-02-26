// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/InputSystem/GameCtrl.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Game.Control
{
    public class @GameCtrl : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @GameCtrl()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameCtrl"",
    ""maps"": [
        {
            ""name"": ""Default"",
            ""id"": ""d4fcae66-c5d2-4928-833a-7b7593b0fcad"",
            ""actions"": [
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""5fbd855e-0bf3-4216-a05d-707979fdf3dd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""9d17db39-0caa-4770-a985-7967160ea7ec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""1d813d5a-d4b6-423b-98cc-c9ffdf63a752"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5f344b3a-eb79-4137-b14b-0a593252cba4"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3c0c8b9e-666a-499d-befb-b1be6d64bbd1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7d67888c-a862-4ebe-b964-c9e60feade15"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Default
            m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
            m_Default_Aim = m_Default.FindAction("Aim", throwIfNotFound: true);
            m_Default_Click = m_Default.FindAction("Click", throwIfNotFound: true);
            m_Default_Pause = m_Default.FindAction("Pause", throwIfNotFound: true);
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

        // Default
        private readonly InputActionMap m_Default;
        private IDefaultActions m_DefaultActionsCallbackInterface;
        private readonly InputAction m_Default_Aim;
        private readonly InputAction m_Default_Click;
        private readonly InputAction m_Default_Pause;
        public struct DefaultActions
        {
            private @GameCtrl m_Wrapper;
            public DefaultActions(@GameCtrl wrapper) { m_Wrapper = wrapper; }
            public InputAction @Aim => m_Wrapper.m_Default_Aim;
            public InputAction @Click => m_Wrapper.m_Default_Click;
            public InputAction @Pause => m_Wrapper.m_Default_Pause;
            public InputActionMap Get() { return m_Wrapper.m_Default; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
            public void SetCallbacks(IDefaultActions instance)
            {
                if (m_Wrapper.m_DefaultActionsCallbackInterface != null)
                {
                    @Aim.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAim;
                    @Aim.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAim;
                    @Aim.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAim;
                    @Click.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnClick;
                    @Click.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnClick;
                    @Click.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnClick;
                    @Pause.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnPause;
                    @Pause.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnPause;
                    @Pause.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnPause;
                }
                m_Wrapper.m_DefaultActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Aim.started += instance.OnAim;
                    @Aim.performed += instance.OnAim;
                    @Aim.canceled += instance.OnAim;
                    @Click.started += instance.OnClick;
                    @Click.performed += instance.OnClick;
                    @Click.canceled += instance.OnClick;
                    @Pause.started += instance.OnPause;
                    @Pause.performed += instance.OnPause;
                    @Pause.canceled += instance.OnPause;
                }
            }
        }
        public DefaultActions @Default => new DefaultActions(this);
        public interface IDefaultActions
        {
            void OnAim(InputAction.CallbackContext context);
            void OnClick(InputAction.CallbackContext context);
            void OnPause(InputAction.CallbackContext context);
        }
    }
}
