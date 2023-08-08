using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarPresentPlatform : MonoBehaviour
{

    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private Transform CarSpawnTransform;
    public Vector2 RotateSignal { get; set; }

    public Transform GetSpawnTransform()
    {
        return CarSpawnTransform;
    }

    private void FixedUpdate()
    {
        var currentAngle = transform.eulerAngles;
        if (RotateSignal.x != 0)
        {
            currentAngle.y += RotateSignal.x * rotateSpeed;
            transform.DORotate(currentAngle, 1f);
        }
    }
}
