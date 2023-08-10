using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class TirePhysics : MonoBehaviour
{
    [Header("Basic Properties")] public int TireID;
    [SerializeField] private float tireRadius;

    [SerializeField] private float tireWidth;

    [SerializeField] private float tireMass;

    [SerializeField] [Range(0.01f, 1f)] private float raycastPrecision = 0.2f;

    [SerializeField] [Range(0.1f, 1f)] private float widthRaycastPrecision = 0.3f;


    [Space] [Header("Suspension Properties")] [SerializeField]
    private float springStrength;

    [SerializeField] private float springDamping;

    [SerializeField] private float springDefaultLength;


    [SerializeField] private float springMaxLength;

    [SerializeField] private bool isBraking = false;
    [SerializeField] private float springMinLength;


    [Space][Header("Steering Properties")] [SerializeField]
    private float frictionCoefficient;

    [SerializeField] private float driftingTireGripFactor = 0.1f;
    [SerializeField] private AnimationCurve steeringCurve;

    [SerializeField] [Range(-180f, 180f)] private float steerAngle;

    [SerializeField] private float assistSteerRatio = 0.05f;
    [SerializeField] private float brakeFrictionMultiplier = 10f;

    private float tireGripFactor;
    private Dictionary<Collider, bool> isColliderSameCarDict = new Dictionary<Collider, bool>();


    [Space][Header("Debug Properties")]
    [SerializeField]
    private bool drawRaycastDebugLine = false;

    private TireVisual tireVisual;

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

    private void Awake()
    {
        tireVisual = GetComponent<TireVisual>();
        tireVisual.Radius = tireRadius;
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
            Quaternion newQuaternion = Quaternion.Euler(carFrame.rotation.eulerAngles + new Vector3(0, -steerRotateAngle, 0));
            transform.DORotateQuaternion(newQuaternion, steerRotateTime);
        }
        else
        {
            Quaternion newQuaternion = Quaternion.Euler(carFrame.rotation.eulerAngles + new Vector3(0, steerRotateAngle, 0));
            transform.DORotateQuaternion(newQuaternion, steerRotateTime);
        }
    }

    /**
     * 1. Simulate Suspension Force and add on carRigidbody;
     * 2. Update Tire Position According to the suspension system.
     */
    public void SimulateSuspensionSystem(Transform tireConnectPoint, Rigidbody carRigidbody, float minRaycastDistance, out Vector3 suspensionForceOnSpring)
    {
        var springDirection = tireConnectPoint.up;
        var connectPointPos = tireConnectPoint.position;
        var wheelWorldVelocity = carRigidbody.GetPointVelocity(connectPointPos);

        var suspensionRestDist = springDefaultLength + tireRadius;

        float offset = suspensionRestDist - minRaycastDistance;

        float springVelocity = Vector3.Dot(springDirection, wheelWorldVelocity);

        float suspensionForce = (offset * springStrength) - (springVelocity * springDamping);

        var tirePosition = transform.position;
        Debug.DrawLine(tirePosition,
            tirePosition + springDirection * suspensionForce / carRigidbody.mass / 2f, Color.blue);

        suspensionForceOnSpring = springDirection * suspensionForce;

        carRigidbody.AddForceAtPosition(suspensionForceOnSpring,
            tirePosition, ForceMode.Force);

        Vector3 wheelPosOffset =
            -carRigidbody.transform.up * Math.Max(minRaycastDistance, springMinLength);

        //update tire position;
        tirePosition = connectPointPos + wheelPosOffset;
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


        tireGripFactor = !isDrifting ? steeringCurve.Evaluate(Math.Abs(wheelForwardVelocity / maxEngineVelocity)) : driftingTireGripFactor;

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
                                frictionCoefficient * tireWidth * (isBraking ? brakeFrictionMultiplier : 1f);

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


    public bool SteerRaycast(CarBody carBody, Transform tireConnectPoint, out float minRaycastDistance)
    {
        bool raycastResult = false;
        minRaycastDistance = Single.MaxValue;
        var origin = tireConnectPoint.position;
        var direction = -tireConnectPoint.up;
        for (float i = 0; i <= 1; i += raycastPrecision)
        {
            for (float k = 0; k <= 1; k += widthRaycastPrecision)
            {
                var rayOrigin = origin + transform.forward * ((i - 0.5f) * 2 * tireRadius) +
                                transform.right * ((k - 0.5f) * tireWidth);
                var rayDirection = direction;
                var rayRadius = Math.Sqrt(Math.Pow(tireRadius, 2f) -
                                          Math.Pow((i - 0.5f) * 2 * tireRadius, 2));
                var rayMaxDistance = springMaxLength + rayRadius;

                var unitRayResult = Physics.Raycast(rayOrigin, rayDirection, out RaycastHit raycastHit,
                    (float)rayMaxDistance);
                
                
                if (drawRaycastDebugLine)
                {
                    Debug.DrawLine(rayOrigin, rayOrigin + direction * (float)rayMaxDistance, Color.green);
                }
                
                if (unitRayResult)
                {
                    // check whether this collider is the sub object of this car;
                    if (isColliderSameCarDict.ContainsKey(raycastHit.collider))
                    {
                        if (isColliderSameCarDict[raycastHit.collider])
                        {
                            continue;
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
                                continue;
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

                    raycastResult = true;

                    float rayPointOffset = (float)(raycastHit.distance - rayRadius);
                    if (minRaycastDistance > rayPointOffset)
                    {
                        minRaycastDistance = rayPointOffset;
                    }

                    if (drawRaycastDebugLine)
                    {
                        var defaultColor = Color.red;
                        if (raycastHit.distance - rayRadius < springMinLength)
                        {
                            defaultColor = Color.yellow;
                        }

                        Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * (raycastHit.distance), defaultColor);
                    }
                }
            }
        }

        return raycastResult;
    }
}