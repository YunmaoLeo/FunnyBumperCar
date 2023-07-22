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

        carSimulationComponent.BindAddonInputActions(gameInputActions);
    }

    private void FixedUpdate()
    {
        if (ControlledCar == null) return;

        var moveInputVector = gameInputActions.Player.Move.ReadValue<Vector2>();
        var isBraking = gameInputActions.Player.CarBrake.ReadValue<float>() != 0;
        var isDrifting = gameInputActions.Player.Drifting.ReadValue<float>() != 0;
        carSimulationComponent.TireRotateSignal = moveInputVector.x;
        carSimulationComponent.CarDriveSignal = moveInputVector.y;
        carSimulationComponent.IsBraking = isBraking;
        carSimulationComponent.IsDrifting = isDrifting;
    }
}