using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarSimulation : MonoBehaviour
{
    [SerializeField] private Transform wheelConnectionTransform;
    [SerializeField] private Transform wheelsTransform;
    [SerializeField] private float wheelRadius = 1f;
    [SerializeField] private float wheelWidth = 0.25f;
    [SerializeField] private float springStrength = 1200f;
    [SerializeField] private float springDamping = 200f;
    [SerializeField] private float suspensionRestDist = 1.2f;
    [SerializeField] private float springDefaultLength = 1f;
    [SerializeField] private float springMaxLength = 1.5f;
    [SerializeField] private float springMinLength = 0.5f;
    [SerializeField] private float fixedDeltaTime = 0.005f;

    [SerializeField] [Range(0f,1f)]private float tireGripFactor = 0.7f;
    [SerializeField] private float tireMass = 100f;
    [SerializeField] [Range(0.01f, 1f)]private float raycastPrecision = 0.1f;
    [SerializeField] [Range(0.1f, 1f)] private float widthRaycastPrecision = 0.2f;
    [SerializeField] [Range(0.1f, 1f)] private float frictionCoefficient = 0.1f;
    [SerializeField] private float engineTorque = 100f;
    [SerializeField] private float brakeForce = 50f;

    [SerializeField] private bool drawRaycastDebugLine = true;
    
    private Rigidbody carRigidbody;
    private BoxCollider carFrameCollider;
    private List<Transform> wheelConnectTransforms = new List<Transform>();
    private List<Transform> wheelTransforms = new List<Transform>();
    
    private GameInputActions gameInputActions;

    private Vector3 carFrameSize;

    private void Awake()
    {
        carFrameCollider = GetComponent<BoxCollider>();
        carRigidbody = GetComponent<Rigidbody>(); 
        gameInputActions = new GameInputActions();
        gameInputActions.Player.Enable();
        AssignWheelTransformsToArray();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        Time.fixedDeltaTime = fixedDeltaTime;
        HandleWheelsSuspensionForce();
    }

    private void HandleWheelsSuspensionForce()
    {
        for (int index = 0; index < wheelConnectTransforms.Count; index++)
        {
            var wheelConnectTransform = wheelConnectTransforms[index];
            var wheelTransform = wheelTransforms[index];
            var wheelPosition = wheelTransform.position;
            //raycast to detect a support ground or object
            var origin = wheelConnectTransform.position;
            var direction = -wheelConnectTransform.up;
            var maxDistance = springMaxLength + wheelRadius;
            
            float minRayCastDistance = Single.MaxValue;
            bool raycastResult = false;
            RaycastHit raycastHit;
            for (float i = 0; i <= 1; i+= raycastPrecision)
            {
                for (float k = 0; k<=1; k+=widthRaycastPrecision)
                {
                    
                    var rayOrigin = origin + wheelConnectTransform.forward * ((i - 0.5f) * 2 * wheelRadius) + wheelConnectTransform.right * ((k-0.5f) * wheelWidth);
                    var rayDirection = direction;
                    var rayRadius = Math.Sqrt(Math.Pow(wheelRadius, 2f) -
                                              Math.Pow((i - 0.5f) * 2 * wheelRadius, 2));
                    var rayMaxDistance = springMaxLength + rayRadius;

                    var unitRayResult = Physics.Raycast(rayOrigin, rayDirection, out raycastHit, (float)rayMaxDistance);
                    if (drawRaycastDebugLine)
                    {
                        Debug.DrawLine(rayOrigin, rayOrigin + direction * (float)rayMaxDistance, Color.green);
                    }

                    if (unitRayResult)
                    {
                        var defaultColor = Color.red;
                        if (raycastHit.distance - rayRadius < springMinLength)
                        {
                            defaultColor = Color.yellow;
                        }

                        float rayPointOffset = (float)(raycastHit.distance - rayRadius);
                        // float rayPointOffset = (float)Math.Max(raycastHit.distance - rayRadius, springMinLength);
                        if (minRayCastDistance > rayPointOffset)
                        {
                            minRayCastDistance = rayPointOffset;
                        }

                        if (drawRaycastDebugLine)
                        {
                            Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * (raycastHit.distance), defaultColor);
                        }

                        raycastResult = true;
                    }
                }


            }
            
            // var raycastResult = Physics.Raycast(origin, direction, out RaycastHit raycastHit, maxDistance);

            if (drawRaycastDebugLine)
            {
                Debug.DrawLine(wheelConnectTransform.position, wheelConnectTransform.position - wheelConnectTransform.up * (minRayCastDistance), Color.blue);
            }

            
            Vector3 wheelNewPosition;
            Quaternion newQuaternion;

            //Suspension Force = (Spring Offset Length * Spring Strength) - (Spring Velocity * Damping阻尼)
            if (raycastResult)
            {
                var springDirection = wheelConnectTransform.up;
                var connectPointPos = wheelConnectTransform.position;
                var wheelWorldVelocity = carRigidbody.GetPointVelocity(connectPointPos);

                suspensionRestDist = springDefaultLength + wheelRadius;

                float offset = suspensionRestDist - minRayCastDistance;
                Debug.Log("offset distance:" + offset);

                float springVelocity = Vector3.Dot(springDirection, wheelWorldVelocity);

                float suspensionForce = (offset * springStrength) - (springVelocity * springDamping);

                Debug.Log("Suspension force: " + suspensionForce);
                Debug.DrawLine(wheelPosition,
                    wheelPosition + springDirection * suspensionForce / carRigidbody.mass / 2f, Color.blue);

                var suspensionForceOnSpring = springDirection * suspensionForce;
                carRigidbody.AddForceAtPosition(suspensionForceOnSpring,
                    wheelTransform.position);


                Vector3 wheelPosOffset =
                    // -wheelConnectTransform.up * (Math.Min(minRayCastDistance - wheelRadius, springDefaultLength));
                    -carRigidbody.transform.up * Math.Max(minRayCastDistance, springMinLength);

                    wheelNewPosition = connectPointPos +wheelPosOffset;

                Vector3 steeringDirection = wheelTransform.right;

                Vector3 wheelVelocity = carRigidbody.GetPointVelocity(wheelPosition);
                float wheelForwardVelocity = Vector3.Dot(wheelVelocity, wheelTransform.forward);

                float steeringVelocity = Vector3.Dot(wheelVelocity, steeringDirection);

                float expectedVelChange = -steeringVelocity * tireGripFactor;

                float desiredAccel = expectedVelChange / Time.fixedDeltaTime;

                var steeringForce = steeringDirection * (tireMass * desiredAccel);
                Debug.Log("Add Tire Steering force of " + wheelTransform.name + " with force: " + steeringForce);
                carRigidbody.AddForceAtPosition(steeringForce, wheelPosition);
                Debug.DrawLine(wheelPosition, wheelPosition + steeringForce / carRigidbody.mass, Color.red, 0f, false);
                
                if (wheelTransform.CompareTag("CannotSteer"))
                {
                    newQuaternion = Quaternion.Euler( new Vector3(0, carRigidbody.rotation.eulerAngles.y, carRigidbody.rotation.eulerAngles.z));
                    wheelTransform.DORotateQuaternion(newQuaternion, 0.02f);
                }
                
                
                //handle accelerating and braking
                Vector2 inputVector = gameInputActions.Player.Move.ReadValue<Vector2>();
                Debug.Log("inputVectorValue: " + inputVector);
                if (inputVector.y == 0)
                {
                    
                }
                else
                {
                    Vector3 forwardDir = wheelTransform.forward;
                    if (inputVector.y > 0)
                    {
                        float carSpeed = Vector3.Dot(carRigidbody.transform.forward, carRigidbody.velocity);
                        carRigidbody.AddForceAtPosition(forwardDir * engineTorque, wheelPosition);
                        Debug.DrawLine(wheelPosition, wheelPosition + forwardDir * engineTorque / carRigidbody.mass / 2f, Color.magenta);
                    }
                    else
                    {
                        carRigidbody.AddForceAtPosition(-forwardDir * brakeForce, wheelPosition);
                        Debug.DrawLine(wheelPosition, wheelPosition - forwardDir * engineTorque / carRigidbody.mass / 2f, Color.magenta);
                    }
                }
                
                //handle friction
                if (wheelForwardVelocity != 0f)
                {
                    int directionControl = wheelForwardVelocity > 0 ? 1 : -1;
                    var frictionForce = Vector3.Dot(suspensionForceOnSpring,Vector3.down) * frictionCoefficient;
                    carRigidbody.AddForceAtPosition(wheelTransform.forward * (directionControl * frictionForce), wheelTransform.position);
                    Debug.DrawLine(wheelPosition,
                        wheelPosition + wheelTransform.forward * -frictionForce / carRigidbody.mass / 2f, Color.black);
                }
                
                //update wheel position;
                wheelTransform.position = wheelNewPosition;
                
            }
            else
            {
                // wheelNewPosition = origin + -transform.up * springDefaultLength;
                // wheelTransform.DORotateQuaternion(carRigidbody.transform.rotation, 0.02f);
            }

            if (wheelTransform.GetComponent<WheelVisual>())
            {
                wheelTransform.GetComponent<WheelVisual>().WorldVelocity =
                    Vector3.Dot(carRigidbody.GetPointVelocity(wheelTransform.position), wheelTransform.forward);
            }

            carRigidbody.AddForceAtPosition(tireMass * 10f * Vector3.down, wheelConnectTransform.position);
        }
    }

    private void AssignWheelTransformsToArray()
    {
        //initialize _wheels
        var wheelCount = wheelConnectionTransform.childCount;

        for (int index = 0; index < wheelCount; index++)
        {
            wheelConnectTransforms.Add(wheelConnectionTransform.GetChild(index));
            wheelTransforms.Add(wheelsTransform.GetChild(index));
        }
    }
}