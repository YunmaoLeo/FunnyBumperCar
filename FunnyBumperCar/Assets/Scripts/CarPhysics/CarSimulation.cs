using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarSimulation : MonoBehaviour, ICanBeExploded
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

    [SerializeField] public Transform CenterOfMass;
    [SerializeField] public Transform Parachute;

    private bool isDrifting = false;
    
    private Transform frontLeftTire;
    private Transform frontRightTire;
    private Transform backLeftTire;
    private Transform backRightTire;

    [SerializeField] public Transform FrontLeftTireConnectionPoint;
    [SerializeField] public Transform FrontRightTireConnectionPoint;
    [SerializeField] public Transform BackLeftTireConnectionPoint;
    [SerializeField] public Transform BackRightTireConnectionPoint;
    
    [SerializeField] private float carFlipOverMaxForceCoefficient = 30f;
    [SerializeField] private float carFlipOverMinForceCoefficient = 4f;
    [SerializeField] private float carFlipOverDragAngleLimitation = 120;
    [SerializeField] private float carFlipOverAngularVelocityLimitation = 1f;

    private bool isBraking;

    private bool isParachuteActive;

    private Dictionary<TireLocation, Transform> tireConnectPointsMap = new Dictionary<TireLocation, Transform>();
    private Dictionary<TireLocation, TirePhysics> tiresMap = new Dictionary<TireLocation, TirePhysics>();
    private Dictionary<TireLocation, bool> tiresAbleToSteerMap = new Dictionary<TireLocation, bool>();
    private Dictionary<TireLocation, bool> tiresAbleToDriveMap = new Dictionary<TireLocation, bool>();

    private float totalTireMass;
    
    private Rigidbody carRigidbody;
    
    private Vector3 carFrameSize;

    [SerializeField] private Transform WheelsTransform;

    [SerializeField] private Transform CarsAddonsTransform;

    public float TireRotateSignal { get; set; }

    public float CarDriveSignal { get; set; }

    public float MaxEngineVelocity
    {
        get => maxEngineVelocity;
    }
    
    public bool IsBraking
    {
        get => isBraking;
        set => isBraking = value;
    }

    public bool IsDrifting
    {
        get => isDrifting;
        set => isDrifting = value;
    }

    public float GetCarRadiusSize
    {
        get => (GetComponentInChildren<MeshRenderer>().bounds.extents.magnitude);
    }
    
    public bool IsParachuteActive
    {
        get => isParachuteActive;
        set => isParachuteActive = value;
    }

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
        carRigidbody.centerOfMass = CenterOfMass.localPosition;

        var parachute = Parachute.GetComponent<Parachute>();
        parachute.SetCarRigidbody(carRigidbody);
        parachute.SetCarSimulation(this);
        
        InitializeTires();
        
        //initialize car addons;
        InitializeCarAddons();
        
        //precompute max engine velocity
        maxEngineVelocity = engineMaxTorque / (carRigidbody.mass + totalTireMass) * maxEngineVelocityCoefficient;
    }

    private void ResetAddonStates(bool enable)
    {
        var addonsCount = CarsAddonsTransform.childCount;
        for (int index = 0; index < addonsCount; index++)
        {
            var addonSlot = CarsAddonsTransform.GetChild(index);
            var slot = addonSlot.GetComponent<AddonSlot>();
            if (slot.GetAddonContainer() != null)
            {
                slot.GetAddonContainer().SetEnable(enable);
            }
        }
    }

    public void ResetPhysicalState(Transform spawnPointTransform)
    {

        ResetAddonStates(false);
        carRigidbody.isKinematic = true;
        carRigidbody.detectCollisions = false;
        carRigidbody.velocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero; 
        carRigidbody.ResetInertiaTensor();
        carRigidbody.position = spawnPointTransform.position;
        carRigidbody.rotation = spawnPointTransform.rotation;

        StartCoroutine(ActivePhysicalBodyWithDelay(1f));
    }

    IEnumerator ActivePhysicalBodyWithDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        carRigidbody.isKinematic = false;
        carRigidbody.detectCollisions = true;
        ResetAddonStates(true);
    }

    private void InitializeCarAddons()
    {
        var addonsCount = CarsAddonsTransform.childCount;
        for (int index = 0; index < addonsCount; index++)
        {
            var addonSlot = CarsAddonsTransform.GetChild(index);
            addonSlot.GetComponent<AddonSlot>().InitializeCarAddon(carRigidbody);
        }
    }

    public void BindAddonInputActions(InputActionMap playerActionMap)
    {
        var addonsCount = CarsAddonsTransform.childCount;
        for (int index = 0; index < addonsCount; index++)
        {
            var addonSlot = CarsAddonsTransform.GetChild(index);
            var slot = addonSlot.GetComponent<AddonSlot>();
            if (slot.GetAddonContainer() == null)
            {
                continue;
            }

            string actionName = "";
            switch (slot.SlotType)
            {
                case AddonSlot.AddonSlotType.Top:
                    actionName = "CarAddonTriggerTop";
                    break;
                case AddonSlot.AddonSlotType.Front:
                    actionName = "CarAddonTriggerFront";
                    break;
                case AddonSlot.AddonSlotType.Side:
                    actionName = "CarAddonTriggerSide";
                    break;
                case AddonSlot.AddonSlotType.Back:
                    actionName = "CarAddonTriggerBack";
                    break;
                case AddonSlot.AddonSlotType.Bottom:
                    actionName = "CarAddonTriggerBottom";
                    break;
                
                default:
                    break;
            }

            if (actionName != "")
            {
                playerActionMap.FindAction(actionName).performed += slot.GetAddonContainer().TriggerAddon;
            }
        }

        playerActionMap.FindAction("ParachuteTrigger").performed += context => TurnOnAndOffParachute();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        Time.fixedDeltaTime = fixedDeltaTime;
        CarTireSimulation();
    }

    private void TurnOnAndOffParachute()
    {
        isParachuteActive = !isParachuteActive;

        var parachute = Parachute.GetComponent<Parachute>();
        if (isParachuteActive)
        {
            parachute.OpenParachute();
        }
        else
        {
            parachute.CloseParachute();
        }
    }

    private void CarTireSimulation()
    {
        int tiresContactToGroundCount = 4;
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
                tiresContactToGroundCount--;
                continue;
            }

            tirePhysicsComponent.SimulateSuspensionSystem(tireConnectPoint, carRigidbody, minRaycastDistance, out Vector3 suspensionForceOnSpring);
            
            tirePhysicsComponent.SimulateSteeringForces(carRigidbody, maxEngineVelocity, isDrifting);

            tirePhysicsComponent.IsBraking = isBraking;
            tirePhysicsComponent.SimulateFriction(carRigidbody, suspensionForceOnSpring);

            if (ableToDrive)
            {
                float engineTorque = 0f;
                engineTorque = engineTorqueCurve.Evaluate(Math.Abs(Vector3.Dot(carRigidbody.velocity, carRigidbody.transform.forward) / maxEngineVelocity)) *
                               engineMaxTorque;
                tirePhysicsComponent.SimulateAccelerating(CarDriveSignal, carRigidbody, engineTorque);
            }
        }

        if (tiresContactToGroundCount != 4)
        {
            RecoverCarWhenFlippedOver(tiresContactToGroundCount);
        }
    }

    private void RecoverCarWhenFlippedOver(int tiresContactToGroundCount)
    {
        if (carRigidbody.angularVelocity.magnitude > carFlipOverAngularVelocityLimitation)
        {
            return;
        }
        var angle = Mathf.Abs(Vector3.Angle(-transform.up, Vector3.up));
        if (angle < carFlipOverDragAngleLimitation)
        {
            var carFlipOverDragForceCoefficient = angle / carFlipOverDragAngleLimitation * carFlipOverMaxForceCoefficient + carFlipOverMinForceCoefficient;
            Vector3 dragForce = transform.up * (carRigidbody.mass * carFlipOverDragForceCoefficient);
            if (TireRotateSignal < 0f)
            {
                AddForceAndDrawLine(frontRightTire.transform.position, dragForce, Color.cyan);
                AddForceAndDrawLine(backRightTire.transform.position, dragForce, Color.cyan);
                AddForceAndDrawLine(frontLeftTire.transform.position, -dragForce, Color.cyan);
                AddForceAndDrawLine(backLeftTire.transform.position, -dragForce, Color.cyan);
            }
            else if (TireRotateSignal > 0f)
            {
                AddForceAndDrawLine(frontLeftTire.transform.position, dragForce, Color.cyan);
                AddForceAndDrawLine(backLeftTire.transform.position, dragForce, Color.cyan);
                AddForceAndDrawLine(frontRightTire.transform.position, -dragForce, Color.cyan);
                AddForceAndDrawLine(backRightTire.transform.position, -dragForce, Color.cyan);
            }
        }
    }
    private void AddForceAndDrawLine(Vector3 startPoint, Vector3 force, Color color, ForceMode forceMode = ForceMode.Force)
    {
        carRigidbody.AddForceAtPosition(force, startPoint, forceMode);
        Debug.DrawLine(startPoint,
            startPoint + force / carRigidbody.mass / 2f, color);
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

    public void BeExploded(Vector3 explosionCenter, float explosionIntensity, float explosionRadius)
    {
        carRigidbody.AddExplosionForce(explosionIntensity, explosionCenter, explosionRadius);
    }
}