using Assets.GameplayObjects.Balls;
using Assets.Infrastructure.ObjectPool;
using UnityEngine;

public interface IBaseBall : IPooledObject
{
    BallTypeParameters BallParameters { get; }
    CircleCollider2D Collider { get; }
    Vector2 Position { get; }
    void Init(BallTypeParameters typeParams);
}