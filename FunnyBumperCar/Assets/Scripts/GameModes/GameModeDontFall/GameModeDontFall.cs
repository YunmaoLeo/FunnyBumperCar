using System.Collections.Generic;
using UnityEngine;

public class GameModeDontFall : GameModeBase
{
    private List<PlayerDontFall> players = new List<PlayerDontFall>();
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
        foreach (var player in players)
        {
            if (player.playerInput.transform == null)
            {
                continue;
            }

            if (player.playerInput.transform == car.parent.transform)
            {
                player.RemainFallDownTime--;
                fallGameStateUI.UpdatePlayerRemainFallTime(players);
                if (player.RemainFallDownTime <= 0)
                {
                    OnGameOver();
                }
                else
                {
                    RespawnCar(car.GetComponent<CarSimulation>());
                }
            }
        }
    }

    private void RespawnCar(CarSimulation car)
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
        for (int i = 0; i < playerInputs.Count; i++)
        {
            players.Add(new PlayerDontFall(MaxFallDownTime));
            players[i].PlayerIndex = i;
            players[i].playerInput = playerInputs[i];
        }

        fallGameStateUI.gameObject.SetActive(true);
        fallGameStateUI.UpdatePlayerRemainFallTime(players);
    }
}