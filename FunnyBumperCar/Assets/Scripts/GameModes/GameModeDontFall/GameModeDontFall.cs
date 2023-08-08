using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameModeDontFall : GameModeBase
{
    private List<PlayerDontFall> playerDontFalls = new List<PlayerDontFall>();
    [SerializeField] private FallDetection fallDetectionTransform;
    [SerializeField] private FallGameStateUI fallGameStateUI;

    public int MaxFallDownTime = 3;

    protected override void InitializeGameMode()
    {
        base.InitializeGameMode();
        fallDetectionTransform.OnCarFallDetected += OnPlayerCarFallDetected;
    }

    private void OnPlayerCarFallDetected(Transform car)
    {
        foreach (var player in playerDontFalls)
        {
            if (player.playerInput.transform == null)
            {
                continue;
            }
            if (player.PlayerIndex == car.root.GetComponent<CarBody>().PlayerIndex)
            {
                player.RemainFallDownTime--;
                fallGameStateUI.UpdatePlayerRemainFallTime(playerDontFalls);
                if (player.RemainFallDownTime <= 0)
                {
                    OnGameOver();
                }
                else
                {
                    RespawnCar(car.root.GetComponent<CarBody>());
                }
            }
        }
    }

    private void RespawnCar(CarBody car)
    {
        var randomPosition = carSpawnPoints[Random.Range(0, carSpawnPoints.Count)];
        foreach (var carSpawnPoint in carSpawnPoints)
        {
            bool hasDetected = false;
            Collider[] colliders = Physics.OverlapSphere(carSpawnPoint.position, car.GetCarRadiusSize * 2);
            foreach (var collider1 in colliders)
            {
                if (collider1.CompareTag("Car"))
                {
                    hasDetected = true;
                    break;
                }
            }

            if (!hasDetected)
            {
                randomPosition = carSpawnPoint;
            }
        }

        car.ResetPhysicalState(randomPosition);
    }

    protected override void OnGameOver()
    {
        base.OnGameOver();
        fallGameStateUI.gameObject.SetActive(false);
    }

    protected override void SpawnPlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            playerDontFalls.Add(new PlayerDontFall(MaxFallDownTime));
            playerDontFalls[i].PlayerIndex = players[i].playerIndex;
            playerDontFalls[i].playerInput = players[i].GetComponent<PlayerInput>();
        }

        fallGameStateUI.gameObject.SetActive(true);
        fallGameStateUI.UpdatePlayerRemainFallTime(playerDontFalls);
    }
}