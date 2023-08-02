using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AddonController : MonoBehaviour
{
    [Serializable]
    public enum ControlMode{
        Manually,
        Automatic,
    }

    [SerializeField] private ControlMode controlMode;
    [SerializeField] private AddonObject addon;

    [SerializeField] private float automaticTriggerCDTime = 1f;

    private float triggerCDTimer = 0f;
    private void FixedUpdate()
    {
        triggerCDTimer -= Time.fixedDeltaTime;
        if (controlMode == ControlMode.Automatic)
        {
            if (triggerCDTimer <= 0f)
            {
                addon.TriggerAddon(new InputAction.CallbackContext());
                triggerCDTimer = automaticTriggerCDTime;
            }
        }
    }
}