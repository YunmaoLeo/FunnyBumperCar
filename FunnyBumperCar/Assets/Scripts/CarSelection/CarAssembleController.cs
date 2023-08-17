using Unity.VisualScripting;
using UnityEngine;

public class CarAssembleController : MonoBehaviour
{
    [SerializeField] private CarComponentsListSO componentsListSO;
    [SerializeField] private SelectorListUI selectorListUITemplate;
    [HideInInspector] public Transform carSpawnTransform;

    private SelectorListUI selectorListUI;
    public Player player;
    private Transform carAssembleUI;
    private CarPresentPlatform presentPlatform;

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
        presentPlatform = carSpawnTransform.parent.GetComponent<CarPresentPlatform>();
    }

    public Transform GetCar()
    {
        return carBody.transform;
    }

    public void SetNewCarBody(Transform newCar)
    {
        if (newCar.TryGetComponent<CarBody>(out CarBody bodyComponent))
        {
            if (carBody.CarName == bodyComponent.CarName)
            {
                return;
            }

            Destroy(carBody.gameObject);
            var carBodyTransform =
                Instantiate(bodyComponent.transform, carSpawnTransform);
            carBody = carBodyTransform.GetComponent<CarBody>();
            carBody.PlayerIndex = player.playerIndex;
            carBody.playerIndicatorTargetCamera = Camera.main;
            
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
        switch (slotType)
        {
            case AddonSlot.AddonSlotType.Front:
                frontAddon = addon;
                presentPlatform.DoShowFront();
                break;
            case AddonSlot.AddonSlotType.SideLeft:
                sideLeftAddon = addon;
                presentPlatform.DoShowSideLeft();
                break;

            case AddonSlot.AddonSlotType.SideRight:
                sideRightAddon = addon;
                presentPlatform.DoShowSideRight();
                break;

            case AddonSlot.AddonSlotType.Top:
                topAddon = addon;
                break;

            case AddonSlot.AddonSlotType.Back:
                backAddon = addon;
                presentPlatform.DoShowBack();
                break;
        }

        if (carBody.EquipCarAddon(slotType, addon))
        {
            ResetCarState();
        }
    }

    public void SetNewTire(CarBody.TireLocation location, Transform tireTransform)
    {
        switch (location)
        {
            case CarBody.TireLocation.FrontLeft:
                frontLeftTire = tireTransform;
                presentPlatform.DoShowSideLeft();
                break;
            case CarBody.TireLocation.FrontRight:
                presentPlatform.DoShowSideRight();
                frontRightTire = tireTransform;
                break;
            case CarBody.TireLocation.BackLeft:
                presentPlatform.DoShowSideLeft();
                backLeftTire = tireTransform;
                break;
            case CarBody.TireLocation.BackRight:
                presentPlatform.DoShowSideRight();
                backRightTire = tireTransform;
                break;
        }

        if (carBody.SetTireAndInstantiate(location, tireTransform))
        {
            ResetCarState();
        }
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
            Instantiate(componentsListSO.CarBodysList[0].transform, carSpawnTransform);
        carBody = carBodyTransform.GetComponent<CarBody>();
        carBody.PlayerIndex = player.playerIndex;
        carBody.playerIndicatorTargetCamera = Camera.main;
    }

    private void ResetCarState()
    {
        carBody.transform.localPosition = Vector3.zero;
        carBody.transform.localRotation = Quaternion.identity;
        carBody.ResetPhysicalState();
    }
}