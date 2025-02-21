using Assets.GameplayObjects.Balls;
using Assets.Infrastructure.Events;
using Events;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class NormalBall : BaseBall
{
    protected override void OnMouseDown()
    {
        EventManager.Instance.Publish(TypeOfEvent.BallClick, new BallClickEventParams(this));
    }

    public override void Init(int type, Color color)
    {
        _type = type;
        _spriteRenderer.color = color;
    }

    public override void ReturnToPool()
    {
        SendToPoolCB?.Invoke(gameObject);
    }
}
