namespace Assets.GameRules
{
    public interface IRulesTracker
    {
        //bool ScoreTargetReached { get; }

        void Init(GameRulesScriptableObject gameRules);
    }
}