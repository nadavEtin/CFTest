using UnityEngine;

namespace Assets.Scripts.Utility
{
    public interface IObjectPool
    {
        int Count { get; }

        void AddObjectToPool(GameObject obj);

        GameObject GetObjectFromPool();
    }
}
