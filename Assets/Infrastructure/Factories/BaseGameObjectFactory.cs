using Assets.Infrastructure.ObjectPool;
using UnityEngine;

namespace GameCore.Factories
{

    public abstract class BaseGameObjectFactory : IGameObjectFactory
    {
        protected readonly GameObject prefab;
        protected GameObject[] additionalPrefabs;
        protected ObjectPool objectPool;

        protected BaseGameObjectFactory(GameObject prefab)
        {
            this.prefab = prefab;
            objectPool = new ObjectPool();
        }

        protected BaseGameObjectFactory(GameObject prefab, GameObject[] additionalPrefabs) : this(prefab)
        {
            this.additionalPrefabs = additionalPrefabs;
        }

        public abstract GameObject[] Create(int amount, Transform parent = null);
    }
}