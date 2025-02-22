using UnityEngine;

namespace Assets.Infrastructure
{
    [CreateAssetMenu(fileName = "AssetRefs", menuName = "ScriptableObjects/Asset References")]
    public class AssetRefsScriptableObject : ScriptableObject
    {
        [SerializeField] private GameObject _normalBallPrefab, _factoiesManager, _ballSpawner;
        [SerializeField] private GameObject _scorePanel, _movesPanel, _timerPanel;
        public GameObject NormalBallPrefab => _normalBallPrefab;
        public GameObject FactoriesManager => _factoiesManager;
        public GameObject BallSpawner => _ballSpawner;
        public GameObject ScorePanel => _scorePanel;
        public GameObject MovesPanel => _movesPanel;
        public GameObject TimerPanel => _timerPanel;
    }
}