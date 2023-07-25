using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPillarAddon : BaseAddon
{
    public override void OnInitialState()
    {
        GetComponentInChildren<SpringPillar>().InitializeAddon(carRigidbody);
    }
}
