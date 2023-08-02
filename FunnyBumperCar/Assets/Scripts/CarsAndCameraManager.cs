using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CarsAndCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cameraTargetGroup;
    public static CarsAndCameraManager Instance { get; private set; }

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

    public void RegisterVisualEffect(Transform effect, float time = 1f)
    {
        cameraTargetGroup.AddMember(effect, 1f, 10);
        StartCoroutine(RemoveTransformAfterCertainTime(effect, time * 0.8f));
    }

    IEnumerator RemoveTransformAfterCertainTime(Transform transform, float delay)
    {
        yield return new WaitForSeconds(delay);
        cameraTargetGroup.RemoveMember(transform);
    }

    public Transform GetHostileCar(Transform car)
    {
        return P1CarTransform == car ? P2CarTransform : P1CarTransform;
    }
}
