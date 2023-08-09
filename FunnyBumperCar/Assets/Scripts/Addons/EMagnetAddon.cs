using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EMagnetAddon : AddonObject
{
    [SerializeField] private float magnetRange = 100f;
    [SerializeField] private float magnetIntensity = 5000f;
    [SerializeField] private float magnetFactorAccelerationPerSecond = 2f;
    [SerializeField] private float magnetMaxFactor = 1f;
    [SerializeField] private float activeMagnetCoolDownTime = 5f;
    [SerializeField] private float magnetLastsDuration = 4f;

    [SerializeField] private LayerMask magneticLayerMask;

    private float currentMagnetFactor;
    private Rigidbody addonRb;
    private bool isMagnetOn;
    
    private float magnetCDTimer;
    
    private FixedJoint fixedJoint;
    private HashSet<Rigidbody> detectedRbSet = new HashSet<Rigidbody>();

    public override void InitializeBasePlatformRigidbody(Rigidbody rigidbody)
    {
        base.InitializeBasePlatformRigidbody(rigidbody);
        fixedJoint = GetComponent<FixedJoint>();
        fixedJoint.connectedBody = rigidbody;
        addonRb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
       magnetCDTimer -= Time.fixedDeltaTime;

       if (isMagnetOn)
       {
           currentMagnetFactor += Time.fixedDeltaTime * magnetFactorAccelerationPerSecond;
           currentMagnetFactor = Mathf.Min(currentMagnetFactor, magnetMaxFactor);
           DoMagnetEveryFrame();
       }
    }

    private void DoMagnetEveryFrame()
    {
        // 1. Do Circle Detection, find any object with magnetic attribute;
        // 2. traversal every magnetic object, assign attractive force;
        var colliders = Physics.OverlapSphere(transform.position, magnetRange, magneticLayerMask);
        Vector3 forceOnMagnet = Vector3.zero;
        foreach (var other in colliders)
        {
            if (other.attachedRigidbody != null && other.attachedRigidbody!= basePlatformRigidbody && !detectedRbSet.Contains(other.attachedRigidbody))
            {
                detectedRbSet.Add(other.attachedRigidbody);
                var distance = Vector3.Distance(other.transform.position, transform.position);
                distance = Mathf.Min(distance, 1f);
                var forceMagnitude = (magnetIntensity * currentMagnetFactor * other.attachedRigidbody.mass) /
                                     Mathf.Pow(distance, 2);
                var magneticForce = Vector3.Normalize(transform.position - other.transform.position) * forceMagnitude;
                other.attachedRigidbody.AddForce(magneticForce);
                forceOnMagnet += (-magneticForce);
            }
        }
        basePlatformRigidbody.AddForce(forceOnMagnet);
        Debug.DrawLine(transform.position, transform.position + forceOnMagnet / 100f, Color.red);
        detectedRbSet.Clear();
    }
    
    public override void TriggerAddon(InputAction.CallbackContext context)
    {
        base.TriggerAddon(context);

        if (magnetCDTimer > 0f)
        {
            return;
        }

        magnetCDTimer = activeMagnetCoolDownTime;
        isMagnetOn = true;
        StartCoroutine(TurnOffMagnet(magnetLastsDuration));
    }

    IEnumerator TurnOffMagnet(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentMagnetFactor = 0;
        isMagnetOn = false;
    }
}