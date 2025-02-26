using UnityEngine;

[CreateAssetMenu(fileName = "EffectRefs", menuName = "ScriptableObjects/Effect Refrences")]
public class EffectRefsScriptableObject : ScriptableObject
{
    [SerializeField] private GameObject _normalBallPop1;
    [SerializeField] private GameObject _normalBallPop2;
    [SerializeField] private GameObject _normalBallPop3;
    [SerializeField] private GameObject _specialBallPop;

    private GameObject[] _normalBallPopEffects;

    public GameObject NormalBallPop1 => _normalBallPop1;
    public GameObject NormalBallPop2 => _normalBallPop2;
    public GameObject NormalBallPop3 => _normalBallPop3;    
    public GameObject SpecialBallPop => _specialBallPop;
    public GameObject[] NormalBallPopEffects => _normalBallPopEffects;

    private void OnEnable()
    {
        if (_normalBallPopEffects == null || _normalBallPopEffects.Length == 0)
        {
            _normalBallPopEffects = new GameObject[]
            {
                _normalBallPop1,
                _normalBallPop2,
                _normalBallPop3
            };
        }
    }

    //public GameObject[] NormalBallPopEffects
    //{
    //    get
    //    {
    //        _normalBallPopEffects ??= new GameObject[]
    //        {
    //            _normalBallPop1,
    //            _normalBallPop2,
    //            _normalBallPop3
    //        };                

    //        return _normalBallPopEffects;
    //    }
    //}
}