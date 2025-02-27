using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.UI.LoadingScreen
{
    public class LoadingScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreenPrefab;

        private LoadingScreenObject _activeLoadingScreen;

        private void Start()
        {
            LoadScene("MainMenu");
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            GameObject loadingScreenObj = Instantiate(_loadingScreenPrefab, transform);
            _activeLoadingScreen = loadingScreenObj.GetComponent<LoadingScreenObject>();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            // Start loading game assets
            float totalProgress = 0f;
            List<AsyncOperation> asyncOperations = new List<AsyncOperation>();

            // Add your asset loading operations here
            asyncOperations.Add(asyncLoad);
            // Example: asyncOperations.Add(Resources.LoadAsync<GameObject>("PreloadAssets"));

            while (IsLoadingComplete(asyncOperations, out totalProgress) == false)
            {
                _activeLoadingScreen.UpdateProgress(totalProgress);
                yield return null;
            }

            // Ensure animation has completed
            yield return StartCoroutine(_activeLoadingScreen.OnLoadingCompleted());

            // Activate the scene
            asyncLoad.allowSceneActivation = true;

            // Clean up
            Destroy(loadingScreenObj);
        }

        private bool IsLoadingComplete(List<AsyncOperation> operations, out float totalProgress)
        {
            float progress = 0f;
            foreach (AsyncOperation op in operations)
            {
                progress += op.progress;
            }

            totalProgress = progress / operations.Count;
            return totalProgress >= 0.9f;   //asyncOperation usually only goes to 0.9
        }
    }
}
