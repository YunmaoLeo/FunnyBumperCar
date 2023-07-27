using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [SerializeField] private float ejectDuration = 0.1f;
    [SerializeField] private float ejectForce = 5000f;
    [SerializeField] private float rotateAngle = 35f;
    [SerializeField] private float resetRotateTime = 1f;
    
    [SerializeField] private bool rotateTrigger = false;

    private Dictionary<GameObject, List<GameObject>> carOnPlatformTiresDict =
        new Dictionary<GameObject, List<GameObject>>();

    private List<GameObject> carBodyOnPlatformList = new List<GameObject>();

    private Renderer meshRenderer;

    private bool isRotating = false;
    private Vector3 initialEulerAngle;
    
    private void Update()
    {
    }

    private void Awake()
    {
        initialEulerAngle = transform.eulerAngles;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        if (rotateTrigger)
        {
            DoRotate();
            rotateTrigger = false;
        }
    }

    public bool Trigger()
    {
        if (!isRotating)
        {
            DoRotate();
            return true;
        }

        return false;
    }

    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(initialEulerAngle);
    }

    private void DoRotate()
    {
        var newQuaternion = Quaternion.Euler(initialEulerAngle.x + rotateAngle, initialEulerAngle.y, initialEulerAngle.z);
        transform.DORotateQuaternion(newQuaternion, ejectDuration).SetEase(Ease.InCubic);
        isRotating = true;
        
        foreach (var pair in carOnPlatformTiresDict)
        {
            var rigidBody = pair.Key.GetComponent<Rigidbody>();
            if (CheckIsMassCenterOnPlatform(rigidBody))
            {
                pair.Key.transform.SetParent(transform);
            }
        }
        
        foreach (var car in carBodyOnPlatformList)
        {
            if (!carOnPlatformTiresDict.ContainsKey(car))
            {
                var rigidBody = car.GetComponent<Rigidbody>();
                if (CheckIsMassCenterOnPlatform(rigidBody))
                {
                    car.transform.SetParent(transform);
                }
            }
        }
        StartCoroutine(DoEjection(ejectDuration * 0.8f));
        StartCoroutine(ResetRotateState(ejectDuration));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tire"))
        {
            var carRigidBody = other.GetComponentInParent<Rigidbody>();
            if (carRigidBody != null)
            {
                if (!carOnPlatformTiresDict.ContainsKey(carRigidBody.gameObject))
                {
                    carOnPlatformTiresDict.Add(carRigidBody.gameObject, new List<GameObject>(4));
                }
                carOnPlatformTiresDict[carRigidBody.gameObject].Add(other.gameObject);
            }   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tire"))
        {
            var carRigidBody = other.GetComponentInParent<Rigidbody>();
            if (carRigidBody != null)
            {
                if (carOnPlatformTiresDict.ContainsKey(carRigidBody.gameObject))
                {
                    var tireList = carOnPlatformTiresDict[carRigidBody.gameObject];
                    tireList.Remove(other.gameObject);

                    if (tireList.Count == 0)
                    {
                        carOnPlatformTiresDict.Remove(carRigidBody.gameObject);
                        carRigidBody.transform.SetParent(null);
                    }
                }
            }
        }
        
 
    }

    private void OnCollisionEnter(Collision collision)
    {
        var other = collision.collider;
        if (other.CompareTag("Car"))
        {
            var carRigidBody = other.GetComponent<Rigidbody>();
            if (carRigidBody != null)
            {
                if (!carBodyOnPlatformList.Contains(carRigidBody.gameObject))
                {
                    carBodyOnPlatformList.Add(carRigidBody.gameObject);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        var other = collision.collider;
        if (other.CompareTag("Car"))
        {
            var carRigidBody = other.GetComponent<Rigidbody>();
            if (carRigidBody != null)
            {
                if (carBodyOnPlatformList.Contains(carRigidBody.gameObject))
                {
                    carBodyOnPlatformList.Remove(carRigidBody.gameObject);
                    carRigidBody.transform.SetParent(null);
                }
            }
        }
    }

    IEnumerator DoEjection(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        
        foreach (var pair in carOnPlatformTiresDict)
        {
            var carRigidbody = pair.Key.GetComponent<Rigidbody>();
            float force = calculateForceFactorAccordingToDistance(carRigidbody);
            
            foreach (var tire in pair.Value)
            {
                carRigidbody.AddForceAtPosition(transform.up * (force * 1)/4, tire.transform.position);
            }
        }

        foreach (var car in carBodyOnPlatformList)
        {
            var carRigidbody = car.GetComponent<Rigidbody>();
            float force = calculateForceFactorAccordingToDistance(carRigidbody);
            
            carRigidbody.AddForce(force * transform.up);
        }
    }

    IEnumerator ResetRotateState(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        isRotating = false;
        transform.DORotateQuaternion(Quaternion.Euler(initialEulerAngle), resetRotateTime);
    }

    private float calculateForceFactorAccordingToDistance(Rigidbody carRigidbody)
    {
        CheckIsMassCenterOnPlatform(carRigidbody, out float distanceFactor);
        return distanceFactor * ejectForce;
    }

    private bool CheckIsMassCenterOnPlatform(Rigidbody carRigidbody, out float leverageDistanceFactor)
    {
        Vector3 centerOfMass = carRigidbody.transform.position + carRigidbody.centerOfMass;
        Vector3 boundsSize = meshRenderer.bounds.size;

        Vector3 projectorPoint = transform.InverseTransformPoint(centerOfMass);

        Vector2 boundsMaxPoint = new Vector2(boundsSize.x / 2, 0);
        Vector2 boundsMinPoint = new Vector2(-boundsSize.x / 2, -boundsSize.z);

        
        leverageDistanceFactor = (0f - projectorPoint.z) / boundsSize.z;
        
        if (projectorPoint.x > boundsMinPoint.x 
            && projectorPoint.x < boundsMaxPoint.x
            && projectorPoint.z > boundsMinPoint.y 
            && projectorPoint.z < boundsMaxPoint.y)
        {
            return true;
        }
        return false;
    }

    private bool CheckIsMassCenterOnPlatform(Rigidbody carRigidbody)
    {
        return CheckIsMassCenterOnPlatform(carRigidbody, out float defaultValue);
    }
}