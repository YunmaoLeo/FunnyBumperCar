using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CircleThrusterAddon : BaseAddon
{
    [SerializeField] private float ejectionForceCoefficient = 100f;
    [SerializeField] private float ejectionCDTime = 1f;
    [SerializeField] private float ejectionDuration = 0.2f;
    private float ejectionCDTimer = 0f;


    [SerializeField] private ForceMode forceMode;
    private void Awake()
    {
        
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
            StartCoroutine(Ejection());
        }
    }

    IEnumerator Ejection()
    {
        float durationTime = 0f;
        while (durationTime < ejectionDuration)
        {
            carRigidbody.AddForce(transform.forward * ejectionForceCoefficient);
            Debug.DrawLine(transform.position, transform.position + transform.forward * ejectionForceCoefficient, Color.red);
            durationTime += Time.deltaTime;
            yield return null;
        }
    }
}