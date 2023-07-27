using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelAddon : AddonObject
{
    public override void InitializeCarRigidbody(Rigidbody rigidbody)
    {
        base.InitializeCarRigidbody(rigidbody);
        GetComponent<ConfigurableJoint>().connectedBody = rigidbody;
    }
}
