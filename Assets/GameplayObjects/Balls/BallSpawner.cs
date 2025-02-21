using Assets.Infrastructure.Factories;
using UnityEngine;

namespace Assets.GameplayObjects.Balls
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _newBallSpawnPoint, _levelStartSpawnPoint;

        private FactoriesManager _factoriesManager;
        private BallParameterScriptableObject _ballParams;

        private float _sameTypeProbability;
        private int _lastAssignedType;

        public void Init(FactoriesManager factoriesManager, BallParameterScriptableObject ballParams)
        {
            _factoriesManager = factoriesManager;
            _ballParams = ballParams;
            LevelStartSpawnBalls(_ballParams.StartingLevelBallCount);
        }

        public void LevelStartSpawnBalls(int ballAmount)
        {
            var spawnRadius = 1f;
            _sameTypeProbability = _ballParams.SameTypeProbability;
            var newBalls = _factoriesManager.GetObject(FactoryType.NormalBall, ballAmount);
            for (int i = 0; i < newBalls.Length; i++)
            {
                //small offset to prevent balls from stacking
                Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPos = _levelStartSpawnPoint.position + new Vector3(randomOffset.x, randomOffset.y, 0);

                NewBallSetup(newBalls[i], i);
                newBalls[i].transform.position = spawnPos;
            }
        }

        public void SpawnBalls(int numberOfBalls)
        {
            _sameTypeProbability = _ballParams.SameTypeProbability;
            var newBalls = _factoriesManager.GetObject(FactoryType.NormalBall, numberOfBalls);
            var ballHeight = newBalls[0].GetComponent<Renderer>().bounds.size.y;
            for (int i = 0; i < newBalls.Length; i++)
            {
                var randomVariation = Random.Range(-0.25f, 0.25f);
                newBalls[i].transform.position = _newBallSpawnPoint.position +
                    new Vector3(randomVariation, i * ballHeight, 0);
            }
        }

        private GameObject NewBallSetup(GameObject ball, int index)
        {
            var normalBall = ball.GetComponent<INormalBall>();
            var ballType = GetBiasedType(index);
            var ballColor = _ballParams.Colors[ballType];
            normalBall.Init(ballType, ballColor);
            return ball;
        }

        private int GetBiasedType(int index)
        {
            if (index == 0 || Random.value < _sameTypeProbability)
            {
                return GetRandomType();
            }
            return _lastAssignedType;
        }

        private int GetRandomType()
        {
            _lastAssignedType = Random.Range(0, _ballParams.Colors.Count);
            return _lastAssignedType;
        }
    }
}