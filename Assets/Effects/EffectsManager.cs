using Assets.Infrastructure.Factories;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Effects
{
    public class EffectsManager
    {
        //[SerializeField] private EffectRefsScriptableObject _effectsRefs;

        private IFactoriesManager _factoriesManager;
        private GameObject _fxContainer;

        public EffectsManager(/*EffectRefsScriptableObject effectRefs,*/ IFactoriesManager factoriesManager)
        {
            //_effectsRefs = effectRefs;
            _fxContainer = new GameObject("FxContainer");
            _factoriesManager = factoriesManager;
        }

        public void PlayRandomNormalBallFX(HashSet<IBaseBall> balls)
        {
            if (balls == null || balls.Count == 0) return;

            var ballsList = balls.ToList();
            var fxObjs = _factoriesManager.GetObject(FactoryType.BallPopFX, balls.Count, _fxContainer.transform);
            for (int i = 0; i < ballsList.Count; i++)
            {
                var pos = ballsList[i].Position;
                var fx = fxObjs[i];
                fx.transform.position = pos;
            }
        }

        // Optional: Method to play the special pop effect specifically
        public void PlaySpecialEffect(Vector2 position)
        {
            //GameObject instance = Instantiate(_effectsRefs.SpecialBallPop, position, Quaternion.identity);
            //Destroy(instance, 2f);
        }
    }
}
