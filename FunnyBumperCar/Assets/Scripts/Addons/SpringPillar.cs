using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SpringPillar : AddonObject
{
    [SerializeField] private float springForceCoefficient = 400000f;
    [SerializeField] private float springDefaultSize = 1f;

    [SerializeField] private float springMinScaleRatio = 0.6f;

    [SerializeField] private float springDamping = 20000f;

    [SerializeField] private Transform pillarMesh;
    [SerializeField] private AnimationCurve springForceCoefficientCurve;

    [SerializeField] private ForceMode forceMode;
    
    private bool isAddon = false;

    private float springScaleVelocity;

    private List<GameObject> collisionGameObjectsList = new List<GameObject>();

    private float currentScale = 1f;
    private float lastFrameScale = 1f;

    private BoxCollider triggerCollider;

    private BoxCollider minPillarCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<BoxCollider>();
        CreateMinPillarCollider(triggerCollider);
    }

    public override void InitializeCarRigidbody(Rigidbody rigidbody)
    {
        base.InitializeCarRigidbody(rigidbody);
        isAddon = true;
        GetComponent<FixedJoint>().connectedBody = rigidbody;
    }


    private void CreateMinPillarCollider(BoxCollider triggerCollider)
    {
        minPillarCollider = gameObject.AddComponent<BoxCollider>();
        var colliderCenter = triggerCollider.center;
        var colliderSize = triggerCollider.size;
        colliderSize.z = triggerCollider.size.z * springMinScaleRatio;

        minPillarCollider.size = colliderSize;
        minPillarCollider.center = colliderCenter;
    }

    private void FixedUpdate()
    {

        if (collisionGameObjectsList.Count == 0)
        {
            currentScale = Mathf.Lerp(currentScale, 1f, (1 - currentScale) * 0.08f);
        }

        currentScale = Mathf.Clamp(currentScale, springMinScaleRatio, 1f);

        springScaleVelocity = currentScale - lastFrameScale;
        lastFrameScale = currentScale;
        
        pillarMesh.localScale = new Vector3(1, 1, currentScale);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Ground"))
        {
            return;
        }
        collisionGameObjectsList.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Ground"))
        {
            return;
        }
        if (collisionGameObjectsList.Contains(other.gameObject))
        {
            collisionGameObjectsList.Remove(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            return;
        }
        var contactPoint = other.bounds.ClosestPoint(transform.position);

        var distance = Vector3.Distance(transform.position, contactPoint);

        var springOffsetDistance = springDefaultSize - Math.Max(distance, springDefaultSize * springMinScaleRatio);

        currentScale = (springDefaultSize - springOffsetDistance) / springDefaultSize;

        var springDirection = Vector3.Dot(contactPoint - transform.position, transform.forward);

        var springDampingForce = springScaleVelocity * springDamping;
        var springBaseForce = springOffsetDistance * springForceCoefficient;
        var springForceDir = springDirection * transform.forward;
        var springForceCurveFactor = springForceCoefficientCurve.Evaluate(currentScale);
        
        var springForce = (springBaseForce * springForceCurveFactor - springDampingForce) * springForceDir;

        if (other.GetComponent<Rigidbody>())
        {
            var rigidBody = other.GetComponent<Rigidbody>();
            rigidBody.AddForce(springForce, forceMode);
            Debug.DrawLine(rigidBody.position, rigidBody.position + springForce / rigidBody.mass / 2, Color.red);
        }

        if (isAddon)
        {
            carRigidbody.AddForce(-springForce, forceMode);
            Debug.DrawLine(carRigidbody.position, carRigidbody.position - springForce / carRigidbody.mass / 2, Color.red);
        }
    }
}