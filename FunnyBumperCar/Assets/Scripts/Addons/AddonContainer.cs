using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Responsible for containing addon and provides calibration with cars.
 */
public class AddonContainer : MonoBehaviour
{
    [SerializeField] public Transform Calibrator;
    [SerializeField] private AddonObject Addon;

    public void TriggerAddon(InputAction.CallbackContext context)
    {
        if (Addon != null)
        {
            Addon.TriggerAddon(context);
        }
    }

    public void SetEnable(bool enable)
    {
        if (Addon != null)
        {
            Addon.SetEnable(enable);
        }
    }



    public void AssignCarRigidbody(Rigidbody rigidbody)
    {
        if (Addon != null)
        {
            Addon.InitializeCarRigidbody(rigidbody);
        }
    }


}
