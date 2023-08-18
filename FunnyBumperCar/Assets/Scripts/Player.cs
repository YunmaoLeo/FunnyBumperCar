using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private CarAssembleController controllerPrefab;
    private CarAssembleController assembleController;
    public Transform carTransform;
    private CarBody carBody;
    private InputActionMap player;
    private InputActionMap selection;
    private bool isGamePlaying = false;
    private CarPresentPlatform presentPlarform;

    public int playerIndex;
    private PlayerInput playerInput;

    private void Awake()
    {
       playerInput = GetComponent<PlayerInput>();
        player = playerInput.actions.FindActionMap("Player");
        selection = playerInput.actions.FindActionMap("Selection");
        playerIndex = playerInput.playerIndex;
    }

    private void Start()
    {
        var instance = Instantiate(controllerPrefab);
        assembleController = instance.GetComponent<CarAssembleController>();
        assembleController.carSpawnTransform = CarAssembleManager.Instance.GetAssemblePositionForCar(playerIndex);
        presentPlarform = assembleController.carSpawnTransform.GetComponentInParent<CarPresentPlatform>();
        assembleController.player = this;
        
        CarAssembleManager.Instance.InitializePlayer(playerInput);
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
        presentPlarform.RotateSignal = Vector2.zero;
        carTransform = assembleController.GetCar();

        assembleController.onAssembleDone();
        CarAssembleManager.Instance.OnCarAssembleStateChange(playerIndex, carTransform, selection);
    }

    public void OnGameModeStart()
    {
        selection.Disable();
        player.Enable();
        isGamePlaying = true;
        carBody = carTransform.GetComponent<CarBody>();
        carBody.BindAddonInputActions(player);
        //initialize carManager;
        CarsAndCameraManager.Instance.RegisterCar(carTransform, playerIndex);
        SceneManager.MoveGameObjectToScene(carBody.gameObject, SceneManager.GetActiveScene());
    }

    private void OnEnable()
    {
        player.Enable();
        selection.Enable();
    }

    private void FixedUpdate()
    {
        if (selection.enabled)
        {
            var rotateSignal = selection.FindAction("PlatformRotation").ReadValue<Vector2>();
            presentPlarform.RotateSignal = rotateSignal;
        }
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