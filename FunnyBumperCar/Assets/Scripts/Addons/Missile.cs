using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Missile : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 50f;
    [SerializeField] private float initSpeed = 20f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float maxAngularSpeed = 270f;
    [SerializeField] private float angularSpeedAcceleration = 20f;
    
    [SerializeField] private float maxPredictionTime = 1f;
    [SerializeField] private float maxPredictionDistance = 50f;
    [SerializeField] private float minPredictionDistance = 5f;

    [SerializeField] private float deviationSpeed = 5f;
    [SerializeField] private float deviationAmount = 3f;
    
    [SerializeField] private float explosionIntensity = 2000f;
    [SerializeField] private float explosionRadius = 5.12f;

    [SerializeField] private Transform ExplosionPrefab;

    [HideInInspector]
    public Rigidbody BasePlatformRigidbody;

    private float currentSpeed = 0f;
    private float currentAngularSpeed = 0f;
    
    private Rigidbody homingTargetRb;
    private Rigidbody missileRb;

    private void Awake()
    {
        missileRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentSpeed = initSpeed;
    }

    public void AssignHomingTarget(Rigidbody rigidbody)
    {
        homingTargetRb = rigidbody;
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherCar = other.GetComponentInParent<CarBody>();
        if (otherCar != null && otherCar.CarRigidbody == BasePlatformRigidbody)
        {
            return;
        }
        
        HashSet<Rigidbody> rbSet = new HashSet<Rigidbody>();
        var explosion = Instantiate(ExplosionPrefab, position:other.ClosestPoint(transform.position), Quaternion.identity);
        explosion.localScale = Vector3.one * (explosionRadius / 2.5f);
        Destroy(explosion.gameObject, 1f);

        Collider[] colliders = Physics.OverlapSphere(explosion.position, explosionRadius);
        foreach (var explodedCollider in colliders)
        {
            Rigidbody explodeRb;
            var carBody = explodedCollider.transform.GetComponentInParent<CarBody>();
            if (carBody != null)
            {
                explodeRb = carBody.CarRigidbody;
            }
            else
            {
                explodeRb = explodedCollider.attachedRigidbody;
            }
            if (explodeRb != null && !rbSet.Contains(explodeRb))
            {
                rbSet.Add(explodeRb);
                explodeRb.AddExplosionForce(explosionIntensity, explosion.position, explosionRadius);
            }
        }
        
        //Explosion
        Destroy(gameObject);
    }
    

    private void FixedUpdate()
    {
        // 1. provide driving force/velocity;
        currentSpeed += acceleration * Time.fixedDeltaTime;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        missileRb.velocity = currentSpeed * transform.forward;

        currentAngularSpeed += angularSpeedAcceleration * Time.fixedDeltaTime;
        currentAngularSpeed = MathF.Min(currentAngularSpeed, maxAngularSpeed);
        
        // 2. missile pose adjustment;

        if (homingTargetRb != null)
        {
            Vector3 targetPosition = homingTargetRb.worldCenterOfMass + (homingTargetRb.transform.up * 10f);

            var distance = Vector3.Distance(targetPosition, transform.position);

            var predictionPercentage = Mathf.InverseLerp(minPredictionDistance, maxPredictionDistance, distance);

            // if distance is greater than tolerance distance, we use a predict position;
            targetPosition = PredictTargetPosition(predictionPercentage) + AddDeviationOnPrediction(predictionPercentage);
            
            RotateMissile(targetPosition);
        }
        else
        {
            missileRb.MoveRotation(Quaternion.identity);
        }
    }

    /**
     * Aim the target through rotate the missile;
     * 导弹姿态调整，对准目标
     */
    private void RotateMissile(Vector3 targetPosition)
    {
        var heading = targetPosition - transform.position;
        var rotation = Quaternion.LookRotation(heading);

        var partialRotation =
            Quaternion.RotateTowards(transform.rotation, rotation, currentAngularSpeed * Time.fixedDeltaTime);
        missileRb.MoveRotation(partialRotation);
    }

    private Vector3 PredictTargetPosition(float predictionTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, maxPredictionTime, predictionTimePercentage);

        var targetPredictedPosition = homingTargetRb.worldCenterOfMass + homingTargetRb.velocity * predictionTime;

        return targetPredictedPosition;
    }

    private Vector3 AddDeviationOnPrediction(float predictionTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * deviationSpeed), 0, 0);

        var predictionOffset = transform.TransformDirection(deviation) * predictionTimePercentage * deviationAmount;

        return predictionOffset;
    }
}
