// GENERATED AUTOMATICALLY FROM 'Assets/Data/Input/PlayerActionAsset.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Components
{
    public class @PlayerActionAsset : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerActionAsset()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerActionAsset"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""a9562f79-73e7-4e7a-9b59-43d2d97f3162"",
            ""actions"": [
                {
                    ""name"": ""ChangeColor"",
                    ""type"": ""Button"",
                    ""id"": ""139d1038-369d-4f3a-a75f-0db4793c71a8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""84f443ad-318c-430b-9b37-2f59f42986f1"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ChangeColor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_ChangeColor = m_Player.FindAction("ChangeColor", throwIfNotFound: true);
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

        // Player
        private readonly InputActionMap m_Player;
        private IPlayerActions m_PlayerActionsCallbackInterface;
        private readonly InputAction m_Player_ChangeColor;
        public struct PlayerActions
        {
            private @PlayerActionAsset m_Wrapper;
            public PlayerActions(@PlayerActionAsset wrapper) { m_Wrapper = wrapper; }
            public InputAction @ChangeColor => m_Wrapper.m_Player_ChangeColor;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    @ChangeColor.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChangeColor;
                    @ChangeColor.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChangeColor;
                    @ChangeColor.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChangeColor;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @ChangeColor.started += instance.OnChangeColor;
                    @ChangeColor.performed += instance.OnChangeColor;
                    @ChangeColor.canceled += instance.OnChangeColor;
                }
            }
        }
        public PlayerActions @Player => new PlayerActions(this);
        private int m_KeyboardSchemeIndex = -1;
        public InputControlScheme KeyboardScheme
        {
            get
            {
                if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
                return asset.controlSchemes[m_KeyboardSchemeIndex];
            }
        }
        public interface IPlayerActions
        {
            void OnChangeColor(InputAction.CallbackContext context);
        }
    }
}
