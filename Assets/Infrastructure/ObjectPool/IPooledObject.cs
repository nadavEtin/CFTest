using System;
using UnityEngine;

namespace Assets.Infrastructure.ObjectPoolNS
{
    //implemented by all objects that need to be pooled
    public interface IPooledObject
    {
        Action<GameObject> SendToPoolCB { set; get; }

        void ReturnToPool();
    }
}