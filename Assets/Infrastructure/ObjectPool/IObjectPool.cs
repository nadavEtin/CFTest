using UnityEngine;

namespace Assets.Infrastructure.ObjectPoolNS
{
    public interface IObjectPool
    {
        int Count { get; }

        void AddObjectToPool(GameObject obj);

        GameObject GetObjectFromPool();
    }
}
