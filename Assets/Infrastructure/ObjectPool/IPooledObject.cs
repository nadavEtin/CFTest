using System;
using UnityEngine;

namespace Assets.Infrastructure.ObjectPool
{
    public interface IPooledObject
    {
        Action<GameObject> SendToPoolCB { set; get; }

        void ReturnToPool();
    }
}