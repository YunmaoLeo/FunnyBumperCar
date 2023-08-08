using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarAssembleController : MonoBehaviour
{
    [SerializeField] private CarComponentsListSO componentsListSO;
    [SerializeField] private SelectorListUI selectorListUITemplate;
    [HideInInspector]
    public Transform carSpawnTransform;

    private SelectorListUI selectorListUI;
    public Player player;
    private Transform carAssembleUI;

    private CarBody carBody;
    private Transform frontLeftTire;
    private Transform frontRightTire;
    private Transform backLeftTire;
    private Transform backRightTire;
    
    private Transform frontAddon;
    private Transform sideLeftAddon;
    private Transform sideRightAddon;
    private Transform backAddon;
    private Transform topAddon;

    private void Awake()
    {
        carAssembleUI = CarAssembleUIManager.Instance.transform;
    }

    private void Start()
    {
        InitializeCar();
        InitializeUI();
    }

    public Transform GetCar()
    {
        return carBody.transform;
    }

    public void SetNewCarBody(Transform newCar)
    {
        if (newCar.TryGetComponent<CarBody>(out CarBody bodyComponent))
        {
            if (carBody.CarID == bodyComponent.CarID)
            {
                return;
            }
            Destroy(carBody.gameObject);
            var carBodyTransform =
                Instantiate(bodyComponent.transform, carSpawnTransform.position, Quaternion.identity);
            carBody = carBodyTransform.GetComponent<CarBody>();
            
            carBody.SetTireAndInstantiate(CarBody.TireLocation.FrontLeft, frontLeftTire);
            carBody.SetTireAndInstantiate(CarBody.TireLocation.FrontRight, frontRightTire);
            carBody.SetTireAndInstantiate(CarBody.TireLocation.BackLeft, backLeftTire);
            carBody.SetTireAndInstantiate(CarBody.TireLocation.BackRight, backRightTire);
            
            carBody.EquipCarAddon(AddonSlot.AddonSlotType.Front, frontAddon);
            carBody.EquipCarAddon(AddonSlot.AddonSlotType.SideRight, sideRightAddon);
            carBody.EquipCarAddon(AddonSlot.AddonSlotType.SideLeft, sideLeftAddon);
            carBody.EquipCarAddon(AddonSlot.AddonSlotType.Top, topAddon);
            carBody.EquipCarAddon(AddonSlot.AddonSlotType.Back, backAddon);
        }
    }

    public void onAssembleDone()
    {
        selectorListUI.showIsDoneUI();
    }

    public void SetNewAddon(AddonSlot.AddonSlotType slotType, Transform addon)
    {
        carBody.EquipCarAddon(slotType, addon);

        switch (slotType)
        {
            case AddonSlot.AddonSlotType.Front:
                frontAddon = addon;
                break;
            case AddonSlot.AddonSlotType.SideLeft:
                sideLeftAddon = addon;
                break;
            
            case AddonSlot.AddonSlotType.SideRight:
                sideRightAddon = addon;
                break;
            
            case AddonSlot.AddonSlotType.Top:
                topAddon = addon;
                break;
            
            case AddonSlot.AddonSlotType.Back:
                backAddon = addon;
                break;
        }
    }

    public void SetNewTire(CarBody.TireLocation location, Transform tireTransform)
    {
        switch (location)
        {
            case CarBody.TireLocation.FrontLeft:
                frontLeftTire = tireTransform;
                break;
            case CarBody.TireLocation.FrontRight:
                frontRightTire = tireTransform;
                break;
            case CarBody.TireLocation.BackLeft:
                backLeftTire = tireTransform;
                break;
            case CarBody.TireLocation.BackRight:
                backRightTire = tireTransform;
                break;
        }
        carBody.SetTireAndInstantiate(location, tireTransform);
    }

    private void InitializeUI()
    {
        selectorListUI = Instantiate(selectorListUITemplate, carAssembleUI);
        selectorListUI.ComponentsListSO = componentsListSO;
        selectorListUI.assembleController = this;
        player.BindSelectionInputActions(selectorListUI);
    }
    
    private void InitializeCar()
    {
        var carBodyTransform =
            Instantiate(componentsListSO.CarBodysList[0].transform, carSpawnTransform.position, Quaternion.identity);
        carBody = carBodyTransform.GetComponent<CarBody>();
    }
}
