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
                },
                {
                    ""name"": ""Join"",
                    ""type"": ""Button"",
                    ""id"": ""28f11deb-bca0-466d-9523-bf0f8cfc179e"",
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
                    ""groups"": ""Keyboard"",
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
                    ""groups"": ""Keyboard"",
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
                    ""groups"": ""Keyboard"",
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
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""b4fd8843-0af9-4622-a0ab-72860e83b0f1"",
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
                    ""id"": ""0c4b62d7-05c3-43fb-b704-752761f3b883"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6fd4ec22-b152-4f27-b9a1-37b3cdae1ce9"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e154fab3-4b0a-4544-b0e9-f91fd570ea63"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""33f4b97d-4353-4641-8a61-dba3302812b4"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard"",
                    ""action"": ""CarBrake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9bcc514f-697c-446e-b6cd-1f4a68da496c"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard"",
                    ""action"": ""CarAddonTriggerFront"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d08a2633-c846-46a0-9b08-31de948ef3e7"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard"",
                    ""action"": ""CarAddonTriggerSide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fc9e2da8-f9e8-44d6-a60e-3463144f7c6d"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard"",
                    ""action"": ""CarAddonTriggerBack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ec706fa5-0afa-42f1-8b5a-dea09487be52"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard"",
                    ""action"": ""CarAddonTriggerTop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""48d5fe93-0256-45a5-880d-79b0394e6b46"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard"",
                    ""action"": ""ParachuteTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa072d57-59f1-4f5d-86c7-e637583dd8b7"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard"",
                    ""action"": ""Drifting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3048a981-96df-444c-952f-9c8f9f0701ff"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Drifting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ebb1331-e697-4b0a-8b9e-a04eeeefe40a"",
                    ""path"": ""<Keyboard>/anyKey"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Join"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb3245a5-1b41-4921-9431-837814e91ac5"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Join"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Selection"",
            ""id"": ""54a55aa1-305a-4b18-99aa-ed411f93445b"",
            ""actions"": [
                {
                    ""name"": ""MoveUp"",
                    ""type"": ""Button"",
                    ""id"": ""7973a2b8-0655-4b17-8c59-036a196053aa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveDown"",
                    ""type"": ""Button"",
                    ""id"": ""62b24828-ee72-43e7-9ff8-160337c55f03"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveRight"",
                    ""type"": ""Button"",
                    ""id"": ""d66b43a3-6fb8-4497-8d45-be8568ee0316"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveLeft"",
                    ""type"": ""Button"",
                    ""id"": ""1237d970-e961-4abb-a788-7c3e577f6c26"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""de2e1447-e2a3-4f92-9812-2ac54ab3e513"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bcb5d8cc-3313-4069-ae39-2ad5d99952f4"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""194a6bda-80bb-476a-bff6-03cbdd2e81a8"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""727833b3-7ff7-44ec-9c92-266201c08f45"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bef09187-6122-41e5-b52a-b894463b531d"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c103b376-d56d-46b4-9c93-8ed08671b540"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
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
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
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
        m_Player_Join = m_Player.FindAction("Join", throwIfNotFound: true);
        // Selection
        m_Selection = asset.FindActionMap("Selection", throwIfNotFound: true);
        m_Selection_MoveUp = m_Selection.FindAction("MoveUp", throwIfNotFound: true);
        m_Selection_MoveDown = m_Selection.FindAction("MoveDown", throwIfNotFound: true);
        m_Selection_MoveRight = m_Selection.FindAction("MoveRight", throwIfNotFound: true);
        m_Selection_MoveLeft = m_Selection.FindAction("MoveLeft", throwIfNotFound: true);
        m_Selection_Select = m_Selection.FindAction("Select", throwIfNotFound: true);
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
    private readonly InputAction m_Player_Join;
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
        public InputAction @Join => m_Wrapper.m_Player_Join;
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
            @Join.started += instance.OnJoin;
            @Join.performed += instance.OnJoin;
            @Join.canceled += instance.OnJoin;
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
            @Join.started -= instance.OnJoin;
            @Join.performed -= instance.OnJoin;
            @Join.canceled -= instance.OnJoin;
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

    // Selection
    private readonly InputActionMap m_Selection;
    private List<ISelectionActions> m_SelectionActionsCallbackInterfaces = new List<ISelectionActions>();
    private readonly InputAction m_Selection_MoveUp;
    private readonly InputAction m_Selection_MoveDown;
    private readonly InputAction m_Selection_MoveRight;
    private readonly InputAction m_Selection_MoveLeft;
    private readonly InputAction m_Selection_Select;
    public struct SelectionActions
    {
        private @GameInputActions m_Wrapper;
        public SelectionActions(@GameInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveUp => m_Wrapper.m_Selection_MoveUp;
        public InputAction @MoveDown => m_Wrapper.m_Selection_MoveDown;
        public InputAction @MoveRight => m_Wrapper.m_Selection_MoveRight;
        public InputAction @MoveLeft => m_Wrapper.m_Selection_MoveLeft;
        public InputAction @Select => m_Wrapper.m_Selection_Select;
        public InputActionMap Get() { return m_Wrapper.m_Selection; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SelectionActions set) { return set.Get(); }
        public void AddCallbacks(ISelectionActions instance)
        {
            if (instance == null || m_Wrapper.m_SelectionActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_SelectionActionsCallbackInterfaces.Add(instance);
            @MoveUp.started += instance.OnMoveUp;
            @MoveUp.performed += instance.OnMoveUp;
            @MoveUp.canceled += instance.OnMoveUp;
            @MoveDown.started += instance.OnMoveDown;
            @MoveDown.performed += instance.OnMoveDown;
            @MoveDown.canceled += instance.OnMoveDown;
            @MoveRight.started += instance.OnMoveRight;
            @MoveRight.performed += instance.OnMoveRight;
            @MoveRight.canceled += instance.OnMoveRight;
            @MoveLeft.started += instance.OnMoveLeft;
            @MoveLeft.performed += instance.OnMoveLeft;
            @MoveLeft.canceled += instance.OnMoveLeft;
            @Select.started += instance.OnSelect;
            @Select.performed += instance.OnSelect;
            @Select.canceled += instance.OnSelect;
        }

        private void UnregisterCallbacks(ISelectionActions instance)
        {
            @MoveUp.started -= instance.OnMoveUp;
            @MoveUp.performed -= instance.OnMoveUp;
            @MoveUp.canceled -= instance.OnMoveUp;
            @MoveDown.started -= instance.OnMoveDown;
            @MoveDown.performed -= instance.OnMoveDown;
            @MoveDown.canceled -= instance.OnMoveDown;
            @MoveRight.started -= instance.OnMoveRight;
            @MoveRight.performed -= instance.OnMoveRight;
            @MoveRight.canceled -= instance.OnMoveRight;
            @MoveLeft.started -= instance.OnMoveLeft;
            @MoveLeft.performed -= instance.OnMoveLeft;
            @MoveLeft.canceled -= instance.OnMoveLeft;
            @Select.started -= instance.OnSelect;
            @Select.performed -= instance.OnSelect;
            @Select.canceled -= instance.OnSelect;
        }

        public void RemoveCallbacks(ISelectionActions instance)
        {
            if (m_Wrapper.m_SelectionActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ISelectionActions instance)
        {
            foreach (var item in m_Wrapper.m_SelectionActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_SelectionActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public SelectionActions @Selection => new SelectionActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
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
        void OnJoin(InputAction.CallbackContext context);
    }
    public interface ISelectionActions
    {
        void OnMoveUp(InputAction.CallbackContext context);
        void OnMoveDown(InputAction.CallbackContext context);
        void OnMoveRight(InputAction.CallbackContext context);
        void OnMoveLeft(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
    }
}
