using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonAddon : AddonObject
{
    [SerializeField] private Transform cannonRotatePlatform;
    [SerializeField] private Transform cannonBarrel;

    [SerializeField] private Transform projectilePrefab;

    [SerializeField] private float recoilForceFactor;

    public override void InitializeCarRigidbody(Rigidbody rigidbody)
    {
        base.InitializeCarRigidbody(rigidbody);
        GetComponent<FixedJoint>().connectedBody = rigidbody;
    }

    public override void TriggerAddon(InputAction.CallbackContext context)
    {
    }
    
    
}
