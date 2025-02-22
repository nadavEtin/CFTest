using Assets.Infrastructure;
using Assets.Infrastructure.Events;
using Assets.Scripts.Utility;
using TMPro;
using UnityEngine;

namespace GameCore.UI
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        private GameObject _scorePanel, _movesPanel, _timerPanel;

        private TextMeshProUGUI _scoreText, _movesText, _timerText;
        private AssetRefsScriptableObject _assetRefs;
        private int _score, _movesLeft, _timeLeft;

        public void Init(AssetRefsScriptableObject assetRefs, int startingMoves, int startingTime)
        {            
            _assetRefs = assetRefs;
            _movesLeft = startingMoves;
            _timeLeft = startingTime;
            SetupGameplayUI();
            EventManager.Instance.Subscribe(TypeOfEvent.ScoreUpdate, ScoreUpdate);
            EventManager.Instance.Subscribe(TypeOfEvent.BallClick, MovesUpdate);
            EventManager.Instance.Subscribe(TypeOfEvent.TimeUpdate, TimeUpdate);
        }

        private void SetupGameplayUI()
        {
            _scorePanel = Instantiate(_assetRefs.ScorePanel, transform);
            _movesPanel = Instantiate(_assetRefs.MovesPanel, transform);
            _timerPanel = Instantiate(_assetRefs.TimerPanel, transform);
            _scoreText = _scorePanel.GetComponentInChildren<TextMeshProUGUI>();
            _movesText = _movesPanel.GetComponentInChildren<TextMeshProUGUI>();
            _timerText = _timerPanel.GetComponentInChildren<TextMeshProUGUI>();

            _scoreText.text = $"Score: {_score}";
            _movesText.text = $"Moves: {_movesLeft}";
            _timerText.text = $"Time left: {_timeLeft}";
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
        }
    }
}
