//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/InputSystems/GameInputActions.inputactions
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

public partial class @GameInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""989761ba-f171-48d0-804c-3f5c22101d83"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""f204cb75-4244-4f56-9d48-9a0eb41f1e2e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CarBrake"",
                    ""type"": ""Button"",
                    ""id"": ""71ef74ba-b6aa-4b95-af6e-ae163980baa1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CarAddonTriggerFront"",
                    ""type"": ""Button"",
                    ""id"": ""9214a617-fafd-4459-b1f4-11c901dfda9f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CarAddonTriggerSide"",
                    ""type"": ""Button"",
                    ""id"": ""1b742196-9bff-47bb-846b-fd4cfd9b61d7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CarAddonTriggerBack"",
                    ""type"": ""Button"",
                    ""id"": ""fcfba858-adf5-4b70-aa85-ac7b2c315266"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CarAddonTriggerTop"",
                    ""type"": ""Button"",
                    ""id"": ""abdf5844-514d-47ba-b2be-a79b19d093f4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ParachuteTrigger"",
                    ""type"": ""Button"",
                    ""id"": ""05beca8e-8464-49e2-9d32-1354fe67b5c8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Drifting"",
                    ""type"": ""Button"",
                    ""id"": ""ae9565ee-d929-4c4e-b279-a8954459f110"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""2b217fa4-b4cf-41dd-87a9-746a921101a5"",
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
                    ""id"": ""9940a8c6-79de-49f1-a165-1983021e6692"",
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
                    ""id"": ""f15a10ca-fbc0-404a-a7cf-58da71c732e0"",
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
                    ""id"": ""9a90724c-c5e3-4035-b03e-d6dcecb2106c"",
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
                    ""id"": ""9c99ce35-1bee-491a-af25-ce5b9d1bf87f"",
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
                    ""id"": ""96a30322-f834-455e-808f-2a76f4caba4d"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CarBrake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7979221-67b6-4848-9e45-9a78429b6a59"",
                    ""path"": ""<Keyboard>/h"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CarAddonTriggerFront"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35544993-ceae-4329-b069-c6bf5262b7ba"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CarAddonTriggerSide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d3773772-485a-4471-b5c0-70d00435e78d"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CarAddonTriggerBack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b6ad9b02-60b2-4446-bcdb-e079852d5008"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CarAddonTriggerTop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bdd4524e-d722-4356-b4cf-a6dd1df6b9cd"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ParachuteTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7904eaa-eb8b-42ec-875f-c60b482d2a07"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drifting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_CarBrake = m_Player.FindAction("CarBrake", throwIfNotFound: true);
        m_Player_CarAddonTriggerFront = m_Player.FindAction("CarAddonTriggerFront", throwIfNotFound: true);
        m_Player_CarAddonTriggerSide = m_Player.FindAction("CarAddonTriggerSide", throwIfNotFound: true);
        m_Player_CarAddonTriggerBack = m_Player.FindAction("CarAddonTriggerBack", throwIfNotFound: true);
        m_Player_CarAddonTriggerTop = m_Player.FindAction("CarAddonTriggerTop", throwIfNotFound: true);
        m_Player_ParachuteTrigger = m_Player.FindAction("ParachuteTrigger", throwIfNotFound: true);
        m_Player_Drifting = m_Player.FindAction("Drifting", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_CarBrake;
    private readonly InputAction m_Player_CarAddonTriggerFront;
    private readonly InputAction m_Player_CarAddonTriggerSide;
    private readonly InputAction m_Player_CarAddonTriggerBack;
    private readonly InputAction m_Player_CarAddonTriggerTop;
    private readonly InputAction m_Player_ParachuteTrigger;
    private readonly InputAction m_Player_Drifting;
    public struct PlayerActions
    {
        private @GameInputActions m_Wrapper;
        public PlayerActions(@GameInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @CarBrake => m_Wrapper.m_Player_CarBrake;
        public InputAction @CarAddonTriggerFront => m_Wrapper.m_Player_CarAddonTriggerFront;
        public InputAction @CarAddonTriggerSide => m_Wrapper.m_Player_CarAddonTriggerSide;
        public InputAction @CarAddonTriggerBack => m_Wrapper.m_Player_CarAddonTriggerBack;
        public InputAction @CarAddonTriggerTop => m_Wrapper.m_Player_CarAddonTriggerTop;
        public InputAction @ParachuteTrigger => m_Wrapper.m_Player_ParachuteTrigger;
        public InputAction @Drifting => m_Wrapper.m_Player_Drifting;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @CarBrake.started += instance.OnCarBrake;
            @CarBrake.performed += instance.OnCarBrake;
            @CarBrake.canceled += instance.OnCarBrake;
            @CarAddonTriggerFront.started += instance.OnCarAddonTriggerFront;
            @CarAddonTriggerFront.performed += instance.OnCarAddonTriggerFront;
            @CarAddonTriggerFront.canceled += instance.OnCarAddonTriggerFront;
            @CarAddonTriggerSide.started += instance.OnCarAddonTriggerSide;
            @CarAddonTriggerSide.performed += instance.OnCarAddonTriggerSide;
            @CarAddonTriggerSide.canceled += instance.OnCarAddonTriggerSide;
            @CarAddonTriggerBack.started += instance.OnCarAddonTriggerBack;
            @CarAddonTriggerBack.performed += instance.OnCarAddonTriggerBack;
            @CarAddonTriggerBack.canceled += instance.OnCarAddonTriggerBack;
            @CarAddonTriggerTop.started += instance.OnCarAddonTriggerTop;
            @CarAddonTriggerTop.performed += instance.OnCarAddonTriggerTop;
            @CarAddonTriggerTop.canceled += instance.OnCarAddonTriggerTop;
            @ParachuteTrigger.started += instance.OnParachuteTrigger;
            @ParachuteTrigger.performed += instance.OnParachuteTrigger;
            @ParachuteTrigger.canceled += instance.OnParachuteTrigger;
            @Drifting.started += instance.OnDrifting;
            @Drifting.performed += instance.OnDrifting;
            @Drifting.canceled += instance.OnDrifting;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @CarBrake.started -= instance.OnCarBrake;
            @CarBrake.performed -= instance.OnCarBrake;
            @CarBrake.canceled -= instance.OnCarBrake;
            @CarAddonTriggerFront.started -= instance.OnCarAddonTriggerFront;
            @CarAddonTriggerFront.performed -= instance.OnCarAddonTriggerFront;
            @CarAddonTriggerFront.canceled -= instance.OnCarAddonTriggerFront;
            @CarAddonTriggerSide.started -= instance.OnCarAddonTriggerSide;
            @CarAddonTriggerSide.performed -= instance.OnCarAddonTriggerSide;
            @CarAddonTriggerSide.canceled -= instance.OnCarAddonTriggerSide;
            @CarAddonTriggerBack.started -= instance.OnCarAddonTriggerBack;
            @CarAddonTriggerBack.performed -= instance.OnCarAddonTriggerBack;
            @CarAddonTriggerBack.canceled -= instance.OnCarAddonTriggerBack;
            @CarAddonTriggerTop.started -= instance.OnCarAddonTriggerTop;
            @CarAddonTriggerTop.performed -= instance.OnCarAddonTriggerTop;
            @CarAddonTriggerTop.canceled -= instance.OnCarAddonTriggerTop;
            @ParachuteTrigger.started -= instance.OnParachuteTrigger;
            @ParachuteTrigger.performed -= instance.OnParachuteTrigger;
            @ParachuteTrigger.canceled -= instance.OnParachuteTrigger;
            @Drifting.started -= instance.OnDrifting;
            @Drifting.performed -= instance.OnDrifting;
            @Drifting.canceled -= instance.OnDrifting;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnCarBrake(InputAction.CallbackContext context);
        void OnCarAddonTriggerFront(InputAction.CallbackContext context);
        void OnCarAddonTriggerSide(InputAction.CallbackContext context);
        void OnCarAddonTriggerBack(InputAction.CallbackContext context);
        void OnCarAddonTriggerTop(InputAction.CallbackContext context);
        void OnParachuteTrigger(InputAction.CallbackContext context);
        void OnDrifting(InputAction.CallbackContext context);
    }
}
