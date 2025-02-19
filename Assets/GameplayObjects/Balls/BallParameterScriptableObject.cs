using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BallParameters", menuName = "ScriptableObjects/Ball parameters", order = 1)]
public class BallParameterScriptableObject : ScriptableObject
{
    [SerializeField] private List<Color> _colors;
    public List<Color> Colors => _colors;

    public Color Color1 => _colors.Count > 0 ? _colors[0] : default;
    public Color Color2 => _colors.Count > 1 ? _colors[1] : default;
    public Color Color3 => _colors.Count > 2 ? _colors[2] : default;
    public Color Color4 => _colors.Count > 3 ? _colors[3] : default;
    public Color Color5 => _colors.Count > 4 ? _colors[4] : default;
}
