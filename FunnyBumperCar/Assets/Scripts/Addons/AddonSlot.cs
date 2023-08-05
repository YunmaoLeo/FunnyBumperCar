using System;
using UnityEngine;

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

    [SerializeField] public Transform CarAddonContainerPrefab;
    [SerializeField] private Transform calibrator;
    [SerializeField] public AddonSlotType SlotType;


    private Transform carAddonInstance;
    private AddonContainer_Car _addonContainerCar;

    private void Awake()
    {
    }

    private void Start()
    {
    }

    public AddonContainer_Car GetAddonContainer()
    {
        return _addonContainerCar;
    }

    public void InitializeCarAddon(Rigidbody carRigidbody)
    {
        EquipSpecificCarAddon(carRigidbody, CarAddonContainerPrefab);
    }

    public void EquipSpecificCarAddon(Rigidbody carRigidbody, Transform addonContainerPrefab)
    {
        if (addonContainerPrefab == null)
        {
            return;
        }
        
        RemoveAddon();
        
        carAddonInstance = Instantiate(addonContainerPrefab, transform);
        _addonContainerCar = carAddonInstance.GetComponent<AddonContainer_Car>();

        //do calibration
        Transform addOnCalibrator = _addonContainerCar.Calibrator;

        var rotationDelta = calibrator.localRotation * Quaternion.Inverse(_addonContainerCar.Calibrator.localRotation);
        _addonContainerCar.transform.localRotation *= rotationDelta;
        _addonContainerCar.transform.position += (calibrator.position - addOnCalibrator.position);

        _addonContainerCar.AssignRigidbody(carRigidbody);
    }

    public void RemoveAddon()
    {
        if (carAddonInstance != null)
        {
            Destroy(carAddonInstance);
        }

        carAddonInstance = null;
        _addonContainerCar = null;
    }
}