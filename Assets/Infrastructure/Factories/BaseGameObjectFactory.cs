using Assets.Infrastructure.ObjectPool;
using UnityEngine;

namespace GameCore.Factories
{

    public abstract class BaseGameObjectFactory : IGameObjectFactory
    {
        protected readonly GameObject prefab;
        protected BallObjectPool objectPool;

        protected BaseGameObjectFactory(GameObject prefab)
        {
            this.prefab = prefab;
            objectPool = new BallObjectPool();
        }

        public abstract GameObject[] Create(int amount, Transform parent = null);
    }
}