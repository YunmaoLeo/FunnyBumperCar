using System;
using Unity.VisualScripting;
using UnityEngine;

public class SoccerGoal : MonoBehaviour
{
    public Action<int> OnPlayerGetScore;

    public int PlayerIndex;
    public ParticleSystem OnGoalFX;

    private void Start()
    {
        OnGoalFX.Stop();
    }

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
            OnGoalFX.Play();
        }
    }
}