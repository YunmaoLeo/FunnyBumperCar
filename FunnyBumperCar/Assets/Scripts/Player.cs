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
        player.Disable();
        selection = playerInput.actions.FindActionMap("Selection"); 
        selection.Enable();
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
        selection.FindAction("MoveUp").performed += context =>
        {
            if (listUI == null) return;
            listUI.OnCursorUp();
        };
        selection.FindAction("MoveDown").performed += context =>
        {
            if (listUI == null) return;
            listUI.OnCursorDown();
        };
        selection.FindAction("MoveRight").performed += context =>
        {
            if (listUI == null) return;
            listUI.OnCursorRight();
        };
        selection.FindAction("MoveLeft").performed += context =>
        {
            if (listUI == null) return;
            listUI.OnCursorLeft();
        };
        selection.FindAction("Select").performed += context =>
        {
            if (listUI == null) return;
            listUI.OnSelect();
        };
        selection.FindAction("SelectDone").performed += context =>
        {
            if (listUI == null) return;
            OnAssembleDone();
        };
    }

    private void OnAssembleDone()
    {
        presentPlarform.RotateSignal = Vector2.zero;
        carTransform = assembleController.GetCar();

        assembleController.onAssembleDone();
        CarAssembleManager.Instance.OnCarAssembleStateChange(playerIndex, carTransform, selection);
    }

    private void OnDestroy()
    {
        selection.Disable();
        player.Disable();
    }

    public void OnGameModeStart()
    {
        selection.Disable();
        selection.RemoveAllBindingOverrides();
        player.Enable();
        isGamePlaying = true;
        carBody = carTransform.GetComponent<CarBody>();
        carBody.BindAddonInputActions(player);
        //initialize carManager;
        player.FindAction("StopGame").performed += CarsAndCameraManager.Instance.PauseGame;
        CarsAndCameraManager.Instance.RegisterCar(carTransform, playerIndex);
        SceneManager.MoveGameObjectToScene(carBody.gameObject, SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
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