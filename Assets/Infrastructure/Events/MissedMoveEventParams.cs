using UnityEngine;


namespace Assets.Infrastructure.Events
{
    public class MissedMoveEventParams : BaseEventParams
    {
        public Vector2 BallWorldPosition { get; }
        public MissedMoveEventParams(Vector2 ballWorldPos)
        {
            BallWorldPosition = ballWorldPos;
        }
    }
}