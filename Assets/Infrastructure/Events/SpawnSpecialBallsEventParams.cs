using UnityEngine;

namespace Assets.Infrastructure.Events
{
    public class SpawnSpecialBallsEventParams : BaseEventParams
    {
        public int BallAmount { get; }
        public Vector2[] SpawnPositions { get; }

        public SpawnSpecialBallsEventParams(int amount, Vector2[] spawnPos)
        {
            BallAmount = amount;
            SpawnPositions = spawnPos;
        }
    }
}