using System.Collections.Generic;
using UnityEngine;

public class GameModeFootball : GameModeBase
{
    public class PlayerFootball : PlayerBase
    {
        public int score = 0;
    }

    private List<PlayerFootball> playerFootballs = new List<PlayerFootball>();
    [SerializeField] private Transform footballPrefab;
    [SerializeField] private SoccerGoal goal1;
    [SerializeField] private SoccerGoal goal2;

    [SerializeField] private Transform footBallSpawnPosition;
    private Transform footBallInstance;

    private PlayerFootball player1;
    private PlayerFootball player2;

    protected override void InitializeGameMode()
    {
        base.InitializeGameMode();
        goal1.OnFootballInGoal += OnSoccerInGoal;
        goal2.OnFootballInGoal += OnSoccerInGoal;

        SpawnFootball();
    }

    private void SpawnFootball()
    {
        Destroy(footBallInstance);
        footBallInstance = Instantiate(footballPrefab, footBallSpawnPosition.position,
            footBallSpawnPosition.rotation);
    }

    private void OnSoccerInGoal(int playerIndex)
    {
        if (playerIndex == 0)
        {
            player2.score++;
        }
        else
        {
            player1.score++;
        }
    }

    protected override void SpawnPlayers()
    {
        player1 = new PlayerFootball();
        player2 = new PlayerFootball();
        player1.PlayerIndex = players[0].playerIndex;
        player2.PlayerIndex = players[1].playerIndex;
    }
}