using Assets.Infrastructure;
using Assets.Infrastructure.Events;
using Assets.Infrastructure.Factories;
using Assets.Scripts.Utility;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace GameCore.UI
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        private IFactoriesManager _factoriesManager;
        private GameObject _scorePanel, _targetScorePanel, _movesPanel, _timerPanel;
        private GameObject _menuPanel;      //shows the avatar, button is just a mock and not active
        private PopupMessage _messageWindow;
        private Camera _mainCamera;

        private TextMeshProUGUI _scoreText, _targetScoreText, _movesText, _timerText;
        private AssetRefsScriptableObject _assetRefs;
        private int _score, _targetScore, _movesLeft, _timeLeft;

        public void Init(AssetRefsScriptableObject assetRefs, Camera mainMacera, IFactoriesManager factoriesManager, int targetScore, int startingMoves, int startingTime)
        {            
            _assetRefs = assetRefs;
            _mainCamera = mainMacera;
            _factoriesManager = factoriesManager;
            _targetScore = targetScore;
            _movesLeft = startingMoves;
            _timeLeft = startingTime;
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            EventManager.Instance.Subscribe(TypeOfEvent.ScoreUpdate, ScoreUpdate);
            EventManager.Instance.Subscribe(TypeOfEvent.BallClick, MovesUpdate);
            EventManager.Instance.Subscribe(TypeOfEvent.TimeUpdate, TimeUpdate);
            EventManager.Instance.Subscribe(TypeOfEvent.GameStart, GameStart);
            EventManager.Instance.Subscribe(TypeOfEvent.MissedMove, ShowMissPopup);
        }

        private void Start()
        {
            SetupGameplayUI();
            ShowMessageWindow();
        }        

        private void ShowMessageWindow()
        {
            _messageWindow = Instantiate(_assetRefs.MessageWindow, transform).GetComponent<PopupMessage>();
            _messageWindow.PopupText.text = $"Reach {_targetScore} points before your time or moves run out!";
            _messageWindow.ShowWindow();
        }

        private void GameStart(BaseEventParams eventParams)
        {
            ShowHideGameplayUI(true);
        }

        private void SetupGameplayUI()
        {
            _scorePanel = Instantiate(_assetRefs.ScorePanel, transform);
            _targetScorePanel = Instantiate(_assetRefs.TargetScorePanel, transform);
            _movesPanel = Instantiate(_assetRefs.MovesPanel, transform);
            _timerPanel = Instantiate(_assetRefs.TimerPanel, transform);
            _menuPanel = Instantiate(_assetRefs.MenuPanel, transform);
            _scoreText = _scorePanel.GetComponentInChildren<TextMeshProUGUI>();
            _targetScoreText = _targetScorePanel.GetComponentInChildren<TextMeshProUGUI>();
            _movesText = _movesPanel.GetComponentInChildren<TextMeshProUGUI>();
            _timerText = _timerPanel.GetComponentInChildren<TextMeshProUGUI>();

            _scoreText.text = $"Score: {_score}";
            _targetScoreText.text = $"/  {_targetScore}";
            _movesText.text = $"Moves: {_movesLeft}";
            _timerText.text = $"Time left: {_timeLeft}";

            ShowHideGameplayUI(false);  //hide ui until game starts
        }

        private void ShowHideGameplayUI(bool show)
        {
            _scorePanel.SetActive(show);
            _targetScorePanel.SetActive(show);
            _movesPanel.SetActive(show);
            //_timerPanel.SetActive(show);
            _menuPanel.SetActive(show);
        }

        private void ShowMissPopup(BaseEventParams eventParams)
        {
            var pos = ((MissedMoveEventParams)eventParams).BallWorldPosition;
            var canvasPos = _mainCamera.WorldToScreenPoint(pos);
            var missPopup = _factoriesManager.GetObject(FactoryType.MissPopup, 1, transform);
            missPopup[0].GetComponent<MissPopup>().ShowPopup(canvasPos);
        }

        private void ScoreUpdate(BaseEventParams eventParams)
        {
            var score = ((ScoreUpdateEventParams)eventParams).Score;
            _score += score;
            _scoreText.text = $"Score: {_score}";
        }

        private void MovesUpdate(BaseEventParams eventParams)
        {
            _movesLeft -= 1;
            _movesText.text = $"Moves: {_movesLeft}";
        }

        private void TimeUpdate(BaseEventParams eventParams)
        {
            var time = ((TimeUpdateEventParams)eventParams).TimeLeft;
            _timerText.text = $"Time left: {time}";
        }

        private void OnDestroy()
        {
            EventManager.Instance.Unsubscribe(TypeOfEvent.ScoreUpdate, ScoreUpdate);
            EventManager.Instance.Unsubscribe(TypeOfEvent.BallClick, MovesUpdate);
            EventManager.Instance.Unsubscribe(TypeOfEvent.TimeUpdate, TimeUpdate);
            EventManager.Instance.Unsubscribe(TypeOfEvent.GameStart, GameStart);
            EventManager.Instance.Unsubscribe(TypeOfEvent.MissedMove, ShowMissPopup);
        }
    }
}
