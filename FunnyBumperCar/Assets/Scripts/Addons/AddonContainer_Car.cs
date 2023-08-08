using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Responsible for containing addon and provides calibration with cars.
 */
public class AddonContainer_Car : AddonContainer
{
    [SerializeField] public Transform Calibrator;
    [SerializeField] public AddonSlot.AddonSlotType allowedPositions;
    public void TriggerAddon(InputAction.CallbackContext context)
    {
        if (Addon != null && Addon.isActiveAndEnabled)
        {
            Addon.TriggerAddon(context);
        }
    }

}