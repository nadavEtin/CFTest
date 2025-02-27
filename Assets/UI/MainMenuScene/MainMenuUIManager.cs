using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.UI.MenuScene
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button playButton;
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private GameObject settingsMenu;

        private void Awake()
        {
            //load previous high score, 0 for default
            var highScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = $"High Score: {highScore}";
        }

        public void OnSettingsButtonClick()
        {
            settingsMenu.SetActive(true);
        }

        public void OnPlayButtonClick()
        {
            try
            {
                SceneManager.LoadScene("Gameplay");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load gameplay scene: {e.Message}");
            }
        }
    }
}