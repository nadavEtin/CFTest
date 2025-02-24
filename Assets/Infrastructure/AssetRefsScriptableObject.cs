using UnityEngine;

namespace Assets.Infrastructure
{
    [CreateAssetMenu(fileName = "AssetRefs", menuName = "ScriptableObjects/Asset References")]
    public class AssetRefsScriptableObject : ScriptableObject
    {
        [SerializeField] private GameObject _normalBallPrefab, _specialBallPrefab, _factoiesManager, _ballSpawner;
        [Space(5)]
        [Header("UI")]
        [SerializeField] private GameObject _scorePanel;
        [SerializeField] private GameObject _targetScorePanel, _movesPanel, _timerPanel, _menuPanel;

        #region Public getters

        public GameObject NormalBallPrefab => _normalBallPrefab;
        public GameObject SpecialBallPrefab => _specialBallPrefab;
        public GameObject FactoriesManager => _factoiesManager;
        public GameObject BallSpawner => _ballSpawner;
        public GameObject ScorePanel => _scorePanel;
        public GameObject TargetScorePanel => _targetScorePanel;
        public GameObject MovesPanel => _movesPanel;
        public GameObject TimerPanel => _timerPanel;
        public GameObject MenuPanel => _menuPanel;

        #endregion

    }
}