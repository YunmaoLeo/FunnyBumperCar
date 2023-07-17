using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarSimulation : MonoBehaviour
{
    [SerializeField] private float fixedDeltaTime = 0.005f;
    
    [SerializeField] private float maxEngineVelocity;
    [SerializeField] private float maxEngineVelocityCoefficient = 5f;
    [SerializeField] private float engineMaxTorque = 100f;

    [SerializeField] private TireDriveMode tireDriveMode;
    [SerializeField] private TireSteerMode tireSteerMode;

    [SerializeField] private float steerRotateTime = 0.2f;
    [SerializeField] private AnimationCurve engineTorqueCurve;

    [SerializeField] public Transform FrontLeftTire;
    [SerializeField] public Transform FrontRightTire;
    [SerializeField] public Transform BackLeftTire;
    [SerializeField] public Transform BackRightTire;

    [SerializeField] public Transform FrontLeftTireConnectionPoint;
    [SerializeField] public Transform FrontRightTireConnectionPoint;
    [SerializeField] public Transform BackLeftTireConnectionPoint;
    [SerializeField] public Transform BackRightTireConnectionPoint;

    private Dictionary<TireLocation, Transform> tireConnectPointsMap = new Dictionary<TireLocation, Transform>();
    private Dictionary<TireLocation, TirePhysics> tiresMap = new Dictionary<TireLocation, TirePhysics>();
    private Dictionary<TireLocation, bool> tiresAbleToSteerMap = new Dictionary<TireLocation, bool>();
    private Dictionary<TireLocation, bool> tiresAbleToDriveMap = new Dictionary<TireLocation, bool>();

    private float totalTireMass;
    
    private Rigidbody carRigidbody;

    private GameInputActions gameInputActions;

    private Vector3 carFrameSize;

    [Serializable]
    public enum TireDriveMode
    {
        FrontDrive,
        RearDrive,
        FourDrive
    }

    [Serializable]
    public enum TireSteerMode
    {
        FrontSteer,
        RearSteer,
        FourSteer
    }

    [Serializable]
    public enum TireLocation
    {
        FrontLeft = 0,
        FrontRight = 1,
        BackLeft = 2,
        BackRight = 3
    }

    private void OnEnable()
    {
        InitializeTires();
        maxEngineVelocity = engineMaxTorque / (carRigidbody.mass + totalTireMass) * maxEngineVelocityCoefficient;
    }

    private void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
        gameInputActions = new GameInputActions();
        gameInputActions.Player.Enable();

        InitializeTires();

        //precompute max engine velocity
        maxEngineVelocity = engineMaxTorque / (carRigidbody.mass + totalTireMass) * maxEngineVelocityCoefficient;
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        Time.fixedDeltaTime = fixedDeltaTime;
        CarTireSimulation();
    }

    private void CarTireSimulation()
    {
        Vector2 inputVector = gameInputActions.Player.Move.ReadValue<Vector2>();
        for (int i = (int)TireLocation.FrontLeft; i <= (int)TireLocation.BackRight; i++)
        {
            TireLocation tireLocation = (TireLocation)i;

            var tirePhysicsComponent = tiresMap[tireLocation];
            var tireConnectPoint = tireConnectPointsMap[tireLocation];
            var ableToDrive = tiresAbleToDriveMap[tireLocation];
            var ableToSteer = tiresAbleToSteerMap[tireLocation];

            bool isAssistSteerTire = tireLocation == TireLocation.BackLeft || tireLocation == TireLocation.BackRight;

            bool raycastResult = tirePhysicsComponent.SteerRaycast(tireConnectPoint, out float minRaycastDistance);

            tirePhysicsComponent.HandleTireVisual(carRigidbody);
            
            if (ableToSteer)
            {
                tirePhysicsComponent.SteerTireRotation(inputVector.x, transform, steerRotateTime, isAssistSteerTire);
            }
            
            if (!raycastResult)
            {
                continue;
            }

            tirePhysicsComponent.SimulateSuspensionSystem(tireConnectPoint, carRigidbody, minRaycastDistance, out Vector3 suspensionForceOnSpring);
            
            tirePhysicsComponent.SimulateSteeringForces(carRigidbody, maxEngineVelocity);
            
            tirePhysicsComponent.SimulateFriction(carRigidbody, suspensionForceOnSpring);

            if (ableToDrive)
            {
                float engineTorque = 0f;
                engineTorque = engineTorqueCurve.Evaluate(Math.Abs(Vector3.Dot(carRigidbody.velocity, carRigidbody.transform.forward) / maxEngineVelocity)) *
                               engineMaxTorque;
                tirePhysicsComponent.SimulateAccelerating(inputVector.y, carRigidbody, engineTorque);
            }
        }
    }

    private void InitializeTires()
    {
        ClearTireConfigMaps();

        tiresMap.Add(TireLocation.FrontLeft, FrontLeftTire.GetComponent<TirePhysics>());
        tiresMap.Add(TireLocation.FrontRight, FrontRightTire.GetComponent<TirePhysics>());
        tiresMap.Add(TireLocation.BackLeft, BackLeftTire.GetComponent<TirePhysics>());
        tiresMap.Add(TireLocation.BackRight, BackRightTire.GetComponent<TirePhysics>());

        tireConnectPointsMap.Add(TireLocation.FrontLeft, FrontLeftTireConnectionPoint);
        tireConnectPointsMap.Add(TireLocation.FrontRight, FrontRightTireConnectionPoint);
        tireConnectPointsMap.Add(TireLocation.BackLeft, BackLeftTireConnectionPoint);
        tireConnectPointsMap.Add(TireLocation.BackRight, BackRightTireConnectionPoint);

        tiresAbleToDriveMap.Add(TireLocation.FrontLeft, tireDriveMode != TireDriveMode.RearDrive);
        tiresAbleToDriveMap.Add(TireLocation.FrontRight, tireDriveMode != TireDriveMode.RearDrive);
        tiresAbleToDriveMap.Add(TireLocation.BackLeft, tireDriveMode != TireDriveMode.FrontDrive);
        tiresAbleToDriveMap.Add(TireLocation.BackRight, tireDriveMode != TireDriveMode.FrontDrive);

        tiresAbleToSteerMap.Add(TireLocation.FrontLeft, tireSteerMode != TireSteerMode.RearSteer);
        tiresAbleToSteerMap.Add(TireLocation.FrontRight, tireSteerMode != TireSteerMode.RearSteer);
        tiresAbleToSteerMap.Add(TireLocation.BackLeft, tireSteerMode != TireSteerMode.FrontSteer);
        tiresAbleToSteerMap.Add(TireLocation.BackRight, tireSteerMode != TireSteerMode.FrontSteer);

        totalTireMass = 0f;
        foreach (var pair in tiresMap)
        {
            totalTireMass += pair.Value.Mass;
        }
    }

    private void ClearTireConfigMaps()
    {
        tiresMap.Clear();
        tireConnectPointsMap.Clear();
        tiresAbleToDriveMap.Clear();
        tiresAbleToSteerMap.Clear();
    }
}