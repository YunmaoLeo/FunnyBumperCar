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
        // Debug.Log("wheel world velocity: " + worldVelocity);
        // // float frequency = (float)(worldVelocity / 2f / Math.PI / radius);
        // float cycleTime = (float)(2f * Math.PI * radius / worldVelocity);
        //
        // var quaternion = wheelMeshTransform.localEulerAngles;
        // Debug.Log("wheel world quaternion: " + quaternion);
        // var newQuaternion = Quaternion.Euler(quaternion + new Vector3(0.3f * 360f / cycleTime * Time.deltaTime,0,0));
        // // wheelMeshTransform.rotation = newQuaternion;
        // // wheelMeshTransform.localRotation = newQuaternion;
        // wheelMeshTransform.localEulerAngles = quaternion + new Vector3(0.3f * 360f / cycleTime * Time.deltaTime, 0, 0);

    }
    
}
