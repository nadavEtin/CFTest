using Assets.Infrastructure.ObjectPoolNS;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Infrastructure.ObjectPoolNS
{
    public class ObjectPool : IObjectPool
    {
        public int Count => _objectPool.Count;

        private readonly List<GameObject> _objectPool;

        public ObjectPool()
        {
            _objectPool = new List<GameObject>();
        }

        public void AddObjectToPool(GameObject obj)
        {
            _objectPool.Add(obj);
            obj.SetActive(false);
        }

        public GameObject GetObjectFromPool()
        {
            if (_objectPool.Count > 0)
            {
                var returnObj = _objectPool[0];
                _objectPool.RemoveAt(0);
                returnObj.SetActive(true);
                return returnObj;
            }
            else
                return null;
        }
    }
}
