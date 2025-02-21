using UnityEngine;
using System.Collections;
using GameCore.Factories;
using System.Collections.Generic;
using GameCore.ScriptableObjects;
using System;

namespace Assets.Infrastructure.Factories
{
    public enum FactoryType
    {
        NormalBall

    }

    public class FactoriesManager : MonoBehaviour
	{
        private AssetRefsScriptableObject _assetRefs;
        private Dictionary<FactoryType, IGameObjectFactory> factories;

        public void Init(AssetRefsScriptableObject assetRefs)
        {
            _assetRefs = assetRefs;
            InitializeFactories();
        }

        //private void Awake()
        //{
        //    //FOR TESTING
        //    _assetRefs = Resources.Load<AssetRefsScriptableObject>("AssetRefs");
        //    InitializeFactories();
        //}

        private void InitializeFactories()
        {
            factories = new Dictionary<FactoryType, IGameObjectFactory>
            {
                { FactoryType.NormalBall, new BallFactory(/*this,*/ _assetRefs.NormalBallPrefab) }                
            };
        }

        public GameObject[] GetObject(FactoryType factoryType, int amount)
        {
            if (factories.TryGetValue(factoryType, out var factory))
            {
                return factory.Create(amount);
            }
            else
            {
                Debug.LogError($"Factory of type {factoryType} not found");
                return null;
            }                
        }
    }
}