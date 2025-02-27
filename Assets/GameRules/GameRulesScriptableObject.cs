using Assets.Infrastructure.Events;
using UnityEngine;

namespace Assets.GameRules
{
    public enum GameDifficulty
    {
        Easy,
        Normal,
        Hard
    }

    [CreateAssetMenu(fileName = "GameRules", menuName = "ScriptableObjects/Game rules")]
    public class GameRulesScriptableObject : ScriptableObject
    {
        [SerializeField] private GameDifficulty _currentDifficulty;
        [Header("Easy Difficulty")]
        [SerializeField] private int _easyTimeLimit;
        [SerializeField] private int _easyTargetScore;
        [SerializeField] private int _easyMaxMoves;
        [Space(5)]
        [Header("Normal Difficulty")]
        [SerializeField] private int _normalTimeLimit;
        [SerializeField] private int _normalTargetScore;
        [SerializeField] private int _normalMaxMoves;
        [Space(5)]
        [Header("Hard Difficulty")]
        [SerializeField] private int _hardTimeLimit;
        [SerializeField] private int _hardTargetScore;
        [SerializeField] private int _hardMaxMoves;

        [HideInInspector] public int TimeLimit, TargetScore, MaxMoves;

        private void OnEnable()
        {
            EventManager.Instance.Subscribe(TypeOfEvent.DifficultySelection, UpdateDifficulty);
        }

        private void UpdateDifficulty(BaseEventParams eventParams)
        {
            _currentDifficulty = ((DifficultySeletionEventParams)eventParams).CurrentDifficulty;
            SetDifficultySettings();
        }

        //sets the game rules based on the current difficulty. default is Normal
        public void SetDifficultySettings()
        {
            switch (_currentDifficulty)
            {
                case GameDifficulty.Easy:
                    TimeLimit = _easyTimeLimit;
                    TargetScore = _easyTargetScore;
                    MaxMoves = _easyMaxMoves;
                    break;
                case GameDifficulty.Normal:
                    TimeLimit = _normalTimeLimit;
                    TargetScore = _normalTargetScore;
                    MaxMoves = _normalMaxMoves;
                    break;
                case GameDifficulty.Hard:
                    TimeLimit = _hardTimeLimit;
                    TargetScore = _hardTargetScore;
                    MaxMoves = _hardMaxMoves;
                    break;
            }
        }

        private void SetDifficulty(GameDifficulty difficulty)
        {
            _currentDifficulty = difficulty;
            SetDifficultySettings();
        }
    }
}