using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestingCarMovingController : MonoBehaviour
{
    [SerializeField] private Transform[] frontSteeringTires;
    [SerializeField] private float steerRotateMaxAngle = 30f;
    [SerializeField] private float steerRotateTime = 0.2f;
    
    private Rigidbody carRigidbody;
    private PlayerInput playerInput;
    private GameInputActions gameInputActions;

    private void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        gameInputActions = new GameInputActions();
        gameInputActions.Player.Enable();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 inputVector = gameInputActions.Player.Move.ReadValue<Vector2>();


        Debug.Log("Input Vector x: " + inputVector.x +" y: " + inputVector.y);

        Vector3 carRigidbodyYawRotationEuler = new Vector3(0, carRigidbody.rotation.eulerAngles.y, 0);
        foreach (var tire in frontSteeringTires)
        {
            var tireRotation = tire.rotation;
            if (inputVector.x == 0)
            {
                Quaternion newQuaternion = Quaternion.Euler(carRigidbodyYawRotationEuler);
                tire.rotation = newQuaternion;
                // tire.DORotateQuaternion(newQuaternion, 0.02f);
                // tire.rotation = newQuaternion;
            }

            else if (inputVector.x < 0f)
            {
                Quaternion newQuaternion = Quaternion.Euler(carRigidbodyYawRotationEuler + new Vector3(0,-steerRotateMaxAngle,0));
                tire.DORotateQuaternion(newQuaternion, steerRotateTime);
                // tire.rotation = newQuaternion;
            }
            else
            {
                Quaternion newQuaternion = Quaternion.Euler(carRigidbodyYawRotationEuler + new Vector3(0,steerRotateMaxAngle,0));
                tire.DORotateQuaternion(newQuaternion, steerRotateTime);
                // tire.rotation = newQuaternion;
            }
        }
        
    }
}