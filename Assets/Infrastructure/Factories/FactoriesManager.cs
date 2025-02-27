using Assets.Effects;
using Assets.GameplayObjects.Balls;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Infrastructure.Factories
{
    public enum FactoryType
    {
        //gameplay objects
        NormalBall,
        SpecialBall,

        //UI
        MissPopup,

        //effects
        BallPopFX,
        SpecialBallPopFX,
    }

    public class FactoriesManager : MonoBehaviour, IFactoriesManager
    {
        private AssetRefsScriptableObject _assetRefs;
        private EffectRefsScriptableObject _effectRefs;
        private Dictionary<FactoryType, IGameObjectFactory> factories;

        public void Init(AssetRefsScriptableObject assetRefs, EffectRefsScriptableObject effectRefs)
        {
            _assetRefs = assetRefs;
            _effectRefs = effectRefs;
            InitializeFactories();
        }

        private void InitializeFactories()
        {
            factories = new Dictionary<FactoryType, IGameObjectFactory>
            {
                { FactoryType.NormalBall, new NormalBallFactory(_assetRefs.NormalBallPrefab) },
                { FactoryType.SpecialBall, new SpecialBallFactory(_assetRefs.SpecialBallPrefab) },
                { FactoryType.MissPopup, new MissPopupFactory(_assetRefs.MissPopupPrefab) },
                { FactoryType.BallPopFX, new ParticleEffectsFactory(_effectRefs.NormalBallPop1, _effectRefs.NormalBallPopEffects) },
                { FactoryType.SpecialBallPopFX, new SpecialBallFXFactory(_effectRefs.SpecialBallPop) }
            };
        }

        public GameObject[] GetObject(FactoryType factoryType, int amount, Transform parent = null)
        {
            if (factories.TryGetValue(factoryType, out var factory))
            {
                return factory.Create(amount, parent);
            }
            else
            {
                Debug.LogError($"Factory of type {factoryType} not found");
                return null;
            }
        }
    }
}