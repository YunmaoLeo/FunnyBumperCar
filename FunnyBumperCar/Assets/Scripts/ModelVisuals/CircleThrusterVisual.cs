using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CircleThrusterVisual : MonoBehaviour
{
    [SerializeField] private Transform bladesTransform;

    [SerializeField] private float bladeRadius = 0.13f;

    [SerializeField] private ParticleSystem flameParticle;
    
    private Vector3 lastPosition;
    private float WorldVelocity = 0f;


    private void Awake()
    {
        lastPosition = transform.position;
        flameParticle.Stop();
    }

    public void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, lastPosition);

        WorldVelocity = distance / Time.fixedDeltaTime;
        float cycleTime = (float)(2f * Math.PI * bladeRadius / WorldVelocity);
        var rotateAngle = 1f * 360f / cycleTime * Time.fixedDeltaTime;
        var newQuaternion = 
            Quaternion.Euler(new Vector3(0,0,rotateAngle));
        // Quaternion.AngleAxis(rotateAngle, bladesTransform.forward);
        bladesTransform.localRotation *= newQuaternion;

        lastPosition = transform.position;
    }

    public void OnThrusterEject()
    {
        flameParticle.Play();
    }

    public void OnThrusterEndEject()
    {
        flameParticle.Stop();
    }
}
