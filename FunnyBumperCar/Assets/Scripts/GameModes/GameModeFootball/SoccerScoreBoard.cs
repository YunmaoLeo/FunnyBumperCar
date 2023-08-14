using TMPro;
using UnityEngine;

public class SoccerScoreBoard : MonoBehaviour
{
    [HideInInspector] public int Player1Score;
    [HideInInspector] public int Player2Score;

    [SerializeField] private TextMeshProUGUI p1ScoreText;
    [SerializeField] private TextMeshProUGUI p2ScoreText;
    
    public void UpdateScore(int p1Score, int p2Score)
    {
        Player1Score = p1Score;
        Player2Score = p2Score;
        
        UpdateScreen();
    }

    private void UpdateScreen()
    {
        p1ScoreText.text = Player1Score.ToString();
        p2ScoreText.text = Player2Score.ToString();
    }
}