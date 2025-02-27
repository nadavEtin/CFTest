using Assets.Infrastructure.Events;
using Assets.Infrastructure.Factories;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameplayObjects.Balls
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _newBallSpawnPoint, _levelStartSpawnPoint;

        private IFactoriesManager _factoriesManager;
        private BallParametersScriptableObject _ballParams;
        private List<BallTypeParameters> _normalBallTypes;

        private float _sameTypeProbability;     //likelihood of spawning same color balls consecutively
        private BallTypeParameters? _lastAssignedType = null;

        public void Init(IFactoriesManager factoriesManager, BallParametersScriptableObject ballParams)
        {
            _factoriesManager = factoriesManager;
            _ballParams = ballParams;
            _normalBallTypes = _ballParams.NormalBallTypes;
            EventsSubscribe();
        }

        private void EventsSubscribe()
        {            
            EventManager.Instance.Subscribe(TypeOfEvent.SpawnNormalBalls, SpawnReplacementBalls);
            EventManager.Instance.Subscribe(TypeOfEvent.SpawnSpecialBall, SpawnSpecialBalls);
            EventManager.Instance.Subscribe(TypeOfEvent.GameStart, LevelStartSpawnBalls);
        }

        //spawns the initial balls at the start of the level
        public void LevelStartSpawnBalls(BaseEventParams eventParams)
        {
            var spawnRadius = 0.2f;
            _sameTypeProbability = _ballParams.SameTypeProbability;
            var newBalls = _factoriesManager.GetObject(FactoryType.NormalBall, _ballParams.StartingLevelBallCount, transform);
            for (int i = 0; i < newBalls.Length; i++)
            {
                //small offset to prevent balls from stacking
                Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPos = _levelStartSpawnPoint.position + new Vector3(randomOffset.x, randomOffset.y, 0);

                NewNormalBallSetup(newBalls[i], i);
                newBalls[i].transform.position = spawnPos;
            }
        }

        //replaces the balls popped in a match
        public void SpawnReplacementBalls(BaseEventParams eventParams)
        {
            var numberOfBalls = ((SpawnNormalBallsEventParams)eventParams).BallAmount;
            _sameTypeProbability = _ballParams.SameTypeProbability;
            var newBalls = _factoriesManager.GetObject(FactoryType.NormalBall, numberOfBalls, transform);
            var ballHeight = newBalls[0].GetComponent<Renderer>().bounds.size.y;
            for (int i = 0; i < newBalls.Length; i++)
            {
                //small offset to prevent balls from stacking
                var randomVariation = Random.Range(-0.25f, 0.25f);

                NewNormalBallSetup(newBalls[i], i);
                newBalls[i].transform.position = _newBallSpawnPoint.position +
                    new Vector3(randomVariation, i * ballHeight, 0);
            }
        }

        private GameObject NewNormalBallSetup(GameObject ball, int index)
        {
            var normalBall = ball.GetComponent<IBaseBall>();
            var ballType = GetBiasedType(index);
            normalBall.Init(ballType);
            return ball;
        }

        //spawns the special exploding ball at the position of the normal ball clicked
        private void SpawnSpecialBalls(BaseEventParams eventParams)
        {
            var specialBallParams = (SpawnSpecialBallsEventParams)eventParams;
            var newBalls = _factoriesManager.GetObject(FactoryType.SpecialBall, specialBallParams.BallAmount, transform);

            for (int i = 0; i < newBalls.Length && i < specialBallParams.SpawnPositions.Length; i++)
            {
                var specialBall = newBalls[i].GetComponent<IBaseBall>();
                specialBall.Init(new BallTypeParameters(BallTypes.Special, Color.white));
                newBalls[i].transform.position = specialBallParams.SpawnPositions[i];
            }
        }

        //apply bias to normal ball color selection
        private BallTypeParameters GetBiasedType(int index)
        {
            if (index == 0 || Random.value > _sameTypeProbability)
            {
                return GetRandomType();
            }
            return _lastAssignedType.Value;
        }

        //force the next ball to be a different color from the last one
        private BallTypeParameters GetRandomType()
        {
            List<BallTypeParameters> availableTypes = new List<BallTypeParameters>(_normalBallTypes);
            if (_lastAssignedType.HasValue)            
                availableTypes.Remove(_lastAssignedType.Value);
            
            _lastAssignedType = availableTypes[Random.Range(0, availableTypes.Count)];
            return _lastAssignedType.Value;
        }

        private void OnDestroy()
        {            
            EventManager.Instance.Unsubscribe(TypeOfEvent.SpawnNormalBalls, SpawnReplacementBalls);
            EventManager.Instance.Unsubscribe(TypeOfEvent.SpawnSpecialBall, SpawnSpecialBalls);
            EventManager.Instance.Unsubscribe(TypeOfEvent.GameStart, LevelStartSpawnBalls);
        }
    }
}