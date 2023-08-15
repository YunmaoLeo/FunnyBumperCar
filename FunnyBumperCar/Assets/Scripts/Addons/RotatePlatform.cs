using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    [SerializeField] private float angularVelocity = 0.5f;
    private Rigidbody rb;
    [SerializeField] private float speed = 0.001f;

    [SerializeField] private Vector3 axisOfRotation;
    
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var rotateAngle = (float)(360f / (2f * Math.PI) * angularVelocity * Time.fixedDeltaTime);
        rotateAngle %= 360.0f;

        var deltaQ = Quaternion.AngleAxis(rotateAngle, axisOfRotation);
        var targetRotation = transform.rotation * deltaQ;

        rb.MoveRotation(targetRotation);
        var newPosition = rb.position;
        newPosition.z += speed;
        rb.MovePosition(newPosition);
    }

    public int getBoolInt(bool input)
    {
        return input ? 1 : 0;
    }
}
