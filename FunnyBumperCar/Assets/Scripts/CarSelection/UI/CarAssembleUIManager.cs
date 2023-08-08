using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAssembleUIManager : MonoBehaviour
{
    public static CarAssembleUIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
