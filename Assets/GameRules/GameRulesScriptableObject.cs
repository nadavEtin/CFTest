using UnityEngine;
using System.Collections;

namespace Assets.GameRules
{
    [CreateAssetMenu(fileName = "GameRules", menuName = "ScriptableObjects/Game rules")]
    public class GameRulesScriptableObject : ScriptableObject
	{

        [SerializeField] private float _timeLimit;
        [SerializeField] private int _targetScore;
        [SerializeField] private int _maxTaps;

        public float TimeLimit => _timeLimit;
        public int TargetScore => _targetScore;
        public int MaxTaps => _maxTaps;
    }
}