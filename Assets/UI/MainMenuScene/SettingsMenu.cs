using Assets.GameRules;
using Assets.Infrastructure.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private bool _isMusicOn = true;
    private const string _playerPrefsDifficulty = "GameDifficulty";
    private const string _playerPrefsMusicSetting = "MusicEnabled";
    private GameDifficulty _currentDifficulty = GameDifficulty.Normal;

    [SerializeField] private GameObject _settingsPanel;

    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _easyButton;
    [SerializeField] private Button _normalButton;
    [SerializeField] private Button _hardButton;
    [SerializeField] private Sprite _greyBtn, _whiteBtn;

    private void Awake()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        //load music setting (default to on if not saved)
        _isMusicOn = PlayerPrefs.GetInt(_playerPrefsMusicSetting, 1) == 1;
        UpdateMusicVisuals();

        //load difficulty setting (default to normal if not saved)
        _currentDifficulty = (GameDifficulty)PlayerPrefs.GetInt(_playerPrefsDifficulty, (int)GameDifficulty.Normal);
        UpdateDifficultyVisuals();
    }

    public void OnMusicButtonClick()
    {
        _isMusicOn = !_isMusicOn;
        UpdateMusicVisuals();
        SaveSettings();
    }

    public void OnEasyButtonClick()
    {
        _currentDifficulty = GameDifficulty.Easy;
        UpdateDifficultyVisuals();
    }

    public void OnNormalButtonClick()
    {
        _currentDifficulty = GameDifficulty.Normal;
        UpdateDifficultyVisuals();
    }

    public void OnHardButtonClick()
    {
        _currentDifficulty = GameDifficulty.Hard;
        UpdateDifficultyVisuals();
    }

    public void OnCloseButtonClick()
    {
        //save changes just to be safe
        SaveSettings();

        _settingsPanel.SetActive(false);        
    }

    public void OpenSettingsMenu()
    {
        _settingsPanel.SetActive(true);        
    }

    private void UpdateMusicVisuals()
    {
        if (_musicButton != null)
        {
            //change button color to indicate current state
            ColorBlock cb = _musicButton.colors;
            cb.normalColor = _isMusicOn ? Color.white : Color.gray;
            _musicButton.colors = cb;
            if(_isMusicOn)
                EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void UpdateDifficultyVisuals()
    {
        _easyButton.image.sprite = _whiteBtn;
        _normalButton.image.sprite = _whiteBtn;
        _hardButton.image.sprite = _whiteBtn;
        switch (_currentDifficulty)
        {
            case GameDifficulty.Easy:
                _easyButton.image.sprite = _greyBtn; 
                break;
            case GameDifficulty.Normal:
                _normalButton.image.sprite = _greyBtn; 
                break;
            case GameDifficulty.Hard:
                _hardButton.image.sprite = _greyBtn;
                break;
        }

        //update the difficulty in the SO
        EventManager.Instance.Publish(TypeOfEvent.DifficultySelection, new DifficultySeletionEventParams(_currentDifficulty));
        SaveSettings();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt(_playerPrefsMusicSetting, _isMusicOn ? 1 : 0);
        PlayerPrefs.SetInt(_playerPrefsDifficulty, (int)_currentDifficulty);
        PlayerPrefs.Save();
    }
}