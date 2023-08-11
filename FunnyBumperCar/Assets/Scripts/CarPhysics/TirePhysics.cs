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


    [Space] [Header("Debug Properties")] [SerializeField]
    private bool drawRaycastDebugLine = false;

    private TireVisual tireVisual;

    private TireUtils.HitPointInfo _currentHitPoint;
    public bool IsContactToGround = false;
    private Vector3 _suspensionForce;
    private Vector3 _frictionForce;

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
    public void SimulateSuspensionSystem(Transform tireConnectPoint, Rigidbody carRigidbody, float minRaycastDistance,
        out Vector3 suspensionForceOnSpring)
    {
        var springDirection = tireConnectPoint.up;
        var connectPointPos = tireConnectPoint.position;
        var wheelWorldVelocity = carRigidbody.GetPointVelocity(connectPointPos);

        var suspensionRestDist = springDefaultLength;

        float offset = springDefaultLength - (springMinLength + minRaycastDistance);

        float springVelocity = Vector3.Dot(springDirection, wheelWorldVelocity);

        float suspensionForce = (offset * springStrength) - (springVelocity * springDamping);

        var tirePosition = transform.position;
        Debug.DrawLine(connectPointPos,
            connectPointPos + springDirection * suspensionForce / carRigidbody.mass / 2f, Color.blue);

        suspensionForceOnSpring = springDirection * suspensionForce;

        Vector3 wheelPosOffset =
            -springDirection * (minRaycastDistance + springMinLength);

        //update tire position;
        tirePosition = connectPointPos + wheelPosOffset;
        carRigidbody.AddForceAtPosition(suspensionForceOnSpring,
            connectPointPos, ForceMode.Force);

        transform.position = tirePosition;
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
        carRigidbody.AddForceAtPosition(steeringForce, tireTransform);

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

    public void SimulateFriction(Rigidbody carRigidbody, Vector3 suspensionForceOnSpring)
    {
        var tireVelocity = carRigidbody.GetPointVelocity(transform.position);
        float tireForwardVelocity = Vector3.Dot(tireVelocity, transform.forward);
        if (tireForwardVelocity != 0f)
        {
            int directionControl = tireForwardVelocity > 0 ? 1 : -1;
            var frictionForce = Vector3.Dot(suspensionForceOnSpring + (tireMass) * Physics.gravity, Vector3.down) *
                                frictionCoefficient * (isBraking ? brakeFrictionMultiplier : 1f);

            //friction clamp
            var frictionAbsMax = tireMass * Math.Abs(tireForwardVelocity) / Time.fixedDeltaTime;
            frictionForce = Mathf.Clamp(frictionForce, -frictionAbsMax, +frictionAbsMax);

            carRigidbody.AddForceAtPosition(transform.forward * (directionControl * frictionForce), transform.position);

            Debug.DrawLine(transform.position,
                transform.position + transform.forward * frictionForce / carRigidbody.mass / 2f, Color.black);
        }
    }

    public void HandleTireVisual(Rigidbody carRigidbody)
    {
        // update tire visual effects;
        tireVisual.WorldVelocity = Vector3.Dot(carRigidbody.GetPointVelocity(transform.position),
            transform.forward);
    }


    private void Start()
    {
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
        bool sphereCastResult = Physics.SphereCast(origin, tireRadius, direction, out var raycastHit, springMaxLength - springMinLength);
        if (!sphereCastResult)
        {
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
            return false;
        }

        // RaycastHit finalHitResult = default;
        // bool finalResult = false;
        //
        // for (float z = 0; z < 1 - 1e-10; z += sphereCastPrecision)
        // {
        //     var castOrigin = origin + transform.right * ((z - 0.5f) * tireWidth);
        //     bool hasDetect = Physics.SphereCast(castOrigin, tireRadius, direction, out var raycastHit, springMaxLength - springMinLength);
        //
        //     if (hasDetect)
        //     {
        //         Debug.DrawLine(castOrigin, raycastHit.point, Color.red);
        //         if (isColliderSameCarDict.ContainsKey(raycastHit.collider))
        //         {
        //             if (isColliderSameCarDict[raycastHit.collider])
        //             {
        //                 continue;
        //             }
        //         }
        //         else
        //         {
        //             var possibleCarBody = raycastHit.collider.transform.GetComponentInParent<CarBody>();
        //             if (possibleCarBody != null)
        //             {
        //                 if (possibleCarBody == carBody)
        //                 {
        //                     isColliderSameCarDict[raycastHit.collider] = true;
        //                     continue;
        //                 }
        //                 else
        //                 {
        //                     isColliderSameCarDict[raycastHit.collider] = false;
        //                 }
        //             }
        //             else
        //             {
        //                 isColliderSameCarDict[raycastHit.collider] = false;
        //             }
        //         }
        //
        //         finalResult = true;
        //         
        //         var rayPointOffset = raycastHit.distance;
        //
        //         if (minRaycastDistance > rayPointOffset)
        //         {
        //             minRaycastDistance = rayPointOffset;
        //             finalHitResult = raycastHit;
        //         }
        //     }
        // }
        //
        // if (finalResult)
        // {
        //     TireUtils.HitPointInfo hitPointInfo = default;
        //     hitPointInfo.raycastHit = finalHitResult;
        //     
        //     var hitRb = finalHitResult.collider.attachedRigidbody;
        //     
        //     // hit collider has a rb, then we compute its velocity and store.
        //     if (hitRb != null)
        //     {
        //         hitPointInfo.rb = hitRb;
        //     }
        //     
        //     _currentHitPoint = hitPointInfo;
        // }
        //
        // return finalResult;

        // check whether this collider is the sub object of this car;
        if (isColliderSameCarDict.ContainsKey(raycastHit.collider))
        {
            if (isColliderSameCarDict[raycastHit.collider])
            {
                return false;
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
                    return false;
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

        TireUtils.HitPointInfo hitPointInfo = default;
        hitPointInfo.raycastHit = raycastHit;
        
        var hitRb = raycastHit.collider.attachedRigidbody;
        
        // hit collider has a rb, then we compute its velocity and store.
        if (hitRb != null)
        {
            hitPointInfo.rb = hitRb;
        }
        
        _currentHitPoint = hitPointInfo;
        
        float rayPointOffset = raycastHit.distance;
        
        if (minRaycastDistance > rayPointOffset)
        {
            minRaycastDistance = rayPointOffset;
        }

        return true;
    }

    public void AddInverseForceToHitPoint()
    {
        if (IsContactToGround)
        {
            if (_currentHitPoint.rb != null)
            {
                Vector3 allForce;
                allForce = - _suspensionForce;
                _currentHitPoint.rb.AddForceAtPosition(transform.position, allForce);
            }
        }
    }

    public bool DetectHitAboveHalfTire()
    {
        return Vector3.Dot(transform.position - _currentHitPoint.raycastHit.point, transform.up) < 0;
    }
}