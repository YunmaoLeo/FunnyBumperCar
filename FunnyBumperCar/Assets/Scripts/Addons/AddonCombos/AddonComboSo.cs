using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AddonComboSo : ScriptableObject
{
    [SerializeField] private List<AddonObject.AddonObjectEnum> RequiredAddons;
    [SerializeField] private List<int> RequiredAddonsCount;

    public Dictionary<AddonObject.AddonObjectEnum, int> RequiredAddonsDict = new Dictionary<AddonObject.AddonObjectEnum, int>();

    private void Awake()
    {
        for (int i = 0; i < RequiredAddons.Count; i++)
        {
            RequiredAddonsDict.Add(RequiredAddons[i], RequiredAddonsCount[i]);
        }
    }

    public bool IsComboValid(Dictionary<AddonObject.AddonObjectEnum, int> addonDict)
    {
        foreach (var pair in RequiredAddonsDict)
        {
            if (!addonDict.ContainsKey(pair.Key) || addonDict[pair.Key] < pair.Value)
            {
                return false;
            }
        }

        return true;
    }

    public virtual void TriggerComboFunction()
    {
        
    }
}