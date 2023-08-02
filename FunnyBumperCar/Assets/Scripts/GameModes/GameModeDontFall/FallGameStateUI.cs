using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FallGameStateUI : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> playerRemainFallTimeText;

    private void Start()
    {
        
    }

    public void UpdatePlayerRemainFallTime(List<PlayerDontFall> playerList)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerRemainFallTimeText[i].text = playerList[i].RemainFallDownTime.ToString();
        }
    }
}
