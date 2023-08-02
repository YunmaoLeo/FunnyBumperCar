using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform CarPrefab;

    private Transform carTransform;
    private CarSimulation carSimulationComponent;
    private InputActionMap player;

    private int playerIndex;

    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        player = playerInput.actions.FindActionMap("Player");
        playerIndex = playerInput.playerIndex;
    }

    private void Start()
    {
        carTransform = Instantiate(CarPrefab, transform);
        carSimulationComponent = carTransform.GetComponent<CarSimulation>();
        carSimulationComponent.BindAddonInputActions(player);

        //initialize carManager;
        CarsAndCameraManager.Instance.RegisterCar(carTransform, playerIndex);
    }

    private void OnEnable()
    {
        player.Enable();
    }

    private void FixedUpdate()
    {
        var moveInputVector = player.FindAction("Move").ReadValue<Vector2>();
        var isBraking = player.FindAction("CarBrake").ReadValue<float>() != 0;
        var isDrifting = player.FindAction("Drifting").ReadValue<float>() != 0;
        carSimulationComponent.TireRotateSignal = moveInputVector.x;
        carSimulationComponent.CarDriveSignal = moveInputVector.y;
        carSimulationComponent.IsBraking = isBraking;
        carSimulationComponent.IsDrifting = isDrifting;
    }
}