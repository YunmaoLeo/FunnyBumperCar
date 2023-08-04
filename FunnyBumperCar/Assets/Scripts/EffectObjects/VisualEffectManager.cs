using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VisualEffectManager : MonoBehaviour
{
    [SerializeField] private List<Transform> carCrashEffectLists;

    public static VisualEffectManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void PlayCarCrashEffectLists(Vector3 position, float factor = 1f)
    {
        if (carCrashEffectLists.Count != 0)
        {
            var effectPrefab = carCrashEffectLists[Random.Range(0, carCrashEffectLists.Count)];
            var effectTransform = Instantiate(effectPrefab, position, Quaternion.identity);
            Destroy(effectTransform.gameObject, 1f);
        }
    }
}
