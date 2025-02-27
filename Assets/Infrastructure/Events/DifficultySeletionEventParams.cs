using Assets.GameRules;

namespace Assets.Infrastructure.Events
{
    public class DifficultySeletionEventParams : BaseEventParams
    {
        public GameDifficulty CurrentDifficulty { get; }

        public DifficultySeletionEventParams(GameDifficulty seletion)
        {
            CurrentDifficulty = seletion;
        }
    }
}
