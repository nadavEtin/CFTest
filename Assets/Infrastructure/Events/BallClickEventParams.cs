using Assets.GameplayObjects.Balls;

namespace Assets.Infrastructure.Events
{
    public class BallClickEventParams : BaseEventParams
    {
        public IBaseBall Ball { get; }

        public BallClickEventParams(IBaseBall ball)
        {
            Ball = ball;
        }
    }
}