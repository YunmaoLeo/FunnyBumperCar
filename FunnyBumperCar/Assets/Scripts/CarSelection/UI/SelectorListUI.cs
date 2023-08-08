using System.Collections.Generic;
using UnityEngine;

public class SelectorListUI : MonoBehaviour
{
    [SerializeField] private ComponentSelector componentSelectorTemplate;
    private List<ComponentSelector> componentSelectors = new List<ComponentSelector>();

    public CarAssembleController assembleController;

    private GameInputActions inputActions;
    [HideInInspector] public CarComponentsListSO ComponentsListSO;

    private int currentFocusIndex = 0;

    private void Awake()
    {
        inputActions = new GameInputActions();
        inputActions.Enable();

        inputActions.Selection.MoveUp.performed += (context => OnCursorUp());
        inputActions.Selection.MoveDown.performed += (context => OnCursorDown());
        inputActions.Selection.MoveRight.performed += (context => OnCursorRight());
        inputActions.Selection.MoveLeft.performed += (context => OnCursorLeft());

        inputActions.Selection.Select.performed += (context => OnSelect());
    }

    public void OnSelect()
    {
        componentSelectors[currentFocusIndex].OnSelect();
    }

    public void OnCursorRight()
    {
        componentSelectors[currentFocusIndex].onCursorRight();
    }

    public void OnCursorLeft()
    {
        componentSelectors[currentFocusIndex].onCursorLeft();
    }

    public void OnCursorUp()
    {
        var oldIndex = currentFocusIndex;
        currentFocusIndex -= 1;
        if (currentFocusIndex < 0)
        {
            currentFocusIndex += componentSelectors.Count;
        }

        componentSelectors[oldIndex].UnFocus();
        componentSelectors[currentFocusIndex].Focus();
    }

    public void OnCursorDown()
    {
        var oldIndex = currentFocusIndex;
        currentFocusIndex += 1;
        if (currentFocusIndex >= componentSelectors.Count)
        {
            currentFocusIndex -= componentSelectors.Count;
        }

        componentSelectors[oldIndex].UnFocus();
        componentSelectors[currentFocusIndex].Focus();
    }

    private void Start()
    {
        InitializeComponentsList();
    }

    private void InitializeComponentsList()
    {
        // CarBody;
        var carBodySelector = InitializeComponentSelector("CarBody", ComponentsListSO.CarBodysList, ComponentsListSO.CarBodysSprites);
        carBodySelector.OnSelectionAction += assembleController.SetNewCarBody;
        // FrontLeftTire:
        var frontLeftTireSelector =InitializeComponentSelector("FrontLeftTire", ComponentsListSO.LeftTiresList, ComponentsListSO.LeftTireSprites);
        frontLeftTireSelector.OnSelectionAction += tireTransform =>
        {
            assembleController.SetNewTire(CarBody.TireLocation.FrontLeft, tireTransform);
        };
        // FrontRightTire:
        var frontRightTireSelector =InitializeComponentSelector("FrontRightTire", ComponentsListSO.RightTiresList,
            ComponentsListSO.RightTireSprites);
        frontRightTireSelector.OnSelectionAction += tireTransform =>
        {
            assembleController.SetNewTire(CarBody.TireLocation.FrontRight, tireTransform);
        };
        // BackLeftTire:
        var backLeftTireSelector =InitializeComponentSelector("BackLeftTire", ComponentsListSO.LeftTiresList, ComponentsListSO.LeftTireSprites);
        // BackRightTire:
        backLeftTireSelector.OnSelectionAction += tireTransform =>
        {
            assembleController.SetNewTire(CarBody.TireLocation.BackLeft, tireTransform);
        };
        var backRightTireSelector =InitializeComponentSelector("BackRightTire", ComponentsListSO.RightTiresList,
            ComponentsListSO.RightTireSprites);
        backRightTireSelector.OnSelectionAction += tireTransform =>
        {
            assembleController.SetNewTire(CarBody.TireLocation.BackRight, tireTransform);
        };

        // AddonSlot
        var frontAddonSelector =InitializeComponentSelector("FrontAddon", ComponentsListSO.AddonsList, ComponentsListSO.AddonsSprites);
        frontAddonSelector.OnSelectionAction+= addonTransform =>
        {
            assembleController.SetNewAddon(AddonSlot.AddonSlotType.Front, addonTransform);
        };
        

        var sideLeftAddonSelector =InitializeComponentSelector("SideLeftAddon", ComponentsListSO.AddonsList, ComponentsListSO.AddonsSprites);
        sideLeftAddonSelector.OnSelectionAction+= addonTransform =>
        {
            assembleController.SetNewAddon(AddonSlot.AddonSlotType.SideLeft, addonTransform);
        };

        var sideRightAddonSelector =InitializeComponentSelector("SideRightAddon", ComponentsListSO.AddonsList, ComponentsListSO.AddonsSprites);
        sideRightAddonSelector.OnSelectionAction+= addonTransform =>
        {
            assembleController.SetNewAddon(AddonSlot.AddonSlotType.SideRight, addonTransform);
        };
        var backAddonSelector =InitializeComponentSelector("BackAddon", ComponentsListSO.AddonsList, ComponentsListSO.AddonsSprites);
        backAddonSelector.OnSelectionAction+= addonTransform =>
        {
            assembleController.SetNewAddon(AddonSlot.AddonSlotType.Back, addonTransform);
        };
        var topAddonSelector =InitializeComponentSelector("TopAddon", ComponentsListSO.AddonsList, ComponentsListSO.AddonsSprites);
        topAddonSelector.OnSelectionAction+= addonTransform =>
        {
            assembleController.SetNewAddon(AddonSlot.AddonSlotType.Top, addonTransform);
        };
    }

    private ComponentSelector InitializeComponentSelector(string componentName, List<Transform> addonsList,
        List<Sprite> spriteList)
    {
        var componentSelector = Instantiate(componentSelectorTemplate, transform);
        componentSelector.ComponentName.text = componentName;
        
        componentSelectors.Add(componentSelector);
        
        for (int i = 0; i < addonsList.Count; i++)
        {
            componentSelector.AddSelectableComponent(addonsList[i], spriteList[i]);
        }

        return componentSelector;
    }
}