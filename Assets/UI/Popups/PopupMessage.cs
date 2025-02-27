using Assets.Infrastructure.Events;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.Popups
{
    //class for both the start game and game over message
    public class PopupMessage : MonoBehaviour
    {
        public TextMeshProUGUI PopupText;

        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _replayButton;
        [SerializeField] private RectTransform _rectTransform;

        private Tween _scaleTween;
        private float animationDuration = 0.75f;
        private Vector3 _normalScale = new Vector3(1, 1, 1);
        private Vector3 _smallScale = new Vector3(0.01f, 0.01f, 0.01f);

        public void ShowWindow()
        {
            _rectTransform.localScale = _smallScale;

            _scaleTween = _rectTransform.DOScale(_normalScale, animationDuration)
                .SetEase(Ease.OutBack);
        }

        public void HideWindow()
        {
            _rectTransform.localScale = _normalScale;

            _scaleTween = _rectTransform.DOScale(_smallScale, animationDuration)
                .SetEase(Ease.InSine)
                .OnComplete(() => HideAnimationDone()); //animation finish callback
        }

        public void ConfirmButtonClick()
        {
            HideWindow();
        }

        public void ReplayButtonClick()
        {
            EventManager.Instance.Publish(TypeOfEvent.ReplayLevel, new EmptyParams());
        }

        public void BackButtonClick()
        {
            EventManager.Instance.Publish(TypeOfEvent.ReturnToMainMenu, new EmptyParams());
        }

        private void HideAnimationDone()
        {
            gameObject.SetActive(false);
            EventManager.Instance.Publish(TypeOfEvent.GameStart, new EmptyParams());
        }
    }

}

