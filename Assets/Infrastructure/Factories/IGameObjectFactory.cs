using UnityEngine;

namespace Assets.Infrastructure.Factories
{
    public interface IGameObjectFactory
    {
        GameObject[] Create(int amount, Transform parent = null);
    }
}