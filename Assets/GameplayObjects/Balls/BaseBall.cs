using Assets.Infrastructure.ObjectPool;
using System;
using UnityEngine;

namespace Assets.GameplayObjects.Balls
{
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class BaseBall : MonoBehaviour, INormalBall, IPooledObject
    {
        public int Type => _type;
        public CircleCollider2D Collider => _collider;
        public Vector2 Position => transform.position;

        public Action<GameObject> SendToPoolCB { get; set; }

        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected CircleCollider2D _collider;

        protected int _type;

        protected abstract void OnMouseDown();

        public abstract void Init(int type, Color color);

        public abstract void ReturnToPool();
    }
}