using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CarsManager : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cameraTargetGroup;
    public static CarsManager Instance { get; private set; }

    private Transform P1CarTransform;
    private Transform P2CarTransform;
    
    private void Awake()
    {
        Instance = this;
    }

    public void RegisterCar(Transform car, int playerIndex)
    {
        if (playerIndex == 0)
        {
            P1CarTransform = car;
        }
        else
        {
            P2CarTransform = car;
        }
        
        cameraTargetGroup.AddMember(car,1,10);
    }

    public Transform GetHostileCar(Transform car)
    {
        return P1CarTransform == car ? P2CarTransform : P1CarTransform;
    }
}
