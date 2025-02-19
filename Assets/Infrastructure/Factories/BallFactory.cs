using GameCore.Factories;
using GameCore.ScriptableObjects;
using UnityEngine;

namespace Assets.Infrastructure.Factories
{
    public class BallFactory : BaseGameObjectFactory
    {
        //private GameObject normalBallPrefab;

        public BallFactory(MonoBehaviour context, GameObject assetPrefab) : base(context, assetPrefab)
        {
        }

        public override GameObject Create()
        {
            return Object.Instantiate(prefab);
        }
    }
}