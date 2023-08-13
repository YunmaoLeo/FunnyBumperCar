using System;
using UnityEngine;

public class SoccerGoal : MonoBehaviour
{
    public Action<int> OnFootballInGoal;
    
    public int PlayerIndex;

    private void OnTriggerEnter(Collider other)
    {
        
    }
}