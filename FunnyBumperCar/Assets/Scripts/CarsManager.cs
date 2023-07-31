using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsManager : MonoBehaviour
{
    public static CarsManager Instance { get; private set; }

    public Transform P1CarTransform;
    public Transform P2CarTransform;
    
    private void Awake()
    {
        Instance = this;
    }

    public Transform GetHostileCar(Transform car)
    {
        return P1CarTransform == car ? P2CarTransform : P1CarTransform;
    }
}
