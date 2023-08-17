using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TireVisual : MonoBehaviour
{
    private float radius = 0.3f;
    private float worldVelocity;
    private Transform wheelMeshTransform;
    [SerializeField] private bool isWheelRotate = true;

    private void Awake()
    {
        wheelMeshTransform = transform.GetChild(0);
    }

    public float  WorldVelocity
    {
        get => worldVelocity;
        set => worldVelocity = value;
    }

    public float Radius
    {
        get => radius;
        set => radius = value;
    }

    public Transform MeshTransform => wheelMeshTransform;

    private void FixedUpdate()
    {
        // float cycleTime = (float)(2f * Math.PI * radius / worldVelocity);
        // var rotateAngle = 1f * 360f / cycleTime * Time.fixedDeltaTime;
        // var newQuaternion = Quaternion.Euler(new Vector3(rotateAngle,0,0));
        // wheelMeshTransform.localRotation *= newQuaternion;
    }

    public void UpdateRotation(float angularVelocity)
    {
        if (!isWheelRotate)
        {
            return;
        }
        var rotateAngle = (float)(360f / (2f * Math.PI) * angularVelocity * Time.fixedDeltaTime);
        var newQuaternion = Quaternion.Euler(new Vector3(rotateAngle,0,0));
        wheelMeshTransform.localRotation *= newQuaternion;
    }
}
