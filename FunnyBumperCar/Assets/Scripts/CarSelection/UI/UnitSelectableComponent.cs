using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectableComponent : MonoBehaviour
{
    public Transform IsFocused;
    public Transform IsSelected;
    public Image ComponentSprite;


    public void Select()
    {
        IsSelected.gameObject.SetActive(true);
    }

    public void UnSelect()
    {
        IsSelected.gameObject.SetActive(false);
    }

    public void Focus()
    {
        IsFocused.gameObject.SetActive(true);
    }

    public void UnFocus()
    {
        IsFocused.gameObject.SetActive(false);
    }

    public void SetImage(Sprite sprite)
    {
        ComponentSprite.sprite = sprite;
    }
}
