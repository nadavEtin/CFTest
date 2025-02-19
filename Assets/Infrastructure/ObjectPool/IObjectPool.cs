using UnityEngine;

namespace Assets.Scripts.Utility
{
    public interface IObjectPool
    {
        void AddObjectToPool(GameObject obj);

        GameObject GetObjectFromPool();
    }
}
