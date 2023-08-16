using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameModeFootball : GameModeBase
{
    public class PlayerFootball : PlayerBase
    {
        public Transform CarTransform;
        public int score = 0;
    }

    [SerializeField] private int scoreToWinTheGame = 5;
    [SerializeField] private Transform cameraPrefab;
    private List<PlayerFootball> playerFootballs = new List<PlayerFootball>();
    [SerializeField] private Transform footballPrefab;
    [SerializeField] private SoccerGoal goal1;
    [SerializeField] private SoccerGoal goal2;
    [SerializeField] private Transform footBallSpawnPosition;
    [SerializeField] private CinemachineVirtualCamera focusBallCamera;
    [SerializeField] private float onSoccerInGoalWinShowTime = 3f;

    [SerializeField] private List<SoccerScoreBoard> scoreBoardList;
    private Transform footBallInstance;

    private bool isGameOver = false;
    
    private PlayerFootball player1;
    private PlayerFootball player2;

    protected override void InitializeGameMode()
    {
        base.InitializeGameMode();
        goal1.OnPlayerGetScore += OnPlayerGoaled;
        goal2.OnPlayerGetScore += OnPlayerGoaled;

        SpawnFootball();

        PlayerInputManager.instance.splitScreen = true;
    }

    private void SpawnFootball()
    {
        if (footBallInstance != null)
        {
            Destroy(footBallInstance.gameObject);
        }
        footBallInstance = Instantiate(footballPrefab, footBallSpawnPosition.position,
            footBallSpawnPosition.rotation);
        AssignFocusBallCamera();
    }

    private void RespawnCars()
    {
        players[0].carTransform.GetComponent<CarBody>().ResetPhysicalState(carSpawnPoints[players[0].playerIndex]);
        players[1].carTransform.GetComponent<CarBody>().ResetPhysicalState(carSpawnPoints[players[1].playerIndex]);
    }

    private void ControlFocusBallCamera(bool enable)
    {
        focusBallCamera.Priority = enable ? 100 : -1;
        

        if (enable)
        {
            PlayerInputManager.instance.splitScreen = false;
        }
        else
        {
            SetScreenSplit();
        }
    }
    

    private void AssignFocusBallCamera()
    {
        focusBallCamera.m_Follow = footBallInstance;
        focusBallCamera.m_LookAt = footBallInstance;
    }

    private void ResetGameState(float delay)
    {
        StartCoroutine(ResetGameStateCoroutine(delay));
    }

    IEnumerator ResetGameStateCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        //respawn football
        SpawnFootball();
        //respawn cars;
        RespawnCars();
        ControlFocusBallCamera(false);
    }

    private void OnPlayerGoaled(int playerIndex)
    {
        playerFootballs[playerIndex].score++;
        ControlFocusBallCamera(true);
        UpdateScoreBoard();
        
        CheckWinState();
        if (isGameOver)
        {
            OnGameOver();
        }
        else
        {
            ResetGameState(onSoccerInGoalWinShowTime);
        }
    }

    protected override void OnGameOver()
    {
        GameOverUI.GetComponent<FootballGameOverUI>().SetWinnerText(player1.score>player2.score ? player1.PlayerIndex : player2.PlayerIndex);
        base.OnGameOver();
    }

    private void CheckWinState()
    {
        if (player1.score >= scoreToWinTheGame || player2.score >= scoreToWinTheGame)
        {
            isGameOver = true;
        }
    }

    private void UpdateScoreBoard()
    {
        foreach (var soccerScoreBoard in scoreBoardList)
        {
            soccerScoreBoard.UpdateScore(
                player1.score,
                player2.score);
        }
    }

    protected override void SpawnPlayers()
    {
        player1 = new PlayerFootball();
        player2 = new PlayerFootball();
        playerFootballs.Add(player1);
        playerFootballs.Add(player2);

        var p1 = 0;
        var p2 = 1;
        if (players[p1].playerIndex != p1)
        {
            p1 = 1;
            p2 = 0;
        }
        player1.PlayerIndex = players[p1].playerIndex;
        player2.PlayerIndex = players[p2].playerIndex;
        player1.playerInput = players[p1].GetComponent<PlayerInput>();
        player2.playerInput = players[p2].GetComponent<PlayerInput>();
        player1.CarTransform = players[p1].carTransform;
        player2.CarTransform = players[p2].carTransform;
        
        var cameraInstance1 = Instantiate(cameraPrefab);
        var cameraInstance2 = Instantiate(cameraPrefab);
        player1.playerInput.camera = cameraInstance1.GetComponentInChildren<Camera>();
        player2.playerInput.camera = cameraInstance2.GetComponentInChildren<Camera>();

        int layerOfPlayer1 = (int)Mathf.Log(LayerMask.GetMask("Player1"), 2);
        int layerOfPlayer2 = (int)Mathf.Log(LayerMask.GetMask("Player2"), 2);
        
        player1.playerInput.camera.cullingMask = ~ (1 << layerOfPlayer2);
        player2.playerInput.camera.cullingMask = ~ (1 << layerOfPlayer1);

        var cm1 = cameraInstance1.GetComponentInChildren<CinemachineFreeLook>();
        cm1.gameObject.layer = layerOfPlayer1;
        
        var cm2 = cameraInstance2.GetComponentInChildren<CinemachineFreeLook>();
        cm2.gameObject.layer = layerOfPlayer2;
        
        var carBodyP1 = player1.CarTransform.GetComponent<CarBody>();
        var carBodyP2 = player2.CarTransform.GetComponent<CarBody>();
        carBodyP1.SetIndicatorLookAtCamera(player1.playerInput.camera);
        carBodyP2.SetIndicatorLookAtCamera(player2.playerInput.camera);

        carBodyP1.HidePlayerIndicatorUIWithDelay(4f);
        carBodyP2.HidePlayerIndicatorUIWithDelay(4f);

        cm1.m_Follow = player1.CarTransform;
        cm1.m_LookAt = player1.CarTransform;
        
        cm2.m_Follow = player2.CarTransform;
        cm2.m_LookAt = player2.CarTransform;
        
        SetScreenSplit();
        //
        var cm1InputHandler = cm1.GetComponent<FreeLookCameraInputHandler>();
        var cm2InputHandler = cm2.GetComponent<FreeLookCameraInputHandler>();

        cm1InputHandler.horizontal = player1.playerInput.actions.FindAction("Look");
        cm2InputHandler.horizontal = player2.playerInput.actions.FindAction("Look");

        if (player1.playerInput.currentControlScheme == "Keyboard")
        {
            cm1InputHandler.isMouse = true;
        }

        if (player2.playerInput.currentControlScheme == "Keyboard")
        {
            cm2InputHandler.isMouse = true;
        }
    }

    private void SetScreenSplit()
    {
        PlayerInputManager.instance.splitScreen = true;
        
        player1.playerInput.camera.rect = new Rect(0, 0, 1, 0.5f);
        player2.playerInput.camera.rect = new Rect(0, 0.5f, 1, 0.5f);
    }
}