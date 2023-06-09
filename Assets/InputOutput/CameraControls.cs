//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/InputOutput/CameraControls.inputactions
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

public partial class @CameraControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @CameraControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CameraControls"",
    ""maps"": [
        {
            ""name"": ""CameraControlsMap"",
            ""id"": ""343749ab-65dd-4bbb-9c36-199d953a77ef"",
            ""actions"": [
                {
                    ""name"": ""MovePointer"",
                    ""type"": ""PassThrough"",
                    ""id"": ""309d8196-49d0-4843-81bc-2bda0cb581c9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Drag"",
                    ""type"": ""Button"",
                    ""id"": ""0a9e8ac7-e9cc-44d7-b7c9-c0f72656aa2a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a0798f6d-f95a-4af5-b62e-6d42cf6dd8eb"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""MovePointer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cd4cc89b-80d8-4e80-98a0-5ca3ff7b50f8"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Drag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mouse"",
            ""bindingGroup"": ""Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // CameraControlsMap
        m_CameraControlsMap = asset.FindActionMap("CameraControlsMap", throwIfNotFound: true);
        m_CameraControlsMap_MovePointer = m_CameraControlsMap.FindAction("MovePointer", throwIfNotFound: true);
        m_CameraControlsMap_Drag = m_CameraControlsMap.FindAction("Drag", throwIfNotFound: true);
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

    // CameraControlsMap
    private readonly InputActionMap m_CameraControlsMap;
    private ICameraControlsMapActions m_CameraControlsMapActionsCallbackInterface;
    private readonly InputAction m_CameraControlsMap_MovePointer;
    private readonly InputAction m_CameraControlsMap_Drag;
    public struct CameraControlsMapActions
    {
        private @CameraControls m_Wrapper;
        public CameraControlsMapActions(@CameraControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MovePointer => m_Wrapper.m_CameraControlsMap_MovePointer;
        public InputAction @Drag => m_Wrapper.m_CameraControlsMap_Drag;
        public InputActionMap Get() { return m_Wrapper.m_CameraControlsMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraControlsMapActions set) { return set.Get(); }
        public void SetCallbacks(ICameraControlsMapActions instance)
        {
            if (m_Wrapper.m_CameraControlsMapActionsCallbackInterface != null)
            {
                @MovePointer.started -= m_Wrapper.m_CameraControlsMapActionsCallbackInterface.OnMovePointer;
                @MovePointer.performed -= m_Wrapper.m_CameraControlsMapActionsCallbackInterface.OnMovePointer;
                @MovePointer.canceled -= m_Wrapper.m_CameraControlsMapActionsCallbackInterface.OnMovePointer;
                @Drag.started -= m_Wrapper.m_CameraControlsMapActionsCallbackInterface.OnDrag;
                @Drag.performed -= m_Wrapper.m_CameraControlsMapActionsCallbackInterface.OnDrag;
                @Drag.canceled -= m_Wrapper.m_CameraControlsMapActionsCallbackInterface.OnDrag;
            }
            m_Wrapper.m_CameraControlsMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MovePointer.started += instance.OnMovePointer;
                @MovePointer.performed += instance.OnMovePointer;
                @MovePointer.canceled += instance.OnMovePointer;
                @Drag.started += instance.OnDrag;
                @Drag.performed += instance.OnDrag;
                @Drag.canceled += instance.OnDrag;
            }
        }
    }
    public CameraControlsMapActions @CameraControlsMap => new CameraControlsMapActions(this);
    private int m_MouseSchemeIndex = -1;
    public InputControlScheme MouseScheme
    {
        get
        {
            if (m_MouseSchemeIndex == -1) m_MouseSchemeIndex = asset.FindControlSchemeIndex("Mouse");
            return asset.controlSchemes[m_MouseSchemeIndex];
        }
    }
    public interface ICameraControlsMapActions
    {
        void OnMovePointer(InputAction.CallbackContext context);
        void OnDrag(InputAction.CallbackContext context);
    }
}
