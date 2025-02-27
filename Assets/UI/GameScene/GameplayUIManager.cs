using Assets.Infrastructure;
using Assets.Infrastructure.Events;
using Assets.Infrastructure.Factories;
using Assets.UI.GameScene;
using TMPro;
using UnityEngine;

namespace GameCore.UI
{
    public class GameplayUIManager : MonoBehaviour
    {
        private IFactoriesManager _factoriesManager;
        private IUIGameplayPanel _scorePanel, _targetScorePanel, _movesPanel, _timerPanel;
        private GameObject _menuPanel;      //shows the avatar, button is just a mock and not active
        private GameObject _clickBlocker;
        private PopupMessage _messageWindow, _gameOverWindow;
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
            _clickBlocker = Instantiate(_assetRefs.ClickBlocker, transform);
            _clickBlocker.SetActive(false);
            SubscribeToEvents();
        }

        public void ShowEndGamePopup(bool win)
        {
            _clickBlocker.SetActive(true);
            if (_gameOverWindow == null)
                _gameOverWindow = Instantiate(_assetRefs.GameOverWindow, transform).GetComponent<PopupMessage>();

            var winText = "Great job! You've reached the target score!";
            var loseText = "Better luck next time";
            _gameOverWindow.PopupText.text = win ? winText : loseText;
            _gameOverWindow.ShowWindow();
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
            if (_messageWindow == null)
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
            _scorePanel = Instantiate(_assetRefs.ScorePanel, transform).GetComponent<IUIGameplayPanel>();
            _targetScorePanel = Instantiate(_assetRefs.TargetScorePanel, transform).GetComponent<IUIGameplayPanel>();
            _movesPanel = Instantiate(_assetRefs.MovesPanel, transform).GetComponent<IUIGameplayPanel>();
            _timerPanel = Instantiate(_assetRefs.TimerPanel, transform).GetComponent<IUIGameplayPanel>();
            _menuPanel = Instantiate(_assetRefs.MenuPanel, transform);
            _scoreText = _scorePanel.PanelText;
            _targetScoreText = _targetScorePanel.PanelText;
            _movesText = _movesPanel.PanelText;
            _timerText = _timerPanel.PanelText;

            _scoreText.text = $"Score: {_score}";
            _targetScoreText.text = $"/  {_targetScore}";
            _movesText.text = $"Moves: {_movesLeft}";
            _timerText.text = $"Time left: {_timeLeft}";

            ShowHideGameplayUI(false);  //hide ui until game starts
        }

        private void ShowHideGameplayUI(bool show)
        {
            if (show)
            {
                _timerPanel.MoveIntoView();
                _movesPanel.MoveIntoView();
                _scorePanel.MoveIntoView();
                _targetScorePanel.MoveIntoView();
            }
            else
            {
                _timerPanel.PositionOutOfView(false);
                _movesPanel.PositionOutOfView(false);
                _scorePanel.PositionOutOfView(true);
                _targetScorePanel.PositionOutOfView(true);
            }

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
