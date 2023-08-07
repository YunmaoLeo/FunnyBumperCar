using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarAssembleController : MonoBehaviour
{
    [SerializeField] private CarComponentsListSO componentsListSO;
    [SerializeField] private Transform carSpawnTransform;
    [SerializeField] private SelectorListUI selectorListUITemplate;
    [SerializeField] private Transform carAssembleUI;

    private CarBody carBody;
    private TirePhysics frontLeftTire;
    private TirePhysics frontRightTire;
    private TirePhysics backLeftTire;
    private TirePhysics backRightTire;
    
    private AddonContainer_Car frontAddon;
    private AddonContainer_Car sideLeftAddon;
    private AddonContainer_Car sideRightAddon;
    private AddonContainer_Car BackAddon;
    private AddonContainer_Car TopAddon;

    private void Start()
    {
        InitializeCar();
        InitializeUI();
    }

    private void InitializeUI()
    {
        var selectorList = Instantiate(selectorListUITemplate, carAssembleUI);
        selectorList.ComponentsListSO = componentsListSO;
        selectorList.assembleController = this;
    }
    
    private void InitializeCar()
    {
        var carBodyTransform =
            Instantiate(componentsListSO.CarBodysList[0].transform, carSpawnTransform.position, Quaternion.identity);
        carBody = carBodyTransform.GetComponent<CarBody>();
    }
}
