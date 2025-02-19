using UnityEngine;

namespace GameCore.Factories
{
    public interface IGameObjectFactory
    {
        GameObject Create();
    }
}