using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Factories
{
    public interface IGameObjectFactory
    {
        GameObject[] Create(int amount);
    }
}