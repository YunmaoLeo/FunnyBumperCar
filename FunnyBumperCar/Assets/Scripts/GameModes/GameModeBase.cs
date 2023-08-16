using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameModeBase : MonoBehaviour
{
    [SerializeField] protected List<Transform> carSpawnPoints;
    [SerializeField] protected string modeName;
    private PlayerInputManager playerInputManager;
    [SerializeField] protected Canvas GameOverUI;
    
    protected List<Player> players = new List<Player>();

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
        foreach (var player in FindObjectsOfType<Player>())
        {
            players.Add(player);
            player.carTransform.SetPositionAndRotation(carSpawnPoints[player.playerIndex].position, carSpawnPoints[player.playerIndex].rotation);
            player.OnGameModeStart();
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