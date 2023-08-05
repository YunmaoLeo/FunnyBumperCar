using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform CarPrefab;

    private Transform carTransform;
    private CarBody _carBodyComponent;
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
        _carBodyComponent = carTransform.GetComponent<CarBody>();
        _carBodyComponent.BindAddonInputActions(player);

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
        _carBodyComponent.TireRotateSignal = moveInputVector.x;
        _carBodyComponent.CarDriveSignal = moveInputVector.y;
        _carBodyComponent.IsBraking = isBraking;
        _carBodyComponent.IsDrifting = isDrifting;
    }
}