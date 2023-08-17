using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarBody : MonoBehaviour, ICanBeExploded
{
    public int CarID;
    public string CarName;
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
    [SerializeField] public Transform CameraLookAtPosition;

    [HideInInspector] public int PlayerIndex;
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

    [HideInInspector] public Rigidbody CarRigidbody;

    private Vector3 carFrameSize;

    [SerializeField] private Transform WheelsTransform;

    [SerializeField] private Transform CarsAddonsTransform;

    [SerializeField] private ConfigurableJoint leftDoorJoint;
    [SerializeField] private ConfigurableJoint rightDoorJoint;
    [SerializeField] private float doorMaxCollideStrength = 10f;

    [SerializeField] private PlayerIndicator PlayerIndicatorUIPrefab;
    private PlayerIndicator PlayerIndicatorUI;

    private List<AddonSlot> slotLists = new List<AddonSlot>();

    private List<GameObject> colliderObjects;
    private List<Collider> colliderList;
    private List<int> initColliderLayers;

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
        maxEngineVelocity = engineMaxTorque / (CarRigidbody.mass + totalTireMass) * maxEngineVelocityCoefficient;
    }

    private void Awake()
    {
        CarRigidbody = GetComponent<Rigidbody>();
        CarRigidbody.centerOfMass = CenterOfMass.localPosition;

        var parachute = Parachute.GetComponent<Parachute>();
        parachute.SetCarRigidbody(CarRigidbody);
        parachute.SetCarSimulation(this);

        InitializeTires();

        //initialize car addons;
        InitializeCarAddons();

        //precompute max engine velocity
        maxEngineVelocity = engineMaxTorque / (CarRigidbody.mass + totalTireMass) * maxEngineVelocityCoefficient;
    }

    private void Start()
    {
        // set initial tire position
        InitializeCarBodyCollider();
        InitializePlayerIndicatorUI();
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

    public void ResetPhysicalState(Transform spawnPointTransform, float delay = 1f)
    {
        ResetAddonStates(false);
        CarRigidbody.isKinematic = true;
        CarRigidbody.detectCollisions = false;
        CarRigidbody.velocity = Vector3.zero;
        CarRigidbody.angularVelocity = Vector3.zero;
        CarRigidbody.ResetInertiaTensor();
        CarRigidbody.position = spawnPointTransform.position;
        CarRigidbody.rotation = spawnPointTransform.rotation;

        StartCoroutine(ActivePhysicalBodyWithDelay(delay));
    }

    public void ResetPhysicalState()
    {
        CarRigidbody.velocity = Vector3.zero;
        CarRigidbody.angularVelocity = Vector3.zero;
        CarRigidbody.ResetInertiaTensor();
    }

    IEnumerator ActivePhysicalBodyWithDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        CarRigidbody.isKinematic = false;
        CarRigidbody.detectCollisions = true;
        ResetAddonStates(true);
    }

    private void InitializeCarAddons()
    {
        var addonsCount = CarsAddonsTransform.childCount;
        for (int index = 0; index < addonsCount; index++)
        {
            var addonSlot = CarsAddonsTransform.GetChild(index).GetComponent<AddonSlot>();
            slotLists.Add(addonSlot);
            addonSlot.InitializeCarAddon(this);
        }
    }

    public bool EquipCarAddon(AddonSlot.AddonSlotType slotType, Transform addonContainerPrefab)
    {
        var addonSlot = GetAddonSlot(slotType);
        if (addonSlot == null)
        {
            return false;
        }

        if (addonContainerPrefab == null)
        {
            return RemoveCarAddon(slotType);
        }

        if (addonSlot.GetAddonContainer() != null && addonSlot.GetAddonContainer().ContainerName ==
            addonContainerPrefab.GetComponent<AddonContainer_Car>().ContainerName)
        {
            return false;
        }

        if (addonSlot.GetAddonContainer() == null && addonContainerPrefab == null)
        {
            return false;
        }


        addonSlot.EquipSpecificCarAddon(this, addonContainerPrefab);

        InitializeCarBodyCollider();
        return true;
    }

    public bool RemoveCarAddon(AddonSlot.AddonSlotType slotType)
    {
        var addonSlot = GetAddonSlot(slotType);
        if (addonSlot == null)
        {
            return false;
        }

        if (addonSlot.SlotType == slotType)
        {
            return addonSlot.RemoveAddon(this);
        }

        return false;
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
                case AddonSlot.AddonSlotType.SideLeft:
                    actionName = "CarAddonTriggerSide";
                    break;
                case AddonSlot.AddonSlotType.SideRight:
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
                playerActionMap.FindAction(actionName).performed += slot.TriggerAddon;
            }
        }

        playerActionMap.FindAction("ParachuteTrigger").performed += context => TurnOnAndOffParachute();
    }

    private void Update()
    {
    }


    [SerializeField] private bool TestChangeCommand = false;
    [SerializeField] private float TestChangeCommandTargetValue = 0f;

    [SerializeField] private bool triggerInitializeCollider;
    private void FixedUpdate()
    {
        if (triggerInitializeCollider)
        {
            InitializeCarBodyCollider();
            triggerInitializeCollider = false;
        }
        Time.fixedDeltaTime = fixedDeltaTime;
        CarTireSimulation();

        if (TestChangeCommand)
        {
            TestChangeCommand = false;

            var addonSlot = GetAddonSlot(AddonSlot.AddonSlotType.SideRight);
            if (addonSlot != null)
            {
                foreach (var configSlideRangeCommand in addonSlot.GetAddon().ConfigFloatSlideRangeCommandsList)
                {
                    configSlideRangeCommand.OnValueLegallyChanged(TestChangeCommandTargetValue);
                    Debug.Log(configSlideRangeCommand.description);
                }
            }
        }
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
        SetCollidersToTrigger();
        for (int i = (int)TireLocation.FrontLeft; i <= (int)TireLocation.BackRight; i++)
        {
            TireLocation tireLocation = (TireLocation)i;

            var tirePhysics = tiresMap[tireLocation];
            var tireConnectPoint = tireConnectPointsMap[tireLocation];
            var ableToDrive = tiresAbleToDriveMap[tireLocation];
            var ableToSteer = tiresAbleToSteerMap[tireLocation];

            bool isAssistSteerTire = tireLocation == TireLocation.BackLeft || tireLocation == TireLocation.BackRight;


            bool isOnGround = tirePhysics.ColliderBasedRaycast(this, tireConnectPoint, out float minRaycastDistance);


            if (!isOnGround)
            {
                tiresContactToGroundCount--;
            }


            bool isHitAboveHalfTire = tirePhysics.DetectHitAboveHalfTire();

            if (isHitAboveHalfTire && isOnGround)
            {
                // is hit top half, active mesh collider of bottom half;
                // and we disable tire self adaption;
                tirePhysics.SetBottomHalfCollider(true);
            }
            else
            {
                // tirePhysics.SetBottomHalfCollider(tirePhysics.springOffset < 0f);
                tirePhysics.SetBottomHalfCollider(false);
            }
            
            tirePhysics.SimulateSuspensionSystem(tireConnectPoint, CarRigidbody, minRaycastDistance);

            if (ableToSteer)
            {
                tirePhysics.SteerTireRotation(TireRotateSignal, transform, steerRotateTime, isAssistSteerTire);
            }

            tirePhysics.StoreHitInfo(this);

            // set current engine Torque
            tirePhysics.motorTorque = ableToDrive ? CarDriveSignal * engineMaxTorque : 0f;
            tirePhysics.IsBraking = isBraking;

            tirePhysics.SimulateFriction(this);

            tirePhysics.ApplyForces(this);

            tirePhysics.AddInverseForceToHitPoint();
        }

        ResetColliders();

        if (tiresContactToGroundCount != 4)
        {
            RecoverCarWhenFlippedOver(tiresContactToGroundCount);
        }
    }

    private void RecoverCarWhenFlippedOver(int tiresContactToGroundCount)
    {
        if (CarRigidbody.angularVelocity.magnitude > carFlipOverAngularVelocityLimitation)
        {
            return;
        }

        var angle = Mathf.Abs(Vector3.Angle(-transform.up, Vector3.up));
        if (angle < carFlipOverDragAngleLimitation)
        {
            var carFlipOverDragForceCoefficient =
                angle / carFlipOverDragAngleLimitation * carFlipOverMaxForceCoefficient +
                carFlipOverMinForceCoefficient;
            Vector3 dragForce = transform.up * (CarRigidbody.mass * carFlipOverDragForceCoefficient);
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

    private void AddForceAndDrawLine(Vector3 startPoint, Vector3 force, Color color,
        ForceMode forceMode = ForceMode.Force)
    {
        CarRigidbody.AddForceAtPosition(force, startPoint, forceMode);
        Debug.DrawLine(startPoint,
            startPoint + force / CarRigidbody.mass / 2f, color);
    }

    public bool SetTireAndInstantiate(TireLocation location, Transform tireTransformPrefab)
    {
        if (tireTransformPrefab == null)
        {
            return false;
        }

        if (tireTransformPrefab.GetComponent<TirePhysics>().TireName == tiresMap[location].TireName)
        {
            return false;
        }

        var oldTire = tiresMap[location];
        var tireTransformInstance = Instantiate(tireTransformPrefab, WheelsTransform);

        switch (location)
        {
            case TireLocation.FrontLeft:
                frontLeftTire = tireTransformInstance;
                break;
            case TireLocation.FrontRight:
                frontRightTire = tireTransformInstance;
                break;
            case TireLocation.BackLeft:
                backLeftTire = tireTransformInstance;
                break;
            case TireLocation.BackRight:
                backRightTire = tireTransformInstance;
                break;
        }

        tiresMap[location] = tireTransformInstance.GetComponent<TirePhysics>();
        Destroy(oldTire.gameObject);
        tiresMap[location].InitializeTirePosition(tireConnectPointsMap[location], CarRigidbody);

        InitializeCarBodyCollider();
        return true;
    }


    private void InitializeTires()
    {
        ClearTireConfigMaps();

        frontLeftTire = Instantiate(frontLeftTirePrefab, WheelsTransform);
        frontRightTire = Instantiate(frontRightTirePrefab, WheelsTransform);
        backLeftTire = Instantiate(backLeftTirePrefab, WheelsTransform);
        backRightTire = Instantiate(backRightTirePrefab, WheelsTransform);


        tiresMap.Add(TireLocation.FrontLeft, frontLeftTire.GetComponent<TirePhysics>());
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

        InitializeTirePosition();

        totalTireMass = 0f;
        foreach (var pair in tiresMap)
        {
            totalTireMass += pair.Value.Mass;
        }
    }

    private void InitializeTirePosition()
    {
        foreach (var pair in tiresMap)
        {
            pair.Value.InitializeTirePosition(tireConnectPointsMap[pair.Key], CarRigidbody);
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
        CarRigidbody.AddExplosionForce(explosionIntensity, explosionCenter, explosionRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            float strength = 0;
            strength = collision.impulse.magnitude;

            if (VisualEffectManager.Instance != null)
            {
                VisualEffectManager.Instance.PlayCarCrashEffectLists(collision.contacts[0].point, strength);
            }
        }

        HandleCollisionDestroy(collision);
    }

    private void HandleCollisionDestroy(Collision collision)
    {
        float collideStrength = collision.relativeVelocity.magnitude;
        var collidePoint = collision.contacts[0].point;

        // handle door destroy
        HandleDoorCollisionDestroy(collideStrength, collidePoint);
    }

    private void HandleDoorCollisionDestroy(float collideStrength, Vector3 collidePoint)
    {
        if (leftDoorJoint == null || rightDoorJoint == null)
        {
            return;
        }

        if (collideStrength > doorMaxCollideStrength)
        {
            leftDoorJoint.angularXMotion = ConfigurableJointMotion.Limited;
            rightDoorJoint.angularXMotion = ConfigurableJointMotion.Limited;
        }
    }

    private AddonSlot GetAddonSlot(AddonSlot.AddonSlotType type)
    {
        foreach (var addonSlot in slotLists)
        {
            if (addonSlot.SlotType == type)
            {
                return addonSlot;
            }
        }

        return null;
    }

    private void InitializeCarBodyCollider()
    {
        colliderObjects = new List<GameObject>();
        initColliderLayers = new List<int>();
        colliderList = new List<Collider>();

        Collider[] colliders = GetComponentsInChildren<Collider>(true);
        foreach (var c in colliders)
        {
            GameObject colliderObject = c.gameObject;
            
            
            if (colliderObject.layer == 2)
            {
                continue;
            }
            colliderObjects.Add(colliderObject);
            initColliderLayers.Add(colliderObject.layer);
        }
    }

    private void SetCollidersToTrigger()
    {
        foreach (var colliderObject in colliderObjects)
        {
            if (colliderObject != null)
            {
                colliderObject.layer = 2;
            }
        }
        
    }

    private void ResetColliders()
    {
        for (int i = 0; i < colliderObjects.Count; i++)
        {
            if (colliderObjects[i] != null)
            {
                colliderObjects[i].layer = initColliderLayers[i];
            }
        }
    }

    public Camera playerIndicatorTargetCamera;

    public void InitializePlayerIndicatorUI()
    {
        PlayerIndicatorUI = Instantiate(PlayerIndicatorUIPrefab, transform);
        PlayerIndicatorUI.GetComponentInChildren<TextMeshProUGUI>().text = "Player" + (PlayerIndex + 1).ToString();
        SetIndicatorLookAtCamera(playerIndicatorTargetCamera);
    }

    public void ShowPlayerIndicatorUI()
    {
        PlayerIndicatorUI.ShowUI();
    }

    public void HidePlayerIndicatorUI()
    {
        PlayerIndicatorUI.HideUI();
        ;
    }


    public void SetIndicatorLookAtCamera(Camera targetCamera)
    {
        PlayerIndicatorUI.SetLookAtCamera(targetCamera);
    }

    public void HidePlayerIndicatorUIWithDelay(float f)
    {
        PlayerIndicatorUI.HideWithDelay(f);
    }
}