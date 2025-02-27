using Assets.Infrastructure.ObjectPoolNS;
using System;
using UnityEngine;

namespace Assets.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public class BallPopFX : MonoBehaviour, IPooledObject
    {
        public Action<GameObject> SendToPoolCB { set; get; }

        public void ReturnToPool()
        {
            SendToPoolCB?.Invoke(gameObject);
        }

        private void OnParticleSystemStopped()
        {
            ReturnToPool();
        }
    }
}