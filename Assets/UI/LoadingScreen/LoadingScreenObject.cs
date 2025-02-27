using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.LoadingScreen
{
    public class LoadingScreenObject : MonoBehaviour
    {
        [SerializeField] private Image _progressBar;
        [SerializeField] private TextMeshProUGUI _progressText;
        //[SerializeField] private Image _loadingSpinner;

        [Header("Animation Settings")]
        //[SerializeField] private float spinDuration = 1f; // Time for one full rotation
        [SerializeField] private float _timeToNextTarget = 0.5f; //time to reach target progress

        //[SerializeField] private Ease spinnerEase = Ease.Linear;

        private Ease _progressBarEase = Ease.OutQuad;
        private float _currentProgress = 0f;
        private Tween _progressTween;
        private Tween _spinnerTween;

        private void Start()
        {
            DOTween.SetTweensCapacity(200, 30);
            _progressBar.fillAmount = 0f;

            //StartSpinnerAnimation();
        }

        //private void StartSpinnerAnimation()
        //{
        //    // Kill any existing spinner animation
        //    spinnerTween?.Kill();

        //    // Create infinite rotating animation
        //    spinnerTween = loadingSpinner.transform
        //        .DORotate(new Vector3(0, 0, -360), spinDuration, RotateMode.FastBeyond360)
        //        .SetEase(spinnerEase)
        //        .SetLoops(-1, LoopType.Restart);
        //}

        public void UpdateProgress(float progress)
        {
            _progressTween?.Kill();

            //fill the progress bar to the target value
            _progressTween = DOTween.To(
                getter: () => _currentProgress,
                setter: (value) =>
                {
                    _currentProgress = value;
                    _progressBar.fillAmount = value;
                    _progressText.text = $"{Mathf.Round(value * 100)}%";
                },
                endValue: progress,
                duration: _timeToNextTarget
            ).SetEase(_progressBarEase);
        }

        public IEnumerator OnLoadingCompleted()
        {
            //finish the fill animation
            _progressTween?.Kill();
            _progressTween = DOTween.To(
                getter: () => _currentProgress,
                setter: (value) =>
                {
                    _currentProgress = value;
                    _progressBar.fillAmount = value;
                    _progressText.text = $"{Mathf.Round(value * 100)}%";
                },
                endValue: 1f,
                duration: _timeToNextTarget
            ).SetEase(_progressBarEase);

            //wait for the progress animation to complete
            yield return _progressTween.WaitForCompletion();

            //fade out the text
            Sequence finishSequence = DOTween.Sequence();
            finishSequence.Append(_progressText.DOFade(0f, 0.5f));
            yield return finishSequence.WaitForCompletion();

            //kill all tweens when done
            _spinnerTween?.Kill();
            _progressTween?.Kill();



            // Example finishing animations:
            // Fade out the spinner
            //finishSequence.Append(_loadingSpinner.DOFade(0f, 0.5f));

            // Scale up and fade out the progress bar
            //finishSequence.Join(_progressBar.transform.DOScale(1.2f, 0.5f));
            //finishSequence.Join(_progressBar.DOFade(0f, 0.5f));






        }

        private void OnDestroy()
        {
            //clean up all tweens when the object is destroyed
            _spinnerTween?.Kill();
            _progressTween?.Kill();
            DOTween.Kill(transform);
        }
    }
}

