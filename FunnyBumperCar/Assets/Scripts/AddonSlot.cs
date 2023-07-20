using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AddonSlot : MonoBehaviour
{
    [Serializable]
    public enum AddonSlotType
    {
        Front,
        Side,
        Back,
        Top,
        Bottom
    }
    
    [SerializeField] public Transform CarAddonPrefab;
    [SerializeField] private Transform calibrator;
    [SerializeField] public AddonSlotType SlotType;
    
    
    private Transform carAddonInstance;
    private BaseAddon addon;
    private void Awake()
    {
        
    }

    private void Start()
    {
    }

    public BaseAddon GetAddon()
    {
        return addon;
    }

    public void InitializeCarAddon(Rigidbody carRigidbody)
    {
        if (carAddonInstance != null)
        {
            Destroy(carAddonInstance);
        }
        
        if (CarAddonPrefab == null)
        {
            return;
        }

        carAddonInstance = Instantiate(CarAddonPrefab, transform);
        addon = carAddonInstance.GetComponent<BaseAddon>();

        Transform addOnCalibrator = addon.Calibrator;

        var rotationDelta = calibrator.rotation * Quaternion.Inverse(addon.Calibrator.rotation);
        addon.transform.rotation *= rotationDelta;
        addon.transform.position += (calibrator.position - addOnCalibrator.position);

        addon.carRigidbody = carRigidbody;

        addon.GetComponent<FixedJoint>().connectedBody = carRigidbody;
    }
    
}
