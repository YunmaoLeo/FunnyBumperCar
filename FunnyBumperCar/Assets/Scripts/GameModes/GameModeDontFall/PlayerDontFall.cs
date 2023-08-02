using Unity.VisualScripting;
using UnityEngine;

public class PlayerDontFall : PlayerBase
{
    public int RemainFallDownTime;
    public PlayerDontFall(int maxFallDownTime = 3)
    {
        RemainFallDownTime = maxFallDownTime;
    }
}