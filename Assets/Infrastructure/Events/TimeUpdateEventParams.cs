using Assets.Scripts.Utility;

namespace Assets.Infrastructure.Events
{
    public class TimeUpdateEventParams : BaseEventParams
    {
        public int TimeLeft { get; }

        public TimeUpdateEventParams(int timeLeft)
        {
            TimeLeft = timeLeft;
        }
    }
}