﻿using Assets.Infrastructure.Events;
using UnityEngine;

namespace Assets.GameRules
{
    public class RulesTracker : MonoBehaviour, IRulesTracker
    {
        private GameRulesScriptableObject _gameRules;
        private int _currentScore, _targetScore, _tapsLeft, _timeLeft;

        public void Init(GameRulesScriptableObject gameRules)
        {
            _gameRules = gameRules;

            _tapsLeft = _gameRules.MaxMoves;
            _timeLeft = _gameRules.TimeLimit;
            _targetScore = _gameRules.TargetScore;

            EventManager.Instance.Subscribe(TypeOfEvent.GameStart, GameStarted);
            EventManager.Instance.Subscribe(TypeOfEvent.BallClick, MovesUpdate);
            EventManager.Instance.Subscribe(TypeOfEvent.ScoreUpdate, ScoreUpdate);
        }

        private void GameStarted(BaseEventParams eventParams)
        {
            _currentScore = 0;
            _tapsLeft = _gameRules.MaxMoves;
            _timeLeft = _gameRules.TimeLimit;
            _targetScore = _gameRules.TargetScore;
            TimerCountdown();
        }

        private void TimerCountdown()
        {
            _timeLeft -= 1;
            if (_timeLeft > 0)
            {
                Invoke("TimerCountdown", 1f);
                EventManager.Instance.Publish(TypeOfEvent.TimeUpdate, new TimeUpdateEventParams(_timeLeft));
            }
            else
            {
                //trigger game over event
                EventManager.Instance.Publish(TypeOfEvent.TimeUpdate, new TimeUpdateEventParams(0));
                EventManager.Instance.Publish(TypeOfEvent.GameOver, new GameOverEventParams(_currentScore));
            }
        }

        private void MovesUpdate(BaseEventParams eventParams)
        {
            _tapsLeft -= 1;
            if (_tapsLeft <= 0)         //trigger game over event
                EventManager.Instance.Publish(TypeOfEvent.GameOver, new GameOverEventParams(_currentScore));            
        }

        private void ScoreUpdate(BaseEventParams eventParams)
        {
            var scoreParams = (ScoreUpdateEventParams)eventParams;
            _currentScore += scoreParams.Score;
            if (_currentScore >= _targetScore)
                EventManager.Instance.Publish(TypeOfEvent.ScoreTargetReached, new EmptyParams());
        }

        private void OnDestroy()
        {
            EventManager.Instance.Unsubscribe(TypeOfEvent.GameStart, GameStarted);
            EventManager.Instance.Unsubscribe(TypeOfEvent.BallClick, MovesUpdate);
            EventManager.Instance.Unsubscribe(TypeOfEvent.ScoreUpdate, ScoreUpdate);
        }
    }
}
