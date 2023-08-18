
    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class StartManager : MonoBehaviour
    {
        [SerializeField] private Button FootballButton;

        private void Start()
        {
            FootballButton.onClick.AddListener(
                delegate
                {
                    SceneManager.LoadScene("CarSelectionScene");
                });
        }
    }
