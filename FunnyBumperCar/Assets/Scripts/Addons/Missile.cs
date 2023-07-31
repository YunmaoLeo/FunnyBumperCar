using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Missile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float angularSpeed = 0.5f;
    
    [SerializeField] private float maxPredictionTime = 1f;
    [SerializeField] private float maxPredictionDistance = 50f;
    [SerializeField] private float minPredictionDistance = 5f;

    [SerializeField] private float deviationSpeed = 5f;
    [SerializeField] private float deviationAmount = 3f;
    
    [SerializeField] private float explosionIntensity = 2000f;
    [SerializeField] private float explosionRadius = 5f;

    [SerializeField] private Transform ExplosionPrefab;
    
    private Rigidbody homingTargetRb;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void AssignHomingTarget(Rigidbody rigidbody)
    {
        homingTargetRb = rigidbody;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Missile has detect collision of trigger");
        Debug.Log(other.gameObject.name);

        var explosion = Instantiate(ExplosionPrefab, position:other.ClosestPoint(transform.position), Quaternion.identity);
        //Explosion
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        // 1. provide driving force/velocity;
        rb.velocity = speed * transform.forward;
        
        // 2. missile pose adjustment;

        if (homingTargetRb != null)
        {
            Vector3 targetPosition = homingTargetRb.worldCenterOfMass;

            var distance = Vector3.Distance(targetPosition, transform.position);

            var predictionPercentage = Mathf.InverseLerp(minPredictionDistance, maxPredictionDistance, distance);

            // if distance is greater than tolerance distance, we use a predict position;
            targetPosition = PredictTargetPosition(predictionPercentage) + AddDeviationOnPrediction(predictionPercentage);
            
            RotateMissile(targetPosition);
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
            Quaternion.RotateTowards(transform.rotation, rotation, angularSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(partialRotation);
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
