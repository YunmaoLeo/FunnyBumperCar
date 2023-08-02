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
    private AddonContainer _addonContainer;
    private void Awake()
    {
        
    }

    private void Start()
    {
    }

    public AddonContainer GetAddonContainer()
    {
        return _addonContainer;
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
        _addonContainer = carAddonInstance.GetComponent<AddonContainer>();

        //do calibration
        Transform addOnCalibrator = _addonContainer.Calibrator;
        
        var rotationDelta = calibrator.localRotation * Quaternion.Inverse(_addonContainer.Calibrator.localRotation);
        _addonContainer.transform.localRotation *= rotationDelta;
        _addonContainer.transform.position += (calibrator.position - addOnCalibrator.position);

        _addonContainer.AssignCarRigidbody(carRigidbody);
    }
    
}
