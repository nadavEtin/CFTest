using Assets.Infrastructure.ObjectPoolNS;
using UnityEngine;

namespace Assets.Infrastructure.Factories
{

    public abstract class BaseGameObjectFactory : IGameObjectFactory
    {
        protected readonly GameObject prefab;   //main prefab to create
        protected GameObject[] additionalPrefabs;   //additional options
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