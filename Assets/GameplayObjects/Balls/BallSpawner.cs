using Assets.Infrastructure.Events;
using Assets.Infrastructure.Factories;
using Assets.Scripts.Utility;
using Events;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameplayObjects.Balls
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _newBallSpawnPoint, _levelStartSpawnPoint;

        private IFactoriesManager _factoriesManager;
        private BallParametersScriptableObject _ballParams;
        private List<NormalBallType> _normalBallTypes;

        private float _sameTypeProbability;
        private NormalBallType? _lastAssignedType = null;

        public void Init(IFactoriesManager factoriesManager, BallParametersScriptableObject ballParams)
        {
            _factoriesManager = factoriesManager;
            _ballParams = ballParams;
            _normalBallTypes = _ballParams.NormalBallTypes;
            EventManager.Instance.Subscribe(TypeOfEvent.SpawnNewBalls, SpawnBalls);
            LevelStartSpawnBalls(_ballParams.StartingLevelBallCount);
        }

        public void LevelStartSpawnBalls(int ballAmount)
        {
            var spawnRadius = 0.5f;
            _sameTypeProbability = _ballParams.SameTypeProbability;
            var newBalls = _factoriesManager.GetObject(FactoryType.NormalBall, ballAmount, transform);
            for (int i = 0; i < newBalls.Length; i++)
            {
                //small offset to prevent balls from stacking
                Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPos = _levelStartSpawnPoint.position + new Vector3(randomOffset.x, randomOffset.y, 0);

                NewBallSetup(newBalls[i], i);
                newBalls[i].transform.position = spawnPos;
            }
        }

        public void SpawnBalls(BaseEventParams eventParams)
        {
            var numberOfBalls = ((SpawnNewBallsEventParams)eventParams).BallAmount;
            _sameTypeProbability = _ballParams.SameTypeProbability;
            var newBalls = _factoriesManager.GetObject(FactoryType.NormalBall, numberOfBalls, transform);
            var ballHeight = newBalls[0].GetComponent<Renderer>().bounds.size.y;
            for (int i = 0; i < newBalls.Length; i++)
            {
                //small offset to prevent balls from stacking
                var randomVariation = Random.Range(-0.25f, 0.25f);

                NewBallSetup(newBalls[i], i);
                newBalls[i].transform.position = _newBallSpawnPoint.position +
                    new Vector3(randomVariation, i * ballHeight, 0);
            }
        }

        private GameObject NewBallSetup(GameObject ball, int index)
        {
            var normalBall = ball.GetComponent<INormalBall>();
            var ballType = GetBiasedType(index);
            normalBall.Init(ballType.Type, ballType.Color);
            return ball;
        }

        private NormalBallType GetBiasedType(int index)
        {
            if (index == 0 || Random.value > _sameTypeProbability)
            {
                return GetRandomType();
            }
            return _lastAssignedType.Value;
        }

        //force the next ball to be different from the last one
        private NormalBallType GetRandomType()
        {
            List<NormalBallType> availableTypes = new List<NormalBallType>(_normalBallTypes);
            if (_lastAssignedType.HasValue)            
                availableTypes.Remove(_lastAssignedType.Value);
            
            _lastAssignedType = availableTypes[Random.Range(0, availableTypes.Count)];
            return _lastAssignedType.Value;
        }

        private void OnDestroy()
        {            
            EventManager.Instance.Unsubscribe(TypeOfEvent.SpawnNewBalls, SpawnBalls);
        }
    }
}