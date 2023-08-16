using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookAddon : AddonObject
{
    [Header("Properties")] [SerializeField]
    private float maxStretchLength = 10f;

    [SerializeField] private float maxDuration = 2f;
    [SerializeField] private float hookStretchSpeed = 10f;
    [SerializeField] private float retrieveSpeed = 20f;
    [SerializeField] private float retrieveForce = 100f;

    [SerializeField] private LineRenderer hookLine;
    [SerializeField] private Transform hookBase;
    [SerializeField] private Transform hookTransform;
    [SerializeField] private Hook hook;
    [SerializeField] private float minDistance = 0.2f;

    [SerializeField] private float hookCD = 8f;
    // [SerializeField] private ConfigurableJoint hookConfigJoint;
    private float hookCDTimer = 5f;
    private float retrieveTimer = 0f;

    [SerializeField] private Rigidbody hookBaseRb;
    private FixedJoint fixedJoint;
    private Vector3 hookDefaultPosition;
    private HookState hookState = HookState.Static;

    enum HookState
    {
        Static,
        IsFindingTarget,
        IsRetrieving
    }

    public override void InitializeBasePlatformRigidbody(Rigidbody rigidbody)
    {
        base.InitializeBasePlatformRigidbody(rigidbody);
        fixedJoint = GetComponent<FixedJoint>();
        fixedJoint.connectedBody = rigidbody;

        hookDefaultPosition = hookTransform.localPosition;
    }

    private void FixedUpdate()
    {
        UpdateRope();

        switch (hookState)
        {
            case HookState.Static:
                hookCDTimer += Time.fixedDeltaTime;
                break;
            case HookState.IsFindingTarget:
                StretchHook();
                if (GetCurrentDistance() > maxStretchLength || hook.isAttaching)
                {
                    Retrieve();
                }
                break;
            case HookState.IsRetrieving:
                retrieveTimer += Time.fixedDeltaTime;
                hook.AddForceOnHookRope(this.hookBaseRb, retrieveForce);
                if (retrieveTimer > maxDuration)
                {
                    hook.LoseHook();
                }

                if (!hook.isAttaching)
                {
                    RetrieveHook();
                }

                if (GetCurrentDistance() < minDistance || Vector3.Dot(hookTransform.position - hookBase.position, hookBase.forward) > 0)
                {
                    hook.LoseHook();
                    hookState = HookState.Static;
                }
                break;
        }
    }

    private float GetCurrentDistance()
    {
        // var basePos = hookBase.position + hookConfigJoint.connectedAnchor;
        var basePos = hookBase.position;
        var hookPos = hookTransform.position;

        return Vector3.Distance(basePos, hookPos);
    }

    private void StretchHook()
    {
        hookTransform.position += -transform.forward * (Time.fixedDeltaTime * hookStretchSpeed);
    }
    
    private void RetrieveHook()
    {
        hookTransform.position -= -transform.forward * (Time.fixedDeltaTime * retrieveSpeed);
    }
    

    private void EjectHook()
    {
        hook.isActive = true;
        hookState = HookState.IsFindingTarget;
        hookCDTimer = 0f;
        // hookConfigJoint.targetPosition = new Vector3(0, 0, maxStretchLength);
    }

    private void Retrieve()
    {
        hookState = HookState.IsRetrieving;
        // hookConfigJoint.targetPosition = new Vector3(0, 0, 0);
        retrieveTimer = 0f;
    }

    public override void TriggerAddon(InputAction.CallbackContext context)
    {
        base.TriggerAddon(context);
        if (hookCDTimer > hookCD)
        {
            EjectHook();
        }
    }

    private void UpdateRope()
    {
        var p1 = hookBase.localPosition;
        var p2 = hookTransform.localPosition;

        hookLine.SetPosition(0, p1);
        hookLine.SetPosition(1, p2);


    }
}