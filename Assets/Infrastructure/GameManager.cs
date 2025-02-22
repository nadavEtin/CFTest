using Assets.GameplayObjects.Balls;
using Assets.Infrastructure.Events;
using Assets.Infrastructure.Factories;
using Assets.Scripts.Utility;
using Events;
using UnityEngine;

namespace Assets.Infrastructure
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private AssetRefsScriptableObject _assetRefs;
        [SerializeField] private BallParameterScriptableObject _ballParameter;

        private BallManager _ballManager;


        //FOR TESTING
        private bool showCircle;
        Vector3 circlePosition;
        float circleRadius;

        private void Awake()
        {
            //FOR TESTING
            //EventManager.Instance.Subscribe(TypeOfEvent.BallClick, OnBallClicked);

            SetupGameplayScene();
        }

        private void SetupGameplayScene()
        {
            var _factoriesManager = Instantiate(_assetRefs.FactoriesManager).GetComponent<FactoriesManager>();
            _factoriesManager.Init(_assetRefs);
            var _ballSpawner = Instantiate(_assetRefs.BallSpawner).GetComponent<BallSpawner>();
            _ballSpawner.Init(_factoriesManager, _ballParameter);
            _ballManager = new BallManager();
        }





        private void LeavingGameplayScene()
        {
            _ballManager.Unsubscribe();
            //EventManager.Instance.Unsubscribe(TypeOfEvent.BallClick, OnBallClicked);
        }




        //FOR TESTING
        //public void OnBallClicked(BaseEventParams eventParams)
        //{
        //    var clickedBall = ((BallClickEventParams)eventParams).Ball;
        //    circleRadius = clickedBall.Collider.radius * 1.03f;
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

        ////FOR TESTING
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