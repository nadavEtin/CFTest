using UnityEngine;
using System.Collections;

namespace Assets.GameRules
{
    [CreateAssetMenu(fileName = "GameRules", menuName = "ScriptableObjects/Game rules")]
    public class GameRulesScriptableObject : ScriptableObject
	{

        [SerializeField] private int _timeLimit;
        [SerializeField] private int _targetScore;
        [SerializeField] private int _maxMoves;

        public int TimeLimit => _timeLimit;
        public int TargetScore => _targetScore;
        public int MaxMoves => _maxMoves;
    }
}