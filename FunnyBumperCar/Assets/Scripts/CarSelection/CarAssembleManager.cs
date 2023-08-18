using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CarAssembleManager : MonoBehaviour
{
    [SerializeField] private List<CarPresentPlatform> assemblePositionLists;
    private List<Transform> readyCarList = new List<Transform>();
    private PlayerInputManager playerInputManager;
    protected List<PlayerInput> playerInputs = new List<PlayerInput>();
    private List<bool> playerAssembleDoneList = new List<bool>();
    private List<InputActionMap> selectionMaps = new List<InputActionMap>();

    [SerializeField] private Transform PlayerJoinHintText;
    [SerializeField] private Transform HelpTextUI;
    
    public static CarAssembleManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        playerInputManager = FindObjectOfType<PlayerInputManager>();
        playerInputManager.enabled = false;
        playerInputManager.enabled = true;
        DontDestroyOnLoad(playerInputManager);
    }

    private void Start()
    {
        // InitializePlayers();
    }

    public void OnCarAssembleStateChange(int playerIndex, Transform car, InputActionMap selectionMap )
    {
        playerAssembleDoneList[playerIndex] = !playerAssembleDoneList[playerIndex];
        readyCarList.Add(car);
        selectionMaps.Add(selectionMap);
        selectionMap.Disable();

        bool isAllDone = true;
        if (readyCarList.Count < 2)
        {
            return;
        }
        foreach (var b in playerAssembleDoneList)
        {
            if (!b)
            {
                isAllDone = false;
            }
        }

        if (isAllDone)
        {
            foreach (var transform1 in readyCarList)
            {
                transform1.SetParent(null);
                DontDestroyOnLoad(transform1);
            }
            
            foreach (var inputActionMap in selectionMaps)
            {
                inputActionMap.Disable();
            }

            LoadNewScene();
        }
    }

    [SerializeField] private string nextSceneName;

    private void LoadNewScene()
    {
        SceneManager.LoadSceneAsync(nextSceneName);
    }

    public void InitializePlayer(PlayerInput playerInput)
    {
        DontDestroyOnLoad(playerInput);
        playerInputs.Add(playerInput);
        playerAssembleDoneList.Add(false);
        if (playerInputs.Count == 2)
        {
            PlayerJoinHintText.gameObject.SetActive(false);
        }
        
        HelpTextUI.gameObject.SetActive(true);

    }

    private void InitializePlayers()
    {
        var player1 = playerInputManager.JoinPlayer(0, controlScheme: "Gamepad");
        var player2 = playerInputManager.JoinPlayer(1, controlScheme: "Keyboard");
        
        DontDestroyOnLoad(player1);
        DontDestroyOnLoad(player2);
        
        playerInputs.Add(player1);
        playerInputs.Add(player2);
        
        playerAssembleDoneList.Add(false);
        playerAssembleDoneList.Add(false);
    }

    public Transform GetAssemblePositionForCar(int playerIndex)
    {
        return assemblePositionLists[playerIndex].GetSpawnTransform();
    }

    public void SetCarOnPlatform(Transform car)
    {
        
    }
}