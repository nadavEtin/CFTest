﻿using UnityEngine;
using System.Collections;
using GameCore.Factories;
using System.Collections.Generic;
using System;

namespace Assets.Infrastructure.Factories
{
    public enum FactoryType
    {
        //gameplay objects
        NormalBall,
        SpecialBall,

        //UI
        MissPopup
    }

    public class FactoriesManager : MonoBehaviour, IFactoriesManager
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
                { FactoryType.NormalBall, new NormalBallFactory(_assetRefs.NormalBallPrefab) },
                { FactoryType.SpecialBall, new SpecialBallFactory(_assetRefs.SpecialBallPrefab) },
                { FactoryType.MissPopup, new MissPopupFactory(_assetRefs.MissPopupPrefab) }
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