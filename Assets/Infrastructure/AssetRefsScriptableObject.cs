using UnityEngine;

namespace GameCore.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AssetRefs", menuName = "ScriptableObjects/Asset References")]
    public class AssetRefsScriptableObject : ScriptableObject
    {
        [SerializeField] private GameObject _normalBallPrefab;
        public GameObject NormalBallPrefab => _normalBallPrefab;
    }
}