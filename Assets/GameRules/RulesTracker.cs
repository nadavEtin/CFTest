using UnityEngine;

namespace Assets.GameRules
{
    public class RulesTracker : MonoBehaviour
    {
        private GameRulesScriptableObject _gameRules;

        private int _currentScore, _taargetScore, _tapsLeft;
        private float _timeLeft;

        public void Init(GameRulesScriptableObject gameRules)
        {
            _gameRules = gameRules;
            _tapsLeft = _gameRules.MaxTaps;
            _timeLeft = _gameRules.TimeLimit;
            _taargetScore = _gameRules.TargetScore;
            TimerCountdown();
        }

        private void TimerCountdown()
        {
            _timeLeft -= 1f;
            if (_timeLeft > 0)
            {
                Invoke("TimerCountdown", 1f);

            }
            
        }
    }
}
