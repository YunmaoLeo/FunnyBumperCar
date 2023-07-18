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
        carSimulationComponent.TireRotateSignal = moveInputVector.x;
        carSimulationComponent.CarDriveSignal = moveInputVector.y;
    }
}
