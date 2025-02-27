using Assets.Effects;
using Assets.GameplayObjects.Balls;
using Assets.GameRules;
using Assets.Infrastructure.Events;
using Assets.Infrastructure.Factories;
using Assets.UI.GameScene;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField] private EffectRefsScriptableObject _effectRefs;

        private GameplayUIManager _uiManager;
        private IRulesTracker _rulesTracker;
        private IFactoriesManager _factoriesManager;
        private BallManager _ballManager;
        private EffectsManager _effectsManager;

        private bool _playerWon, _gameOverSequenceStarted;
        private int _finalScore;

        private void Awake()
        {
            EventManager.Instance.Subscribe(TypeOfEvent.GameOver, GameOverTrigger);
            EventManager.Instance.Subscribe(TypeOfEvent.ScoreTargetReached, TargetScoreReached);
            EventManager.Instance.Subscribe(TypeOfEvent.ReplayLevel, ReplayLevel);
            EventManager.Instance.Subscribe(TypeOfEvent.ReturnToMainMenu, BackToMainMenu);

            SetupManagers();
            SetupGameplayScene();
        }

        private void SetupManagers()
        {
            var factoriesManager = Instantiate(_assetRefs.FactoriesManager).GetComponent<FactoriesManager>();
            factoriesManager.Init(_assetRefs, _effectRefs);
            _factoriesManager = factoriesManager;
            var _ballSpawner = Instantiate(_assetRefs.BallSpawner).GetComponent<BallSpawner>();
            _ballSpawner.Init(_factoriesManager, _ballParameters);
            _effectsManager = new EffectsManager(_factoriesManager);
            _ballManager = new BallManager(_ballParameters, _effectsManager);

            _rulesTracker = GetComponent<IRulesTracker>();
            _rulesTracker.Init(_gameRules);
        }

        private void SetupGameplayScene()
        {
            _uiManager = _canvas.AddComponent<GameplayUIManager>();
            _uiManager.Init(_assetRefs, _mainCamera, _factoriesManager, _gameRules.TargetScore, _gameRules.MaxMoves, _gameRules.TimeLimit);
        }

        private void TargetScoreReached(BaseEventParams eventParams)
        {
            //for the end of game popup
            _playerWon = true;
        }

        private void GameOverTrigger(BaseEventParams eventParams)
        {
            //to ensure only 1 process runs
            if (_gameOverSequenceStarted)
                return;

            _gameOverSequenceStarted = true;
            _finalScore = ((GameOverEventParams)eventParams).FinalScore;
            SaveGameData();
            StartCoroutine(GameOverSequence());
        }

        private IEnumerator GameOverSequence()
        {
            //wait 1 frame to avoid edge case errors from concurrent events
            yield return null;
            _uiManager.ShowEndGamePopup(_playerWon);
        }

        private void ReplayLevel(BaseEventParams eventParams)
        {
            try
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load gameplay scene: {e.Message}");
            }
        }

        private void BackToMainMenu(BaseEventParams eventParams)
        {
            try
            {
                SceneManager.LoadScene("MainMenu");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load gameplay scene: {e.Message}");
            }
        }

        private void SaveGameData()
        {
            //save game score if its the highest
            var savedBest = PlayerPrefs.GetInt("HighScore", 0);
            if (_finalScore > savedBest)
            {
                PlayerPrefs.SetInt("HighScore", _finalScore);
                PlayerPrefs.Save();
            }
        }

        private void OnDestroy()
        {
            EventManager.Instance.Unsubscribe(TypeOfEvent.GameOver, GameOverTrigger);
            EventManager.Instance.Unsubscribe(TypeOfEvent.ScoreTargetReached, TargetScoreReached);
            EventManager.Instance.Unsubscribe(TypeOfEvent.ReplayLevel, ReplayLevel);
            EventManager.Instance.Unsubscribe(TypeOfEvent.ReturnToMainMenu, BackToMainMenu);
            _ballManager.Unsubscribe();
        }
    }
}