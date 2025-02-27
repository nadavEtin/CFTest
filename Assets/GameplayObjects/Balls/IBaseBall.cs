using Assets.Infrastructure.ObjectPoolNS;
using UnityEngine;

namespace Assets.GameplayObjects.Balls
{
    public interface IBaseBall : IPooledObject
    {
        BallTypeParameters BallParameters { get; }
        CircleCollider2D Collider { get; }
        Vector2 Position { get; }
        void Init(BallTypeParameters typeParams);
    }
}
