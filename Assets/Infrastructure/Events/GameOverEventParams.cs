namespace Assets.Infrastructure.Events
{
    public class GameOverEventParams : BaseEventParams
    {
        public int FinalScore { get; }

        public GameOverEventParams(int finalScore)
        {
            FinalScore = finalScore;
        }
    }
}
