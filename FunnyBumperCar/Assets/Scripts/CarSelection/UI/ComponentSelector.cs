using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComponentSelector : MonoBehaviour
{
    private const int NO_SELECTION = -1;
    public TextMeshProUGUI ComponentName;
    public GridLayoutGroup ListOfComponents;
    [SerializeField] private UnitSelectableComponent UnitSelectableComponentTemplate;
    [SerializeField] private int maxRenderCount = 4;
    private List<Sprite> componentSprites = new List<Sprite>();
    private List<Transform> componentTransforms = new List<Transform>();
    private List<UnitSelectableComponent> components = new List<UnitSelectableComponent>();
    
    public Action<Transform> OnSelectionAction;

    private int currentFocusIndex = 0;
    private int currentSelectionIndex = NO_SELECTION;

    private bool isFocused = false;

    public void OnSelect()
    {
        var oldIndex = currentSelectionIndex;
        currentSelectionIndex = currentFocusIndex;
        components[currentSelectionIndex].Select();
        OnSelectionAction?.Invoke(componentTransforms[currentSelectionIndex]);
        if (oldIndex != NO_SELECTION)
        {
            components[oldIndex].UnSelect();
        }
    }
    
    public void onCursorLeft()
    {
        var oldIndex = currentFocusIndex;
        currentFocusIndex--;
        if (currentFocusIndex < 0)
        {
            currentFocusIndex += components.Count;
        }
        components[oldIndex].UnFocus();
        components[currentFocusIndex].Focus();
    }

    public void onCursorRight()
    {
        var oldIndex = currentFocusIndex;
        currentFocusIndex++;
        if (currentFocusIndex >= components.Count)
        {
            currentFocusIndex -= components.Count;
        }
        components[oldIndex].UnFocus();
        components[currentFocusIndex].Focus();
    }
    public void Focus()
    {
        isFocused = true;
        components[currentFocusIndex].Focus();
    }

    public void UnFocus()
    {
        isFocused = false;
        components[currentFocusIndex].UnFocus();
    }
    
    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        for (int i = 0; i < componentTransforms.Count; i++)
        {
            var component = Instantiate(UnitSelectableComponentTemplate, ListOfComponents.transform);
            components.Add(component);
            component.SetImage(componentSprites[i]);
        }
    }

    public void AddSelectableComponent(Transform carComponent, Sprite sprite)
    {
        componentSprites.Add(sprite);
        componentTransforms.Add(carComponent);
    }
}
