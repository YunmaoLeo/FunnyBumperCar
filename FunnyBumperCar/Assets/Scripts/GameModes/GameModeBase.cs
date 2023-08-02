using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameModeBase : MonoBehaviour
{
    [SerializeField] protected List<Transform> carSpawnPoints;
    [SerializeField] protected string modeName;
    private PlayerInputManager playerInputManager;
    [SerializeField] private Canvas GameOverUI;
    
    protected List<PlayerInput> playerInputs = new List<PlayerInput>();

    public static GameModeBase Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void Start()
    {
        SpawnCars();
        SpawnPlayers();
        InitializeGameMode();
    }


    protected virtual void InitializeGameMode()
    {
        
    }
    protected virtual void SpawnCars()
    {
        playerInputs.Add(playerInputManager.JoinPlayer(0, controlScheme: "Gamepad"));
        playerInputs.Add(playerInputManager.JoinPlayer(1, controlScheme: "Keyboard"));

        if (playerInputs[0] == null)
        {
            Debug.LogError("Gamepad Not Connected");
        }

        for (int index = 0; index < playerInputs.Count; index++)
        {
            playerInputs[index].transform.SetPositionAndRotation(carSpawnPoints[index].position, carSpawnPoints[index].rotation);
        }
    }

    protected virtual void OnGameOver()
    {
        Time.timeScale = 0.3f;
        GameOverUI.gameObject.SetActive(true);
    }

    protected virtual void SpawnPlayers()
    {
    }

    protected int GetPlayerOfIndex(List<PlayerBase> players, int playerIndex)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].PlayerIndex == playerIndex)
            {
                return i;
            }
        }
        return -1;
    }
}