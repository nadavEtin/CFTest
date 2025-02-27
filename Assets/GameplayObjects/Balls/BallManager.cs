using Assets.Effects;
using Assets.Infrastructure.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameplayObjects.Balls
{
    public class BallManager
    {
        private BallParametersScriptableObject _ballParameters;
        private EffectsManager _effectsManager;
        private int _minConnectedBallsForMatch, _minMatchSizeForSpecialBall;
        private float _normalBallNeighborDetectionRadius, _specialBallExplosionRadius;

        public BallManager(BallParametersScriptableObject ballParameters, EffectsManager effectsManager)
        {
            _ballParameters = ballParameters;
            _effectsManager = effectsManager;
            _minConnectedBallsForMatch = _ballParameters.MinConnectedCountForMatch;
            _minMatchSizeForSpecialBall = _ballParameters.MinMatchSizeToSpawnSpecial;
            _normalBallNeighborDetectionRadius = _ballParameters.NormalBallNeighborDetectionRadius;
            _specialBallExplosionRadius = _ballParameters.SpecialBallExplosionRadius;

            EventManager.Instance.Subscribe(TypeOfEvent.BallClick, OnBallClicked);
        }

        private HashSet<IBaseBall> GetConnectedBalls(IBaseBall startBall, float detectionRadius)
        {
            HashSet<IBaseBall> connectedBalls = new HashSet<IBaseBall> { startBall };
            HashSet<IBaseBall> newlyFound = new HashSet<IBaseBall> { startBall };

            while (newlyFound.Count > 0)
            {
                HashSet<IBaseBall> nextBatch = new HashSet<IBaseBall>();

                foreach (IBaseBall ball in newlyFound)
                {
                    var neighbors = GetNeighbors(ball, detectionRadius);
                    foreach (IBaseBall neighbor in neighbors)
                    {
                        //check same ball type (color) and that it wasn't already added
                        if (neighbor.BallParameters.Type == startBall.BallParameters.Type && !connectedBalls.Contains(neighbor))
                        {
                            nextBatch.Add(neighbor);
                            connectedBalls.Add(neighbor);
                        }
                    }
                }

                newlyFound = nextBatch;
            }

            return connectedBalls;
        }

        //find all balls in a given radius around a ball
        private HashSet<IBaseBall> GetNeighbors(IBaseBall ball, float detectionRadius)
        {
            var neighbors = new HashSet<IBaseBall>();
            float checkRadius = ball.Collider.radius * detectionRadius;

            Collider2D[] results = new Collider2D[50];
            var numColliders = Physics2D.OverlapCircleNonAlloc(
                ball.Position,
                checkRadius,
                results,
                LayerMask.GetMask("Ball")   //ignore walls and other non-ball objects
            );

            for (int i = 0; i < numColliders; i++)
            {
                var neighbor = results[i].GetComponent<IBaseBall>();
                if (neighbor != null && neighbor != this)
                    neighbors.Add(neighbor);
            }

            return neighbors;
        }

        private void OnBallClicked(BaseEventParams eventParams)
        {
            var clickedBall = ((BallClickEventParams)eventParams).Ball;

            if (clickedBall.BallParameters.Type == BallTypes.Special)
                SpecialBallClicked(clickedBall);
            else
                NormalBallClicked(clickedBall);
        }

        private void NormalBallClicked(IBaseBall clickedBall)
        {
            HashSet<IBaseBall> connected = GetConnectedBalls(clickedBall, _normalBallNeighborDetectionRadius);
            if (connected.Count >= _minConnectedBallsForMatch)
            {
                //spwn special ball on clicked ball position, if requirements are met
                if (connected.Count >= _minMatchSizeForSpecialBall)
                    EventManager.Instance.Publish(TypeOfEvent.SpawnSpecialBall, new SpawnSpecialBallsEventParams(1, new Vector2[] { clickedBall.Position }));

                //play pop fx over matched balls
                _effectsManager.PlayRandomNormalBallFX(connected);
                HandleRemovedBalls(connected);
            }
            else
                EventManager.Instance.Publish(TypeOfEvent.MissedMove, new MissedMoveEventParams(clickedBall.Position));
        }

        private void SpecialBallClicked(IBaseBall clickedBall)
        {
            HashSet<IBaseBall> ballsInRadius = GetNeighbors(clickedBall, _specialBallExplosionRadius);
            _effectsManager.PlaySpecialBallEffect(clickedBall.Position);

            //remove balls after launching fx
            HandleRemovedBalls(ballsInRadius);
        }

        //return balls to object pool, replace them and update score
        private void HandleRemovedBalls(HashSet<IBaseBall> removedBalls)
        {
            foreach (IBaseBall ball in removedBalls)
            {
                RemoveBall(ball);
            }

            var score = _ballParameters.CalculateScore(removedBalls.Count);
            EventManager.Instance.Publish(TypeOfEvent.ScoreUpdate, new ScoreUpdateEventParams(score));

            //replace removed balls
            EventManager.Instance.Publish(TypeOfEvent.SpawnNormalBalls, new SpawnNormalBallsEventParams(removedBalls.Count));
        }

        private void RemoveBall(IBaseBall ball)
        {
            var ballObject = ball.Collider.gameObject;
            ballObject.SetActive(false);
            ballObject.transform.position = new Vector2(100, 0);    //move ball away from the scene to avoid unexpected collision errors
            ball.ReturnToPool();
        }

        public void Unsubscribe()
        {
            EventManager.Instance.Unsubscribe(TypeOfEvent.BallClick, OnBallClicked);
        }
    }
}