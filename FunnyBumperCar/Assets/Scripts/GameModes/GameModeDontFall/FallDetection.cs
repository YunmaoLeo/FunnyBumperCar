using System;
using UnityEngine;

public class FallDetection : MonoBehaviour
{
    public Action<Transform> OnCarFallDetected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            OnCarFallDetected.Invoke(other.transform);
        }
    }
}