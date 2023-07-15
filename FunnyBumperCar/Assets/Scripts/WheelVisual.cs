using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WheelVisual : MonoBehaviour
{
    [SerializeField] private float radius = 0.3f;
    [SerializeField] private Transform wheelMeshTransform;
    private float worldVelocity;

    public float  WorldVelocity
    {
        get => worldVelocity;
        set => worldVelocity = value;
    }

    private void FixedUpdate()
    {
        float cycleTime = (float)(2f * Math.PI * radius / worldVelocity);
        var rotateAngle = 1f * 360f / cycleTime * Time.fixedDeltaTime;
        var newQuaternion = Quaternion.Euler(new Vector3(rotateAngle,0,0));
        wheelMeshTransform.localRotation *= newQuaternion;
    }
}
