//using Assets.Infrastructure.Factories;
//using GameCore.Factories;
//using GameCore.ScriptableObjects;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Assets.Scripts.Utility
//{
//    public class BallObjectPool : IObjectPool
//    {
//        private readonly List<GameObject> _objectPool;
//        //private readonly BaseGameObjectFactory _objectFactory;

//        public BallObjectPool(AssetRefsScriptableObject assetRefs)
//        {
//            _objectPool = new List<GameObject>();
//            //_objectFactory = new BallFactory(assetRefs);
//        }

//        public void AddObjectToPool(GameObject obj)
//        {
//            _objectPool.Add(obj);
//            obj.SetActive(false);
//        }

//        public GameObject GetObjectFromPool()
//        {
//            if (_objectPool.Count > 0)
//            {
//                var returnObj = _objectPool[0];
//                _objectPool.RemoveAt(0);
//                returnObj.SetActive(true);
//                return returnObj;
//            }
//            else
//            {
//                var obj = _objectFactory.Create();
//                return null;
//            }
//        }
//    }
//}
