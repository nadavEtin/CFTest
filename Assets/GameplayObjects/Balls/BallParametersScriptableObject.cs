using Assets.GameRules;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameplayObjects.Balls
{
    public enum BallTypes
    {
        Normal0,
        Normal1,
        Normal2,
        Normal3,
        Special
    }

    [CreateAssetMenu(fileName = "BallParameters", menuName = "ScriptableObjects/Ball parameters")]
    public class BallParametersScriptableObject : ScriptableObject
    {
        [HideInInspector] public GameDifficulty ChosenDifficulty;

        #region Public getters

        public float SameTypeProbability => _sameTypeProbability;
        public float NormalBallNeighborDetectionRadius => _normalBallNeighborDetectionRadius;
        public float SpecialBallExplosionRadius => _specialBallExplosionRadius;
        public int StartingLevelBallCount => _startingLevelBallCount;
        public int MinConnectedCountForMatch => _minConnectedCountForMatch;
        public int MinMatchSizeToSpawnSpecial => _minMatchSizeToSpawnSpecial;
        public List<Color> Colors => _colors;

        #endregion


        [SerializeField] private List<Color> _colors;
        [SerializeField] private int _startingLevelBallCount, _minConnectedCountForMatch, _minMatchSizeToSpawnSpecial;

        [Tooltip("1.0 = exact ball radius. 2.0 = twice the radius of the ball etc.")]
        [SerializeField] private float _normalBallNeighborDetectionRadius;
        [SerializeField] private float _specialBallExplosionRadius;

        [Tooltip("The probability of a new ball being spawned in a group to be the same type(color) as the previous one.\n" +
            "Above 0.5 makes it more likely, below 0.5 is less likely.")]
        [SerializeField] private float _sameTypeProbability;

        private List<BallTypeParameters> _cachedNormalBallTypes;

        public List<BallTypeParameters> NormalBallTypes
        {
            get
            {
                if (_cachedNormalBallTypes == null)
                {
                    _cachedNormalBallTypes = new List<BallTypeParameters>();
                    for (int i = 0; i < _colors.Count; i++)
                    {
                        _cachedNormalBallTypes.Add(new BallTypeParameters { Type = (BallTypes)i, Color = _colors[i] });
                    }
                }

                return _cachedNormalBallTypes;
            }
        }

        public int CalculateScore(int matchBallCount)
        {
            int score = 0;
            if (matchBallCount < _minConnectedCountForMatch)
            {
                Debug.LogError("Matched ball count is less than the minimum");
                return score;
            }

            switch (matchBallCount)
            {
                case <= 10:
                    score = matchBallCount;
                    break;
                case <= 20:
                    score = matchBallCount * 2;
                    break;
                case > 20:
                    score = matchBallCount * 3;
                    break;
            }

            return score;
        }
    }

    public struct BallTypeParameters
    {
        public BallTypes Type;
        public Color Color;

        public BallTypeParameters(BallTypes type, Color color)
        {
            Type = type;
            Color = color;
        }
    }
}
