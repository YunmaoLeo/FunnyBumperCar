using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SpringPillar : MonoBehaviour
{
    [SerializeField] private float springForceCoefficient = 400000f;
    [SerializeField] private float springDefaultSize = 1f;

    [SerializeField] private float springMinScaleRatio = 0.6f;

    [SerializeField] private Transform pillarMesh;
    [SerializeField] private AnimationCurve springForceCoefficientCurve;

    private List<GameObject> collisionGameObjectsList = new List<GameObject>();

    private float currentScale = 1;

    private BoxCollider triggerCollider;

    private BoxCollider minPillarCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<BoxCollider>();
        CreateMinPillarCollider(triggerCollider);
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

        pillarMesh.localScale = new Vector3(1, 1, currentScale);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Car"))
        {
            return;
        }

        collisionGameObjectsList.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Car"))
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
        if (!other.CompareTag("Car"))
        {
            return;
        }
        var contactPoint = other.bounds.ClosestPoint(transform.position);

        var distance = Vector3.Distance(transform.position, contactPoint);

        var springOffsetDistance = springDefaultSize - Math.Max(distance, springDefaultSize * springMinScaleRatio);

        currentScale = (springDefaultSize - springOffsetDistance) / springDefaultSize;

        var springDirection = Vector3.Dot(contactPoint - transform.position, transform.forward);
        
        var springForce = springOffsetDistance * springForceCoefficient * (springDirection * transform.forward) * springForceCoefficientCurve.Evaluate(currentScale);

        var rigidBody = other.GetComponent<Rigidbody>();
        rigidBody.AddForce(springForce, ForceMode.Force);

        Debug.DrawLine(rigidBody.position, rigidBody.position + springForce / rigidBody.mass / 2, Color.red);
    }
}