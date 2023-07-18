using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] public Transform ControlledCar;

    private CarSimulation carSimulationComponent;
    private GameInputActions gameInputActions;
    
    private void Awake()
    {
        carSimulationComponent = ControlledCar.GetComponent<CarSimulation>();
        
        gameInputActions = new GameInputActions();
        gameInputActions.Player.Enable();
    }

    private void FixedUpdate()
    {
        if (ControlledCar == null) return;

        var moveInputVector = gameInputActions.Player.Move.ReadValue<Vector2>();
        
        var isBraking =gameInputActions.Player.CarBrake.ReadValue<float>() != 0;
        
        Debug.Log("car brake info: "+ isBraking);
        carSimulationComponent.TireRotateSignal = moveInputVector.x;
        carSimulationComponent.CarDriveSignal = moveInputVector.y;
        carSimulationComponent.IsBraking = isBraking;
    }
}
