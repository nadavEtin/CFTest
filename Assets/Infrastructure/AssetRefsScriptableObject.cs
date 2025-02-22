using UnityEngine;

namespace Assets.Infrastructure
{
    [CreateAssetMenu(fileName = "AssetRefs", menuName = "ScriptableObjects/Asset References")]
    public class AssetRefsScriptableObject : ScriptableObject
    {
        [SerializeField] private GameObject _normalBallPrefab, _factoiesManager, _ballSpawner;
        public GameObject NormalBallPrefab => _normalBallPrefab;
        public GameObject FactoriesManager => _factoiesManager;
        public GameObject BallSpawner => _ballSpawner;
    }
}