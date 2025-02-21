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


        public BallManager()
        {
            EventManager.Instance.Subscribe(TypeOfEvent.BallClick, OnBallClicked);
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

        public void OnBallClicked(BaseEventParams eventParams)
        {
            var clickedBall = ((BallClickEventParams)eventParams).Ball;
            HashSet<INormalBall> connected = GetConnectedBalls(clickedBall);

            if (connected.Count >= 3)
            {
                foreach (INormalBall ball in connected)
                {
                    // Consider using object pooling instead of Destroy

                }
            }
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