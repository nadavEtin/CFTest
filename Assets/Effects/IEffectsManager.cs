using Assets.GameplayObjects.Balls;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Effects
{
    public interface IEffectsManager
    {
        void PlayRandomNormalBallFX(HashSet<IBaseBall> balls);
        void PlaySpecialBallEffect(Vector2 position);
    }
}