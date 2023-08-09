using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarPresentPlatform : MonoBehaviour
{

    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private float showPartSpeed = 3f;
    [SerializeField] private Transform CarSpawnTransform;

    [SerializeField] private Transform ShowFront;
    [SerializeField] private Transform ShowSideRight;
    [SerializeField] private Transform ShowSideLeft;
    [SerializeField] private Transform ShowBack;

    
    public Vector2 RotateSignal { get; set; }

    public Transform GetSpawnTransform()
    {
        return CarSpawnTransform;
    }

    public void ShowSpecificTransform(Transform showTransform)
    {
        var variation = Math.Abs(showTransform.localEulerAngles.y - transform.eulerAngles.y);
        transform.DORotate(showTransform.localEulerAngles, variation / 360f * showPartSpeed);
    }

    public void DoShowSideRight()
    {
        ShowSpecificTransform(ShowSideRight);
    }

    
    public void DoShowSideLeft()
    {
        ShowSpecificTransform(ShowSideLeft);
    }
    
    
    public void DoShowBack()
    {
        ShowSpecificTransform(ShowBack);
    }
    
    
    public void DoShowFront()
    {
        ShowSpecificTransform(ShowFront);
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
