using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CircleThrusterAddon : AddonObject
{
    [SerializeField] private float ejectionForceCoefficient = 100f;
    [SerializeField] private float ejectionCDTime = 1f;
    [SerializeField] private float ejectionDuration = 0.2f;
    [SerializeField] private Transform ThrusterTransform;
    private float ejectionCDTimer = 0f;

    private CircleThrusterVisual thrusterVisual;

    [SerializeField] private ConfigSlideRangeCommand<float> thrusterRotationChangeCommand = new ConfigSlideRangeCommand<float>
    {
        min = 0f,
        max = 360f,
        description = "Thruster Y Rotation",
    };

    [SerializeField] private ForceMode forceMode;

    public override void OnInitialState()
    {
        base.OnInitialState();
        thrusterVisual = GetComponent<CircleThrusterVisual>();
    }


    public override void InitializeConfigSlideRangeCommands()
    {
        base.InitializeConfigSlideRangeCommands();
        thrusterRotationChangeCommand.OnValueLegallyChanged += RotateThrusterAroundYAxis;
        ConfigFloatSlideRangeCommandsList.Add(thrusterRotationChangeCommand);
    }

    private void RotateThrusterAroundYAxis(float newAngle)
    {
        var oldAngles = ThrusterTransform.localEulerAngles;
        var angleChanges = newAngle - oldAngles.y;
        oldAngles.y = newAngle;

        ThrusterTransform.DOLocalRotate(oldAngles, Math.Abs(angleChanges) / 360f);
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
        base.TriggerAddon(context);
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
            Debug.DrawLine(transform.position, transform.position + ThrusterTransform.forward * ejectionForceCoefficient, Color.red);
            durationTime += Time.deltaTime;
            yield return null;
        }
        thrusterVisual.OnThrusterEndEject();
    }
}