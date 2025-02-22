using UnityEngine;

public interface INormalBall
{
    int Type { get; }
    CircleCollider2D Collider { get; }
    Vector2 Position { get; }
    void Init(int type, Color color);
}