using System;
using Unity.VisualScripting;
using UnityEngine;

public class SoccerGoal : MonoBehaviour
{
    public Action<int> OnPlayerGetScore;

    public int PlayerIndex;

    private void OnTriggerEnter(Collider other)
    {
        var soccer = other.GetComponentInParent<Soccer>();
        if (soccer == null)
        {
            return;
        }

        if (soccer.isActive)
        {
            OnPlayerGetScore?.Invoke(PlayerIndex);
        }
    }
}