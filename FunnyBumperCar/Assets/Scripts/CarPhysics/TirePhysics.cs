using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TirePhysics : MonoBehaviour
{
    [Header("Basic Properties")] public int TireID;
    public string TireName;

    [SerializeField] private float tireWidth;
    [SerializeField] private float tireMass;
    [SerializeField] private float tireRadius;

    [Space] [Header("Suspension Properties")] [SerializeField]
    private float springStrength;

    [SerializeField] private float springDamping;

    [SerializeField] private float springDefaultLength;

    [SerializeField] private float springMaxLength;

    [SerializeField] private bool isBraking = false;
    [SerializeField] private float springMinLength;


    [SerializeField] private float sphereCastPrecision = 0.1f;

    [Space] [Header("Steering Properties")] [SerializeField]
    private float frictionCoefficient;

    [SerializeField] private float driftingTireGripFactor = 0.1f;
    [SerializeField] private AnimationCurve steeringCurve;

    [SerializeField] [Range(-180f, 180f)] private float steerAngle;

    [SerializeField] private float assistSteerRatio = 0.05f;
    [SerializeField] private float brakeFrictionMultiplier = 10f;

    private float tireGripFactor;
    private Dictionary<Collider, bool> isColliderSameCarDict = new Dictionary<Collider, bool>();


    private TireVisual tireVisual;

    [Space] [Header("FrictionBasedDriving")] [SerializeField]
    private float tireForwardGripFactor = 1.2f;

    [SerializeField] private float tireLateralGripFactor = 2f;

    private float forwardFrictionForce;

    private float lateralFrictionForce;

    private float hitPointForwardSlip;
    private float hitPointLateralSlip;

    private float forwardFrictionGrip = 1f;
    private float forwardFrictionStiffness = 1f;
    private float lateralFrictionGrip = 1f;
    private float lateralFrictionStiffness = 1f;


    // current hit point related info;
    private TireUtils.HitPointInfo _currentHitPoint = default;

    // is tire contact to the ground;
    private bool isContactToGround = false;

    // current suspension force to car;
    private Vector3 suspensionForce;
    private float springOffsetRate;
    private float suspensionScalarForce;
    private Vector3 overallFrictionForce;
    private float inertia;
    [SerializeField] private float defaultMaxBrakeTorque = 800f;

    [HideInInspector] public float motorTorque;
    private float motorForce;

    private float scalarSteerBrakeTorque;
    private float overallBrakeTorqueOnGround;
    private float overallBrakeTorqueOnAir;
    private float signedOverallBrakeTorqueOnGround;
    private float overallBrakeForce;
    private float signedOverallBrakeForceOnGround;
    private float brakeSign;

    private float signedOverallTireForwardForce;
    private float absOverallTireForwardForce;
    private float overallTireForwardForceSign;

    private float signedOverallBrakeForceOnTireGear;
    private float signedOverallForceOnTireGear;
    
    float sideSlipSign;
    float peakSideFrictionForce;
    float sideForce;

    private float carMass;

    private float maxForwardFrictionForceRelativeSlideProvided;

    private float maxForwardFrictionGroundProvided;

    private float angularVelocity;
    private float relativeAngularVelocityToGround;


    private float maxForwardForce;

    private const float HALF = 0.5f;


    [Space] [Header("Debug Properties")] [SerializeField]
    private bool drawRaycastDebugLine = false;


    public float Mass
    {
        get => tireMass;
        set => tireMass = value;
    }

    public bool IsBraking
    {
        get => isBraking;
        set => isBraking = value;
    }

    private Rigidbody tireRb;

    private void Awake()
    {
        tireVisual = GetComponent<TireVisual>();
        tireVisual.Radius = tireRadius;
        tireRb = GetComponent<Rigidbody>();
    }

    /**
     * Steer tire rotation according to the control signal;
     * we control the whole tire transform along with mesh collider and visual tire;
     */
    public void SteerTireRotation(float controlSignal, Transform carFrame, float steerRotateTime, bool isAssistTire)
    {
        var steerRotateAngle = steerAngle * (isAssistTire ? -assistSteerRatio : 1f);
        if (controlSignal == 0)
        {
            Quaternion newQuaternion = carFrame.rotation;
            transform.DORotateQuaternion(newQuaternion, steerRotateTime / 5f);
        }

        else if (controlSignal < 0f)
        {
            Quaternion newQuaternion =
                Quaternion.Euler(carFrame.rotation.eulerAngles + new Vector3(0, -steerRotateAngle, 0));
            transform.DORotateQuaternion(newQuaternion, steerRotateTime);
        }
        else
        {
            Quaternion newQuaternion =
                Quaternion.Euler(carFrame.rotation.eulerAngles + new Vector3(0, steerRotateAngle, 0));
            transform.DORotateQuaternion(newQuaternion, steerRotateTime);
        }
    }

    /**
     * 1. Simulate Suspension Force and add on carRigidbody;
     * 2. Update Tire Position According to the suspension system.
     * 3. Check if hit point is higher the current tire position.
     */
    public void SimulateSuspensionSystem(Transform tireConnectPoint, Rigidbody carRigidbody, float minRaycastDistance)
    {
        if (!isContactToGround)
        {
            suspensionScalarForce = 0;
            suspensionForce = Vector3.zero;
            return;
        }

        var springDirection = tireConnectPoint.up;
        var connectPointPos = tireConnectPoint.position;
        var wheelWorldVelocity = carRigidbody.GetPointVelocity(connectPointPos);

        var suspensionRestDist = springDefaultLength;

        float offset = springDefaultLength - (springMinLength + minRaycastDistance);

        float springVelocity = Vector3.Dot(springDirection, wheelWorldVelocity);

        suspensionScalarForce = (offset * springStrength) - (springVelocity * springDamping);
        springOffsetRate = offset / (springMaxLength - springMinLength);

        Debug.DrawLine(connectPointPos,
            connectPointPos + springDirection * suspensionScalarForce / carRigidbody.mass / 2f, Color.blue);

        suspensionForce = springDirection * suspensionScalarForce;

        suspensionScalarForce = Mathf.Max(suspensionScalarForce, 0f);
        Vector3 wheelPosOffset =
            -springDirection * (minRaycastDistance + springMinLength);

        //update tire position;
        var tirePosition = connectPointPos + wheelPosOffset;
        carRigidbody.AddForceAtPosition(suspensionForce,
            connectPointPos, ForceMode.Force);

        transform.position = tirePosition;
    }

    private void CalculateFrictionMaxProvided()
    {
        // F = m * a * contribution factor;
        maxForwardFrictionForceRelativeSlideProvided =
            carMass * 0.25f * Mathf.Abs(_currentHitPoint.hitPointForwardSpeed) / Time.fixedDeltaTime;
        // compute max forward friction force that ground can provide;
        maxForwardFrictionGroundProvided =
            frictionPeak * suspensionScalarForce * forwardLoadCoeff * forwardFrictionGrip;
        maxForwardForce = Math.Min(maxForwardFrictionGroundProvided, maxForwardFrictionForceRelativeSlideProvided);
    }

    public void InitializeTirePosition(Transform tireConnectPoint, Rigidbody carRigidbody)
    {
        var wheelPosOffset = -carRigidbody.transform.up * springDefaultLength;
        var tirePosition = tireConnectPoint.position + wheelPosOffset;
        transform.position = tirePosition;
    }

    public void SimulateSteeringForces(Rigidbody carRigidbody, float maxEngineVelocity, bool isDrifting = false)
    {
        var tireTransform = transform.position;
        Vector3 steeringDirection = transform.right;

        Vector3 wheelVelocity = carRigidbody.GetPointVelocity(tireTransform);
        float wheelForwardVelocity = Vector3.Dot(wheelVelocity, transform.forward);

        float steeringVelocity = Vector3.Dot(wheelVelocity, steeringDirection);


        tireGripFactor = !isDrifting
            ? steeringCurve.Evaluate(Math.Abs(wheelForwardVelocity / maxEngineVelocity))
            : driftingTireGripFactor;

        float expectedVelChange = -steeringVelocity * tireGripFactor;

        float desiredAccel = expectedVelChange / Time.fixedDeltaTime;

        var steeringForce = steeringDirection * (tireMass * desiredAccel);
        
        lateralFrictionForce += tireMass * desiredAccel;

        Debug.DrawLine(tireTransform, tireTransform + steeringForce / carRigidbody.mass, Color.red, 0f, false);
    }

    public void SimulateAccelerating(float controlSignal, Rigidbody carRigidbody, float engineTorque)
    {
        if (controlSignal == 0) return;
        var tirePosition = transform.position;
        Vector3 forwardDir = carRigidbody.transform.forward;
        carRigidbody.AddForceAtPosition(forwardDir * (engineTorque * controlSignal), tirePosition);

        Debug.DrawLine(tirePosition,
            tirePosition + forwardDir * (engineTorque * controlSignal) / carRigidbody.mass / 2f, Color.magenta);
    }

    public void HandleTireVisual(Rigidbody carRigidbody)
    {
        // update tire visual effects;
        tireVisual.WorldVelocity = Vector3.Dot(carRigidbody.GetPointVelocity(transform.position),
            transform.forward);
    }


    private void Start()
    {
        CalculateRotateInertia();
    }

    private void CalculateRotateInertia()
    {
        // I = M * r^2
        inertia = tireMass * tireRadius * tireRadius;
    }

    public void StoreHitInfo(CarBody carBody)
    {
        // store hit point infos;
        if (isContactToGround)
        {
            var raycastHit = _currentHitPoint.raycastHit;

            //calculate contact facet friction grip;
            forwardFrictionGrip = Mathf.Sqrt(raycastHit.collider.material.dynamicFriction * tireForwardGripFactor);
            lateralFrictionGrip = Mathf.Sqrt(raycastHit.collider.material.dynamicFriction * tireLateralGripFactor);

            contactFaceCausedBrakeTorque =
                baseFaceCausedBrakeTorque * (1 + raycastHit.collider.material.staticFriction);

            //calculate relative direction and speed;
            _currentHitPoint.hitPointForwardDirection =
                Vector3.Normalize(Vector3.Cross(raycastHit.normal, -transform.right));
            _currentHitPoint.hitPointLateralDirection = transform.right;
            _currentHitPoint.hitPointVelocityOnTire = carBody.CarRigidbody.GetPointVelocity(raycastHit.point);

            if (_currentHitPoint.rb != null)
            {
                _currentHitPoint.hitPointVelocityOnOther = _currentHitPoint.rb.GetPointVelocity(raycastHit.point);
                _currentHitPoint.hitPointVelocityOnTire -= _currentHitPoint.hitPointVelocityOnOther;
            }
            else
            {
                _currentHitPoint.hitPointVelocityOnOther = Vector3.zero;
            }

            _currentHitPoint.hitPointForwardSpeed =
                Vector3.Dot(_currentHitPoint.hitPointVelocityOnTire, _currentHitPoint.hitPointForwardDirection);

            _currentHitPoint.hitPointLateralSpeed =
                Vector3.Dot(_currentHitPoint.hitPointVelocityOnTire, _currentHitPoint.hitPointLateralDirection);
        }
        else
        {
            contactFaceCausedBrakeTorque = 0f;
            _currentHitPoint.hitPointForwardSpeed = 0f;
            _currentHitPoint.hitPointLateralSpeed = 0f;
        }
    }


    public bool ColliderBasedRaycast(CarBody carBody, Transform tireConnectPoint, out float minRaycastDistance)
    {
        bool raycastResult = false;
        minRaycastDistance = Single.MaxValue;

        var origin = tireConnectPoint.position + springMinLength * (-tireConnectPoint.up);
        var direction = -tireConnectPoint.up;

        var initialPosition = transform.position;
        var rbPosition = tireRb.position;

        // let sphere cast do a filter job;
        bool sphereCastResult = Physics.SphereCast(origin, tireRadius, direction, out var raycastHit,
            springMaxLength - springMinLength);
        if (!sphereCastResult)
        {
            isContactToGround = false;
            return false;
        }

        tireRb.position = origin;
        transform.position = origin;
        raycastResult = tireRb.SweepTest(direction, out raycastHit,
            springMaxLength - springMinLength, QueryTriggerInteraction.Ignore);

        transform.position = initialPosition;
        tireRb.position = initialPosition;
        if (!raycastResult)
        {
            isContactToGround = false;
            return false;
        }

        // check whether this collider is the sub object of this car;
        if (isColliderSameCarDict.ContainsKey(raycastHit.collider))
        {
            if (isColliderSameCarDict[raycastHit.collider])
            {
                raycastResult = false;
            }
        }
        else
        {
            var possibleCarBody = raycastHit.collider.transform.GetComponentInParent<CarBody>();
            if (possibleCarBody != null)
            {
                if (possibleCarBody == carBody)
                {
                    isColliderSameCarDict[raycastHit.collider] = true;
                    raycastResult = false;
                }
                else
                {
                    isColliderSameCarDict[raycastHit.collider] = false;
                }
            }
            else
            {
                isColliderSameCarDict[raycastHit.collider] = false;
            }
        }

        if (!raycastResult)
        {
            _currentHitPoint.rb = null;
            _currentHitPoint.raycastHit = default;
            isContactToGround = false;
            return false;
        }

        _currentHitPoint.raycastHit = raycastHit;

        _currentHitPoint.rb = raycastHit.collider.attachedRigidbody;

        float rayPointOffset = raycastHit.distance;

        if (minRaycastDistance > rayPointOffset)
        {
            minRaycastDistance = rayPointOffset;
        }

        isContactToGround = true;
        return true;
    }

    public void AddInverseForceToHitPoint()
    {
        if (isContactToGround)
        {
            if (_currentHitPoint.rb != null)
            {
                Vector3 allForce;
                allForce = -(suspensionForce + overallFrictionForce);
                _currentHitPoint.rb.AddForceAtPosition(allForce, _currentHitPoint.raycastHit.point);
            }
        }
    }

    public bool DetectHitAboveHalfTire()
    {
        return Vector3.Dot(transform.position - _currentHitPoint.raycastHit.point, transform.up) < 0;
    }

    [SerializeField] private float distanceToApplyFriction = 0.5f;
    [SerializeField] private float frictionPeak = 0.15f;

    [SerializeField] private float forwardLoadCoeff = 1.2f;
    [SerializeField] private float sideLoadCoeff = 2f;

    [SerializeField] private float gearDamperCausedBrakeTorque = 30f;

    [SerializeField] private float baseFaceCausedBrakeTorque = 20f;
    private float contactFaceCausedBrakeTorque = 20f;


    public void SimulateFriction(CarBody carBody)
    {
        //reset friction force;
        forwardFrictionForce = 0;
        lateralFrictionForce = 0;

        carMass = carBody.CarRigidbody.mass;
        motorForce = motorTorque / tireRadius;

        CalculateFrictionMaxProvided();
        CalculateBrakeAndEngineTorques();

        forwardFrictionForce = CalculateForwardFrictionForce();
        forwardFrictionForce = Mathf.Clamp(forwardFrictionForce, -maxForwardFrictionGroundProvided,
            +maxForwardFrictionGroundProvided);

        CalculateAngularVelocity();

        CalculateTireSideFriction(carBody);

    }

    private void CalculateBrakeAndEngineTorques()
    {
        //brake torque = proactive torque + gearDamperCausedTorque + environment cause torque;
        scalarSteerBrakeTorque = defaultMaxBrakeTorque * (isBraking ? 1f : 0f);
        overallBrakeTorqueOnGround =
            scalarSteerBrakeTorque + gearDamperCausedBrakeTorque + contactFaceCausedBrakeTorque;
        overallBrakeForce = overallBrakeTorqueOnGround / tireRadius;

        overallBrakeTorqueOnAir = scalarSteerBrakeTorque + gearDamperCausedBrakeTorque;
        brakeSign = -GetDirection(_currentHitPoint.hitPointForwardSpeed);

        signedOverallBrakeTorqueOnGround = overallBrakeTorqueOnGround * brakeSign;
        signedOverallBrakeForceOnGround = signedOverallBrakeTorqueOnGround / tireRadius;

        // calculate overall forward force = motor + all brake torque force
        signedOverallTireForwardForce = motorForce + signedOverallBrakeForceOnGround;
        absOverallTireForwardForce = Mathf.Abs(signedOverallTireForwardForce);

        float tireBrakeSign = -GetDirection(angularVelocity);
        signedOverallBrakeForceOnTireGear = overallBrakeTorqueOnGround * tireBrakeSign;
        signedOverallForceOnTireGear = motorForce + signedOverallBrakeForceOnTireGear;

        overallTireForwardForceSign = GetDirection(signedOverallTireForwardForce);
    }

    private float CalculateForwardFrictionForce()
    {
        maxForwardForce = Mathf.Abs(motorTorque) < scalarSteerBrakeTorque
            ? maxForwardForce
            : maxForwardFrictionGroundProvided;
        return Mathf.Clamp(signedOverallTireForwardForce, -maxForwardForce, maxForwardForce);
    }

    private float GetDirection(float value)
    {
        return value > 0 ? 1f : -1f;
    }

    public void ApplyForces(CarBody carBody)
    {
        //apply forces;
        if (isContactToGround)
        {
            overallFrictionForce =
                _currentHitPoint.hitPointForwardDirection * forwardFrictionForce
                + _currentHitPoint.hitPointLateralDirection * lateralFrictionForce;
            Vector3 forcePosition;
            forcePosition = _currentHitPoint.raycastHit.point +
                            transform.up * (springDefaultLength * distanceToApplyFriction);
            carBody.CarRigidbody.AddForceAtPosition(overallFrictionForce, forcePosition);
            Debug.DrawLine(forcePosition,
                forcePosition + _currentHitPoint.hitPointLateralDirection * lateralFrictionForce / 400f, Color.red);
            Debug.DrawLine(forcePosition,
                forcePosition + _currentHitPoint.hitPointForwardDirection * forwardFrictionForce / 400f, Color.green);
        }
        else
        {
            overallFrictionForce = Vector3.zero;
        }
    }

    /**
     *      // F = ma = mdv/dt;
            // F = a * I;
            // a = F / I;
            // delta w = a * t;
            // delta w = F / I * t;
            // F = deltaW * I / t;
     */
    private float GetForceForDeltaAngularV(float angularV, float tireInertia, float time)
    {
        return angularV * tireInertia / time;
    }

    private float GetDeltaAngularVForForce(float force, float tireInertia, float time)
    {
        return force / tireInertia * time;
    }

    private void CalculateAngularVelocity()
    {
        if (isContactToGround)
        {
            // when forward force is going to reverse the tire rotation;
            if (signedOverallForceOnTireGear * angularVelocity <= 0)
            {
                float minForceToStopTire =
                    GetForceForDeltaAngularV(Mathf.Abs(angularVelocity), inertia, Time.fixedDeltaTime);
                if (Mathf.Abs(signedOverallForceOnTireGear) > minForceToStopTire)
                {
                    signedOverallForceOnTireGear = Mathf.Clamp(signedOverallForceOnTireGear, -minForceToStopTire,
                        minForceToStopTire);
                }
            }

            angularVelocity += GetDeltaAngularVForForce(signedOverallForceOnTireGear, inertia, Time.fixedDeltaTime);

            // 1. surface and tire has relatively different speed;
            // 2. friction of surface and tire are constantly trying to make their relative velocity 0;
            relativeAngularVelocityToGround = _currentHitPoint.hitPointForwardSpeed / tireRadius;
            float angularVelocityDiff = angularVelocity - relativeAngularVelocityToGround;
            float angularVelocityFixForce =
                GetForceForDeltaAngularV(-angularVelocityDiff, inertia, Time.fixedDeltaTime);
            angularVelocityFixForce = Mathf.Clamp(angularVelocityFixForce, -maxForwardForce, maxForwardForce);
            angularVelocity += GetDeltaAngularVForForce(angularVelocityFixForce, inertia, Time.fixedDeltaTime);
        }
        else
        {
            //max brake torque = torqueMakeWheelStop + motorTorque;
            float maxBrakeTorque =
                Mathf.Abs(GetForceForDeltaAngularV(angularVelocity, inertia, Time.fixedDeltaTime) * tireRadius + motorTorque);
            float tireTorque = Mathf.Clamp(overallBrakeTorqueOnAir, -maxBrakeTorque, maxBrakeTorque);
            angularVelocity +=
                GetDeltaAngularVForForce((motorTorque - GetDirection(angularVelocity) * tireTorque) / tireRadius, inertia,
                    Time.fixedDeltaTime);
        }

        tireVisual.UpdateRotation(angularVelocity);
    }


    private void CalculateTireSideFriction(CarBody carBody)
    {
        if (!isContactToGround)
        {
            return;
        }
        // 处理轮胎接触平面与自身产生横向相对滑动时的力
        float absLateralSpeed = Mathf.Abs(_currentHitPoint.hitPointLateralSpeed);
        float lateralForceMax = carMass * 0.25f * absLateralSpeed / Time.fixedDeltaTime;

        sideSlipSign = GetDirection(_currentHitPoint.hitPointLateralSpeed);
        peakSideFrictionForce = frictionPeak * suspensionScalarForce * sideLoadCoeff * lateralFrictionGrip;
        sideForce = -sideSlipSign * frictionPeak * suspensionScalarForce * sideLoadCoeff * lateralFrictionGrip;
        lateralFrictionForce = Mathf.Clamp(sideForce, -lateralForceMax, lateralForceMax);

        // 主动施加轮胎横向的力实现转向效果
        var tireTransform = transform.position;
        Vector3 steeringDirection = transform.right;

        Vector3 wheelVelocity = carBody.CarRigidbody.GetPointVelocity(tireTransform);
        float wheelForwardVelocity = Vector3.Dot(wheelVelocity, transform.forward);

        float steeringVelocity = Vector3.Dot(wheelVelocity, steeringDirection);
        
        tireGripFactor = !carBody.IsDrifting
            ? steeringCurve.Evaluate(Math.Abs(wheelForwardVelocity / carBody.MaxEngineVelocity))
            : driftingTireGripFactor;

        float expectedVelChange = -steeringVelocity * tireGripFactor;

        float desiredAccel = expectedVelChange / Time.fixedDeltaTime;

        lateralFrictionForce += tireMass * desiredAccel;
        
        lateralFrictionForce = Mathf.Clamp(lateralFrictionForce, -peakSideFrictionForce, +peakSideFrictionForce);
    }
}