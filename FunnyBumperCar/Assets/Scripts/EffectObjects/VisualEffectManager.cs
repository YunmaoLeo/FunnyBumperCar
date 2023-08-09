using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class VisualEffectManager : MonoBehaviour
{
    [SerializeField] private List<Transform> carCrashEffectLists;
    [SerializeField] private float baseCrashFactor = 20000f;
    [SerializeField] private float minCrashFactor = 5000f;
    [SerializeField] private float baseCrashPopUpTime = 0.5f;
    
    public static VisualEffectManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void PlayCarCrashEffectLists(Vector3 position, float factor = 1f)
    {
        if (factor < minCrashFactor)
        {
            return;
        }
        factor /= baseCrashFactor;
        if (carCrashEffectLists.Count != 0)
        {
            var effectPrefab = carCrashEffectLists[Random.Range(0, carCrashEffectLists.Count)];
            var effectTransform = Instantiate(effectPrefab, position, Quaternion.identity);
            effectTransform.localScale = Vector3.zero;
            effectTransform.DOScale(Vector3.one * factor, baseCrashPopUpTime / factor);
            Destroy(effectTransform.gameObject, 1f);
        }
    }
}
