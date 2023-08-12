using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    [SerializeField] private float angularVelocity = 0.5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 0.001f;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var rotateAngle = (float)(360f / (2f * Math.PI) * angularVelocity * Time.fixedDeltaTime);
        rotateAngle %= 360.0f;
        var newQuaternion = Quaternion.Euler(new Vector3(0,rotateAngle,0));
        
        rb.MoveRotation(rb.rotation* newQuaternion);
        var newPosition = rb.position;
        newPosition.z += speed;
        rb.MovePosition(newPosition);
    }
}
