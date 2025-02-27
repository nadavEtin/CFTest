using Assets.Infrastructure.Events;

namespace Assets.GameplayObjects.Balls
{
    //regular colored ball
    public class NormalBall : BaseBall
    {
        public override void Init(BallTypeParameters typeParams)
        {
            _type = typeParams;
            _spriteRenderer.color = typeParams.Color;
        }

        public override void ReturnToPool()
        {
            SendToPoolCB?.Invoke(gameObject);
        }

        protected override void OnMouseDown()
        {
            EventManager.Instance.Publish(TypeOfEvent.BallClick, new BallClickEventParams(this));
        }
    }
}
