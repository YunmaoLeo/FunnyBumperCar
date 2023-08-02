using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CircleThrusterAddon : AddonObject
{
    [SerializeField] private float ejectionForceCoefficient = 100f;
    [SerializeField] private float ejectionCDTime = 1f;
    [SerializeField] private float ejectionDuration = 0.2f;
    private float ejectionCDTimer = 0f;

    private CircleThrusterVisual thrusterVisual;


    [SerializeField] private ForceMode forceMode;
    private void Awake()
    {
        thrusterVisual = GetComponent<CircleThrusterVisual>();
    }
    
    public override void InitializeBasePlatformRigidbody(Rigidbody rigidbody)
    {
        base.InitializeBasePlatformRigidbody(rigidbody);
        GetComponent<FixedJoint>().connectedBody = rigidbody;
    }


    private void FixedUpdate()
    {
        ejectionCDTimer -= Time.fixedDeltaTime;
    }

    public override void TriggerAddon(InputAction.CallbackContext context)
    {
        if (ejectionCDTimer < 0f)
        {
            ejectionCDTimer = ejectionCDTime;
            thrusterVisual.OnThrusterEject();
            StartCoroutine(Ejection());
        }
    }

    IEnumerator Ejection()
    {
        float durationTime = 0f;
        while (durationTime < ejectionDuration)
        {
            basePlatformRigidbody.AddForce(transform.forward * ejectionForceCoefficient);
            Debug.DrawLine(transform.position, transform.position + transform.forward * ejectionForceCoefficient, Color.red);
            durationTime += Time.deltaTime;
            yield return null;
        }
        thrusterVisual.OnThrusterEndEject();
    }
}