using UnityEngine;

namespace GameCore.Factories
{

    public abstract class BaseGameObjectFactory : IGameObjectFactory
    {
        protected readonly MonoBehaviour context;
        protected readonly GameObject prefab;

        protected BaseGameObjectFactory(MonoBehaviour context, GameObject prefab)
        {
            this.context = context;
            this.prefab = prefab;
        }

        public abstract GameObject Create();
    }
}