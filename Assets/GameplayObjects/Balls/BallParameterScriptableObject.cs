using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BallParameters", menuName = "ScriptableObjects/Ball parameters", order = 1)]
public class BallParameterScriptableObject : ScriptableObject
{
    [SerializeField] private List<Color> _colors;
    [SerializeField] private int _startingLevelBallCount;
    [Tooltip("The probability of a new ball being spawned in a group to be the same type(color) as the previous one.\n" +
        "Above 0.5 makes it more likely, below 0.5 is less likely.")]
    [SerializeField] private float _sameTypeProbability;

    public float SameTypeProbability => _sameTypeProbability;
    public int StartingLevelBallCount => _startingLevelBallCount;
    public List<Color> Colors => _colors;

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

}

public struct NormalBallType
{
    public int Type;
    public Color Color;
}
