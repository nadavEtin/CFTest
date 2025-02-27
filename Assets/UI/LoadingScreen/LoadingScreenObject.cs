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
        [SerializeField] private float _timeToNextTarget = 0.5f; //time to reach target progress

        private Ease _progressBarEase = Ease.OutQuad;
        private float _currentProgress = 0f;
        private Tween _progressTween;

        private void Start()
        {
            DOTween.SetTweensCapacity(200, 30);
            _progressBar.fillAmount = 0f;
        }

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

            _progressTween?.Kill();
        }

        private void OnDestroy()
        {
            //clean up when the object is destroyed
            _progressTween?.Kill();
            DOTween.Kill(transform);
        }
    }
}

