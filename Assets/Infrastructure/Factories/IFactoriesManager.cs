using UnityEngine;

namespace Assets.Infrastructure.Factories
{
    public interface IFactoriesManager
    {
        GameObject[] GetObject(FactoryType factoryType, int amount, Transform parent = null);
    }
}