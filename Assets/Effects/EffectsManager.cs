using Assets.GameplayObjects.Balls;
using Assets.Infrastructure.Factories;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Effects
{
    public class EffectsManager : IEffectsManager
    {
        private IFactoriesManager _factoriesManager;
        private GameObject _fxContainer;

        public EffectsManager(IFactoriesManager factoriesManager)
        {
            _fxContainer = new GameObject("FxContainer");
            _factoriesManager = factoriesManager;
        }

        //gets random effects from the available options
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

        public void PlaySpecialBallEffect(Vector2 position)
        {
            var fx = _factoriesManager.GetObject(FactoryType.SpecialBallPopFX, 1, _fxContainer.transform);
            fx[0].transform.position = position;
        }
    }
}
