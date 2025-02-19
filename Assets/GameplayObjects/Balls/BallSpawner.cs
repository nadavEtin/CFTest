using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.GameplayObjects.Balls
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;

        private BallObjectPool _ballObjectPool;

        private void Start()
        {
            _ballObjectPool = new BallObjectPool();
        }
    }
}