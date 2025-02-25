using Assets.GameplayObjects.Balls;
using Assets.GameRules;
using Assets.Infrastructure.Events;
using Assets.Infrastructure.Factories;
using Assets.Scripts.Utility;
using GameCore.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Infrastructure
{
    [RequireComponent(typeof(IRulesTracker))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private AssetRefsScriptableObject _assetRefs;
        [SerializeField] private BallParametersScriptableObject _ballParameters;
        [SerializeField] private GameRulesScriptableObject _gameRules;

        private IRulesTracker _rulesTracker;
        private IFactoriesManager _factoriesManager;
        private BallManager _ballManager;

        private bool _playerWon, _gameOverSequenceStarted;

        //FOR TESTING
        //private bool showCircle;
        //Vector3 circlePosition;
        //float circleRadius;

        private void Awake()
        {
            //FOR TESTING
            //EventManager.Instance.Subscribe(TypeOfEvent.BallClick, OnBallClicked);
            EventManager.Instance.Subscribe(TypeOfEvent.GameOver, GameOverTrigger);
            EventManager.Instance.Subscribe(TypeOfEvent.ScoreTargetReached, TargetScoreReached);

            SetupManagers();
            SetupGameplayScene();
            //EventManager.Instance.Publish(TypeOfEvent.GameStart, new EmptyParams());
        }

        private void SetupManagers()
        {
            var factoriesManager = Instantiate(_assetRefs.FactoriesManager).GetComponent<FactoriesManager>();
            factoriesManager.Init(_assetRefs);
            _factoriesManager = factoriesManager;
            var _ballSpawner = Instantiate(_assetRefs.BallSpawner).GetComponent<BallSpawner>();
            _ballSpawner.Init(_factoriesManager, _ballParameters);
            _ballManager = new BallManager(_ballParameters);

            //REMOVE LATER
            _gameRules.Init(GameDifficulty.Easy);
            _gameRules.SetDifficultySettings();

            _rulesTracker = GetComponent<IRulesTracker>();
            _rulesTracker.Init(_gameRules);
        }

        private void SetupGameplayScene()
        {            
            _canvas.AddComponent<UIManager>().Init(_assetRefs, _mainCamera, _factoriesManager, _gameRules.TargetScore, _gameRules.MaxMoves, _gameRules.TimeLimit);
        }

        private void TargetScoreReached(BaseEventParams eventParams)
        {
            _playerWon = true;
        }

        private void GameOverTrigger(BaseEventParams eventParams)
        {
            if (_gameOverSequenceStarted)
                return;

            _gameOverSequenceStarted = true;
            StartCoroutine(GameOverSequence());
        }

        private IEnumerator GameOverSequence()
        {
            //wait 1 frame to avoid edge case errors from concurrent events
            yield return null;
            if (_playerWon)
                //TODO: show win screen
                Debug.Log("Player Won");
            else
                //TODO: show lose screen
                Debug.Log("Player Lost");
        }

        private void OnDestroy()
        {
            EventManager.Instance.Unsubscribe(TypeOfEvent.GameOver, GameOverTrigger);
            EventManager.Instance.Unsubscribe(TypeOfEvent.ScoreTargetReached, TargetScoreReached);
            _ballManager.Unsubscribe();
        }


        //FOR TESTING
        //public void OnBallClicked(BaseEventParams eventParams)
        //{
        //    var clickedBall = ((BallClickEventParams)eventParams).Ball;
        //    circleRadius = clickedBall.Collider.radius * _ballParameters.SpecialBallExplosionRadius;
        //    circlePosition = clickedBall.Position;
        //    showCircle = true;
        //}


        //private void Update()
        //{
        //    if (showCircle)
        //    {
        //        DrawCircle(circlePosition, circleRadius);
        //    }
        //}

        //FOR TESTING
        //private void DrawCircle(Vector3 position, float radius)
        //{
        //    int segments = 36; // More segments = smoother circle
        //    float angle = 360f / segments;

        //    for (int i = 0; i < segments; i++)
        //    {
        //        float currentAngle = angle * i * Mathf.Deg2Rad;
        //        float nextAngle = angle * (i + 1) * Mathf.Deg2Rad;

        //        Vector3 currentPoint = position + new Vector3(
        //            Mathf.Cos(currentAngle) * radius,
        //            Mathf.Sin(currentAngle) * radius,
        //            0
        //        );

        //        Vector3 nextPoint = position + new Vector3(
        //            Mathf.Cos(nextAngle) * radius,
        //            Mathf.Sin(nextAngle) * radius,
        //            0
        //        );

        //        Debug.DrawLine(currentPoint, nextPoint, Color.black);
        //    }
        //}
    }
}