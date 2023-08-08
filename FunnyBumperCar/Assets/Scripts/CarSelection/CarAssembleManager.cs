using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarAssembleManager : MonoBehaviour
{
    [SerializeField] private List<Transform> assemblePositionLists;

    private PlayerInputManager playerInputManager;
    protected List<PlayerInput> playerInputs = new List<PlayerInput>();
    public static CarAssembleManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void Start()
    {
        InitializePlayers();
    }

    private void InitializePlayers()
    {        
        playerInputs.Add(playerInputManager.JoinPlayer(0, controlScheme: "Gamepad"));
        playerInputs.Add(playerInputManager.JoinPlayer(1, controlScheme: "Keyboard"));
    }

    public Transform GetAssemblePositionForCar(int playerIndex)
    {
        return assemblePositionLists[playerIndex];
    }
}