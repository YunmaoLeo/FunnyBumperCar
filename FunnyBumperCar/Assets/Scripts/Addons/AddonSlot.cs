using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AddonSlot : MonoBehaviour
{
    [Serializable]
    public enum AddonSlotType
    {
        Front,
        SideLeft,
        SideRight,
        Back,
        Top,
        Bottom
    }

    [SerializeField] public Transform CarAddonContainerPrefab;
    [SerializeField] private Transform calibrator;
    [SerializeField] public AddonSlotType SlotType;


    private Transform carAddonContainerInstance;
    private AddonContainer_Car addonContainerCar;
    private AddonObject addon;

    public AddonObject GetAddon()
    {
        return addon;
    }

    private void Awake()
    {
    }

    private void Start()
    {
    }

    public void TriggerAddon(InputAction.CallbackContext context)
    {
        if (addon != null)
        {
            addon.TriggerAddon(context);
        }
    }

    public AddonContainer_Car GetAddonContainer()
    {
        return addonContainerCar;
    }

    public AddonObject InitializeCarAddon(CarBody carBody)
    {
        return EquipSpecificCarAddon(carBody, CarAddonContainerPrefab);
    }
    
    

    public AddonObject EquipSpecificCarAddon(CarBody carBody, Transform addonContainerPrefab)
    {
        if (addonContainerPrefab == null)
        {
            return null;
        }
        
        RemoveAddon(carBody);
        
        carAddonContainerInstance = Instantiate(addonContainerPrefab, transform);
        addonContainerCar = carAddonContainerInstance.GetComponent<AddonContainer_Car>();

        //do calibration
        DoAddonCalibration();
        addonContainerCar.AssignRigidbody(carBody.CarRigidbody);

        addon = carAddonContainerInstance.GetComponentInChildren<AddonObject>();
        addon.OnEquipOnCar(carBody);
        
        return addon;
    }

    public bool RemoveAddon(CarBody carBody)
    {
        if (carAddonContainerInstance == null)
        {
            return false;
        }
        addon.OnRemoveFromCar(carBody);
        Destroy(carAddonContainerInstance.gameObject);


        carAddonContainerInstance = null;
        addonContainerCar = null;
        return true;
    }

    private void DoAddonCalibration()
    {
        Transform addOnCalibrator = addonContainerCar.Calibrator;

        var rotationDelta = calibrator.localRotation * Quaternion.Inverse(addonContainerCar.Calibrator.localRotation);
        addonContainerCar.transform.localRotation *= rotationDelta;
        addonContainerCar.transform.position += (calibrator.position - addOnCalibrator.position);
    }
}