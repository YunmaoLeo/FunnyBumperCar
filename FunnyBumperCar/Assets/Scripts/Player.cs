using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private CarAssembleController controllerPrefab;
    private CarAssembleController assembleController;
    public Transform carTransform;
    private CarBody carBody;
    private InputActionMap player;
    private InputActionMap selection;
    private bool isGamePlaying = false;

    public int playerIndex;

    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        player = playerInput.actions.FindActionMap("Player");
        selection = playerInput.actions.FindActionMap("Selection");
        playerIndex = playerInput.playerIndex;
    }

    private void Start()
    {
        var instance = Instantiate(controllerPrefab);
        assembleController = instance.GetComponent<CarAssembleController>();
        assembleController.carSpawnTransform = CarAssembleManager.Instance.GetAssemblePositionForCar(playerIndex);
        assembleController.player = this;
    }

    public void BindSelectionInputActions(SelectorListUI listUI)
    {
        selection.FindAction("MoveUp").performed += context => { listUI.OnCursorUp(); };
        selection.FindAction("MoveDown").performed += context => { listUI.OnCursorDown(); };
        selection.FindAction("MoveRight").performed += context => { listUI.OnCursorRight(); };
        selection.FindAction("MoveLeft").performed += context => { listUI.OnCursorLeft(); };
        selection.FindAction("Select").performed += context => { listUI.OnSelect(); };
        selection.FindAction("SelectDone").performed += context => { OnAssembleDone(); };
    }

    private void OnAssembleDone()
    {
        carTransform = assembleController.GetCar();
        DontDestroyOnLoad(carTransform);
        CarAssembleManager.Instance.OnCarAssembleStateChange(playerIndex);
        selection.Disable();
    }

    public void OnGameModeStart()
    {
        selection.Disable();
        player.Enable();
        isGamePlaying = true;
        carBody = carTransform.GetComponent<CarBody>();
        carBody.CarID = playerIndex;
        carBody.BindAddonInputActions(player);

        //initialize carManager;
        CarsAndCameraManager.Instance.RegisterCar(carTransform, playerIndex);
    }

    private void OnEnable()
    {
        player.Enable();
        selection.Enable();
    }

    private void FixedUpdate()
    {
        if (isGamePlaying)
        {
            var moveInputVector = player.FindAction("Move").ReadValue<Vector2>();
            var isBraking = player.FindAction("CarBrake").ReadValue<float>() != 0;
            var isDrifting = player.FindAction("Drifting").ReadValue<float>() != 0;
            carBody.TireRotateSignal = moveInputVector.x;
            carBody.CarDriveSignal = moveInputVector.y;
            carBody.IsBraking = isBraking;
            carBody.IsDrifting = isDrifting;
        }
    }
}