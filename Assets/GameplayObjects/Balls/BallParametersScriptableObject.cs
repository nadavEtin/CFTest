using System.Collections.Generic;
using UnityEngine;

public enum DifficultyOptions
{
    Easy,
    Normal,
    Hard
}

[CreateAssetMenu(fileName = "BallParameters", menuName = "ScriptableObjects/Ball parameters", order = 1)]
public class BallParametersScriptableObject : ScriptableObject
{
    [HideInInspector] public DifficultyOptions ChosenDifficulty;
    public float SameTypeProbability => _sameTypeProbability;
    public int StartingLevelBallCount => _startingLevelBallCount;
    public int MinConnectedCountForMatch => _minConnectedCountForMatch;
    public List<Color> Colors => _colors;

    [SerializeField] private List<Color> _colors;
    [SerializeField] private int _startingLevelBallCount, _minConnectedCountForMatch;

    [Tooltip("The probability of a new ball being spawned in a group to be the same type(color) as the previous one.\n" +
        "Above 0.5 makes it more likely, below 0.5 is less likely.")]
    [SerializeField] private float _sameTypeProbability;
    
    private List<NormalBallType> _cachedNormalBallTypes;

    public List<NormalBallType> NormalBallTypes
    {
        get
        {
            if (_cachedNormalBallTypes == null)
            {
                _cachedNormalBallTypes = new List<NormalBallType>();
                for (int i = 0; i < _colors.Count; i++)
                {
                    _cachedNormalBallTypes.Add(new NormalBallType { Type = i, Color = _colors[i] });
                }
            }
            return _cachedNormalBallTypes;
        }
    }

    public int CalculateScore(int matchBallCount)
    {
        int score = 0;
        if(matchBallCount < _minConnectedCountForMatch)
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

public struct NormalBallType
{
    public int Type;
    public Color Color;
}
