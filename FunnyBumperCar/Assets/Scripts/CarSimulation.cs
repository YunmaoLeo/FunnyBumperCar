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

    [SerializeField] public Transform frontLeftTirePrefab;
    [SerializeField] public Transform frontRightTirePrefab;
    [SerializeField] public Transform backLeftTirePrefab;
    [SerializeField] public Transform backRightTirePrefab;

    private Transform frontLeftTire;
    private Transform frontRightTire;
    private Transform backLeftTire;
    private Transform backRightTire;

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
    
    private Vector3 carFrameSize;

    [SerializeField] private Transform WheelsTransform;
    
    public float TireRotateSignal { get; set; }

    public float CarDriveSignal { get; set; }

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
                tirePhysicsComponent.SteerTireRotation(TireRotateSignal, transform, steerRotateTime, isAssistSteerTire);
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
                tirePhysicsComponent.SimulateAccelerating(CarDriveSignal, carRigidbody, engineTorque);
            }
        }
    }

    private void InitializeTires()
    {
        ClearTireConfigMaps();

        frontLeftTire = Instantiate(frontLeftTirePrefab, WheelsTransform);
        frontRightTire = Instantiate(frontRightTirePrefab, WheelsTransform);
        backLeftTire = Instantiate(backLeftTirePrefab, WheelsTransform);
        backRightTire = Instantiate(backRightTirePrefab, WheelsTransform);

        
        tiresMap.Add(TireLocation.FrontLeft,frontLeftTire.GetComponent<TirePhysics>());
        tiresMap.Add(TireLocation.FrontRight, frontRightTire.GetComponent<TirePhysics>());
        tiresMap.Add(TireLocation.BackLeft, backLeftTire.GetComponent<TirePhysics>());
        tiresMap.Add(TireLocation.BackRight, backRightTire.GetComponent<TirePhysics>());

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
        foreach (var keyValuePair in tiresMap)
        {
            Destroy(keyValuePair.Value.gameObject);
        }
        
        tiresMap.Clear();
        tireConnectPointsMap.Clear();
        tiresAbleToDriveMap.Clear();
        tiresAbleToSteerMap.Clear();
    }
}