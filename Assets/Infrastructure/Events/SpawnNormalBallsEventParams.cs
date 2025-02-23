using Assets.Scripts.Utility;

namespace Events
{
    public class SpawnNormalBallsEventParams : BaseEventParams
    {
        public int BallAmount { get; }

        public SpawnNormalBallsEventParams(int amount)
        {
            BallAmount = amount;
        }
    }
}