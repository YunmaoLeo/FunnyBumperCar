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
        InitializeComponentSelector("CarBody", ComponentsListSO.CarBodysList, ComponentsListSO.CarBodysSprites);

        // FrontLeftTire:
        InitializeComponentSelector("FrontLeftTire", ComponentsListSO.LeftTiresList, ComponentsListSO.LeftTireSprites);
        // FrontRightTire:
        InitializeComponentSelector("FrontRightTire", ComponentsListSO.RightTiresList,
            ComponentsListSO.RightTireSprites);
        // BackLeftTire:
        InitializeComponentSelector("BackLeftTire", ComponentsListSO.LeftTiresList, ComponentsListSO.LeftTireSprites);
        // BackRightTire:
        InitializeComponentSelector("BackRightTire", ComponentsListSO.RightTiresList,
            ComponentsListSO.RightTireSprites);

        // AddonSlot
        InitializeComponentSelector("FrontAddon", ComponentsListSO.AddonsList, ComponentsListSO.AddonsSprites);

        InitializeComponentSelector("SideLeftAddon", ComponentsListSO.AddonsList, ComponentsListSO.AddonsSprites);

        InitializeComponentSelector("SideRightAddon", ComponentsListSO.AddonsList, ComponentsListSO.AddonsSprites);

        InitializeComponentSelector("BackAddon", ComponentsListSO.AddonsList, ComponentsListSO.AddonsSprites);

        InitializeComponentSelector("TopAddon", ComponentsListSO.AddonsList, ComponentsListSO.AddonsSprites);
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