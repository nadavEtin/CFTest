using Assets.Infrastructure.ObjectPool;
using System;
using UnityEngine;

namespace Assets.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public class NormalBallPopFX : MonoBehaviour, IPooledObject
    {
        public Action<GameObject> SendToPoolCB { set; get; }

        private void Awake()
        {
            var main = GetComponent<ParticleSystem>().main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

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