using Assets.Infrastructure.Events;
using Assets.Scripts.Utility;
using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameplayObjects.Balls
{
    public class BallManager
    {
        private BallParametersScriptableObject _ballParameters;
        private int _minConnectedBallsForMatch, _minMatchSizeForSpecialBall;
        private float _normalBallNeighborDetectionRadius, _specialBallExplosionRadius;

        public BallManager(BallParametersScriptableObject ballParameters)
        {
            EventManager.Instance.Subscribe(TypeOfEvent.BallClick, OnBallClicked);
            _ballParameters = ballParameters;
            _minConnectedBallsForMatch = _ballParameters.MinConnectedCountForMatch;
            _minMatchSizeForSpecialBall = _ballParameters.MinMatchSizeToSpawnSpecial;
            _normalBallNeighborDetectionRadius = _ballParameters.NormalBallNeighborDetectionRadius;
            _specialBallExplosionRadius = _ballParameters.SpecialBallExplosionRadius;
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

            if(clickedBall.BallParameters.Type == BallTypes.Special)            
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

                HandleRemovedBalls(connected);
                foreach (IBaseBall ball in connected)
                {
                    //TO DO: ADD match FX HERE
                    
                }

                //var score = _ballParameters.CalculateScore(connected.Count);
                //EventManager.Instance.Publish(TypeOfEvent.ScoreUpdate, new ScoreUpdateEventParams(score));

                ////replace removed balls
                //EventManager.Instance.Publish(TypeOfEvent.SpawnNormalBalls, new SpawnNormalBallsEventParams(connected.Count));
            }
            else
            {
                EventManager.Instance.Publish(TypeOfEvent.MissedMove, new MissedMoveEventParams(clickedBall.Position));
            }
        }

        private void SpecialBallClicked(IBaseBall clickedBall)
        {
            HashSet<IBaseBall> ballsInRadius = GetNeighbors(clickedBall, _specialBallExplosionRadius);
            HandleRemovedBalls(ballsInRadius);
            foreach (IBaseBall ball in ballsInRadius)
            {
                //TO DO: ADD explosion FX HERE
                
            }
        }

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
            ballObject.transform.position = new Vector2(100, 0);    //move ball away from the scene to avoid unexpected errors
            ball.ReturnToPool();
        }

        private IEnumerator ExplodeBall(IBaseBall ball)
        {
            // Play explosion effect
            // Maybe add score
            // Return to pool or destroy
            //Destroy(ball.gameObject);
            yield return null;
        }

        public void Unsubscribe()
        {
            EventManager.Instance.Unsubscribe(TypeOfEvent.BallClick, OnBallClicked);
        }
    }
}