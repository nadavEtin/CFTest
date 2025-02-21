using Assets.Scripts.Utility;

namespace Events
{
    public class BallClickEventParams : BaseEventParams
    {
        public INormalBall Ball { get; }

        public BallClickEventParams(INormalBall ball)
        {
            Ball = ball;
        }
    }
}