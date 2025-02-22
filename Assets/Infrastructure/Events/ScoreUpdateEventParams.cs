using Assets.Scripts.Utility;

namespace Assets.Infrastructure.Events
{
    public class ScoreUpdateEventParams : BaseEventParams
    {
        public int Score { get; }
        public ScoreUpdateEventParams(int score)
        {
            Score = score;
        }
    }
}