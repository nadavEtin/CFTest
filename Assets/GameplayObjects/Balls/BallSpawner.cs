using Assets.Infrastructure.Factories;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.GameplayObjects.Balls
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        private FactoriesManager _factoriesManager;

        //FOR TESTING
        public FactoriesManager factoriesManager;
        private void Awake()
        {
            _factoriesManager = factoriesManager;
        }


        public void Init(FactoriesManager factoriesManager)
        {
            _factoriesManager = factoriesManager;
        }

        public void SpawnBalls(int numberOfBalls)
        {
            for (int i = 0; i < numberOfBalls; i++)
            {
                var ball = _factoriesManager.GetObject(FactoryType.NormalBall);
                var ballHeight = ball.GetComponent<Renderer>().bounds.size.y;
                var randomVariation = Random.Range(-0.25f, 0.25f);
                ball.transform.position = _spawnPoint.position + 
                    new Vector3(randomVariation, i * ballHeight, 0);
            }
        }
    }
}