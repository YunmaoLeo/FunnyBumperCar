
    using TMPro;
    using UnityEngine;

    public class FootballGameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI winnerText;

        public void SetWinnerText(int playerIndex)
        {
            winnerText.text = "Winner: Player" + (playerIndex + 1);
        }
    }
