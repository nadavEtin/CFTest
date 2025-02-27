using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Assets.UI.LoadingScreen
{
    public class LoadingScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreenPrefab;
        [SerializeField] private AddressablesLoader _addressablesLoader;

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

            //also load addressable assets
            float totalProgress = 0f;
            var addressableOperations = _addressablesLoader.LoadAllAssets();

            //wait for the loading to finish and update the loading bar on progress
            while (IsLoadingComplete(addressableOperations, out totalProgress) == false)
            {
                _activeLoadingScreen.UpdateProgress(totalProgress);
                yield return null;
            }

            //ensure animation has completed
            yield return StartCoroutine(_activeLoadingScreen.OnLoadingCompleted());

            //start the scene
            asyncLoad.allowSceneActivation = true;

            //clean up
            Destroy(loadingScreenObj);
        }

        private bool IsLoadingComplete(List<AsyncOperationHandle> operations, out float totalProgress)
        {
            float progress = 0f;
            foreach (var op in operations)
            {
                progress += op.PercentComplete;
            }

            totalProgress = progress / operations.Count;
            return operations.All(op => op.PercentComplete >= 0.9f);
        }
    }
}
