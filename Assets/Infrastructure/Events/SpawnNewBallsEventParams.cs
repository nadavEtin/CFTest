using Assets.Scripts.Utility;

namespace Events
{
    public class SpawnNewBallsEventParams : BaseEventParams
    {
        public int BallAmount { get; }

        public SpawnNewBallsEventParams(int amount)
        {
            BallAmount = amount;
        }
    }
}