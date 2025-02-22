using Assets.Infrastructure.Events;
using Assets.Infrastructure.ObjectPool;
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

        public BallManager(BallParametersScriptableObject ballParameters)
        {
            EventManager.Instance.Subscribe(TypeOfEvent.BallClick, OnBallClicked);
            _ballParameters = ballParameters;
        }

        private HashSet<INormalBall> GetConnectedBalls(INormalBall startBall)
        {
            HashSet<INormalBall> connectedBalls = new HashSet<INormalBall> { startBall };
            HashSet<INormalBall> newlyFound = new HashSet<INormalBall> { startBall };

            while (newlyFound.Count > 0)
            {
                HashSet<INormalBall> nextBatch = new HashSet<INormalBall>();

                foreach (INormalBall ball in newlyFound)
                {
                    var neighbors = GetNeighbors(ball);
                    foreach (INormalBall neighbor in neighbors)
                    {
                        if (neighbor.Type == startBall.Type && !connectedBalls.Contains(neighbor))
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

        private HashSet<INormalBall> GetNeighbors(INormalBall ball)
        {
            var neighbors = new HashSet<INormalBall>();
            float checkRadius = ball.Collider.radius * 1.1f;

            Collider2D[] results = new Collider2D[8];
            var numColliders = Physics2D.OverlapCircleNonAlloc(
                ball.Position,
                checkRadius,
                results,
                LayerMask.GetMask("Ball")   //ignore walls and other non-ball objects
            );

            for (int i = 0; i < numColliders; i++)
            {
                var neighbor = results[i].GetComponent<INormalBall>();
                if (neighbor != null && neighbor != this)
                    neighbors.Add(neighbor);
            }

            return neighbors;
        }

        private void OnBallClicked(BaseEventParams eventParams)
        {
            var clickedBall = ((BallClickEventParams)eventParams).Ball;
            HashSet<INormalBall> connected = GetConnectedBalls(clickedBall);

            if (connected.Count >= 3)
            {
                foreach (INormalBall ball in connected)
                {
                    //ADD FX HERE
                    RemoveBall(ball);
                }

                var score = _ballParameters.CalculateScore(connected.Count);
                EventManager.Instance.Publish(TypeOfEvent.ScoreUpdate, new ScoreUpdateEventParams(score));

                //replace removed balls
                EventManager.Instance.Publish(TypeOfEvent.SpawnNewBalls, new SpawnNewBallsEventParams(connected.Count));
            }
        }

        private void RemoveBall(INormalBall ball)
        {
            var ballObject = ball.Collider.gameObject;
            if (ballObject.GetComponent<IPooledObject>() != null)
            {
                ballObject.SetActive(false);
                ballObject.transform.position = new Vector2(100, 0);    //move ball away from the scene to avoid unexpected errors
                ballObject.GetComponent<IPooledObject>().ReturnToPool();
            }
            else
                GameObject.Destroy(ballObject);

        }

        private IEnumerator ExplodeBall(INormalBall ball)
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