using Assets.GameplayObjects.Balls;
using Assets.GameRules;
using Assets.Infrastructure.Events;
using Assets.Infrastructure.Factories;
using Assets.Scripts.Utility;
using Assets.Scripts.Utility.Events;
using GameCore.UI;
using UnityEngine;

namespace Assets.Infrastructure
{
    [RequireComponent(typeof(IRulesTracker))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private AssetRefsScriptableObject _assetRefs;
        [SerializeField] private BallParametersScriptableObject _ballParameters;
        [SerializeField] private GameRulesScriptableObject _gameRules;

        private IRulesTracker _rulesTracker;
        private BallManager _ballManager;


        //FOR TESTING
        private bool showCircle;
        Vector3 circlePosition;
        float circleRadius;

        private void Awake()
        {
            //FOR TESTING
            EventManager.Instance.Subscribe(TypeOfEvent.BallClick, OnBallClicked);

            SetupManagers();
            SetupGameplayScene();
            EventManager.Instance.Publish(TypeOfEvent.GameStart, new EmptyParams());
        }

        private void SetupManagers()
        {
            var _factoriesManager = Instantiate(_assetRefs.FactoriesManager).GetComponent<FactoriesManager>();
            _factoriesManager.Init(_assetRefs);
            var _ballSpawner = Instantiate(_assetRefs.BallSpawner).GetComponent<BallSpawner>();
            _ballSpawner.Init(_factoriesManager, _ballParameters);
            _ballManager = new BallManager(_ballParameters);

            //REMOVE LATER
            _gameRules.Init(GameDifficulty.Easy);
            _gameRules.SetDifficultySettings();
        }

        private void SetupGameplayScene()
        {
            _rulesTracker = GetComponent<IRulesTracker>();
            _rulesTracker.Init(_gameRules);
            _canvas.AddComponent<UIManager>().Init(_assetRefs, _gameRules.MaxMoves, _gameRules.TimeLimit);
        }





        private void LeavingGameplayScene()
        {
            _ballManager.Unsubscribe();
            //EventManager.Instance.Unsubscribe(TypeOfEvent.BallClick, OnBallClicked);
        }




        //FOR TESTING
        public void OnBallClicked(BaseEventParams eventParams)
        {
            var clickedBall = ((BallClickEventParams)eventParams).Ball;
            circleRadius = clickedBall.Collider.radius * _ballParameters.SpecialBallExplosionRadius;
            circlePosition = clickedBall.Position;
            showCircle = true;
        }


        private void Update()
        {
            if (showCircle)
            {
                DrawCircle(circlePosition, circleRadius);
            }
        }

        //FOR TESTING
        private void DrawCircle(Vector3 position, float radius)
        {
            int segments = 36; // More segments = smoother circle
            float angle = 360f / segments;

            for (int i = 0; i < segments; i++)
            {
                float currentAngle = angle * i * Mathf.Deg2Rad;
                float nextAngle = angle * (i + 1) * Mathf.Deg2Rad;

                Vector3 currentPoint = position + new Vector3(
                    Mathf.Cos(currentAngle) * radius,
                    Mathf.Sin(currentAngle) * radius,
                    0
                );

                Vector3 nextPoint = position + new Vector3(
                    Mathf.Cos(nextAngle) * radius,
                    Mathf.Sin(nextAngle) * radius,
                    0
                );

                Debug.DrawLine(currentPoint, nextPoint, Color.black);
            }
        }
    }
}