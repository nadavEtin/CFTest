using Assets.Infrastructure.Events;

namespace Assets.GameplayObjects.Balls
{
    //exploding ball element
    public class SpecialBall : BaseBall
    {
        public override void Init(BallTypeParameters typeParams)
        {
            _type = typeParams;
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