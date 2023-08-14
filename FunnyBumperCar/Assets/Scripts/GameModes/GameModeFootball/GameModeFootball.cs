using System;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameModeFootball : GameModeBase
{
    public class PlayerFootball : PlayerBase
    {
        public int score = 0;
    }

    [SerializeField] private Transform cameraPrefab;
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

        PlayerInputManager.instance.splitScreen = true;
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
        playerFootballs.Add(player1);
        playerFootballs.Add(player2);
        player1.PlayerIndex = players[0].playerIndex;
        player2.PlayerIndex = players[1].playerIndex;
        player1.playerInput = players[0].GetComponent<PlayerInput>();
        player2.playerInput = players[1].GetComponent<PlayerInput>();

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

        cm1.m_Follow = players[0].carTransform;
        cm1.m_LookAt = players[0].carTransform;

        
        cm2.m_Follow = players[1].carTransform;
        cm2.m_LookAt = players[1].carTransform;
        
        PlayerInputManager.instance.splitScreen = true;
        
        player1.playerInput.camera.rect = new Rect(0, 0, 1, 0.5f);
        player2.playerInput.camera.rect = new Rect(0, 0.5f, 1, 0.5f);
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
    

    private void FixedUpdate()
    {
        var lookValue = playerFootballs[1].playerInput.actions.FindAction("Look").ReadValue<Vector2>();

        var lookValue2 = playerFootballs[0].playerInput.actions.FindAction("Look").ReadValue<Vector2>();
        Debug.Log("LookValue of mouse: "+lookValue);
        Debug.Log("LookValue of gamepad: "+lookValue2);
    }
}