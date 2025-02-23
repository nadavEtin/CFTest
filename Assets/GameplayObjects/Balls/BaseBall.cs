using System;
using UnityEngine;

namespace Assets.GameplayObjects.Balls
{
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class BaseBall : MonoBehaviour, IBaseBall
    {
        public BallTypeParameters BallParameters => _type;
        public CircleCollider2D Collider => _collider;
        public Vector2 Position => transform.position;

        public Action<GameObject> SendToPoolCB { get; set; }

        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected CircleCollider2D _collider;

        protected BallTypeParameters _type;

        protected abstract void OnMouseDown();

        public abstract void Init(BallTypeParameters typeParams);

        public abstract void ReturnToPool();
    }
}