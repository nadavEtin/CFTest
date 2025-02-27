using DG.Tweening;
using UnityEngine;

namespace Assets.UI.GameScene
{
    [RequireComponent(typeof(RectTransform))]
    public class UISlideAnimation : MonoBehaviour
    {
        private RectTransform _rectTransform;

        public void Init(RectTransform rectTransform)
        {
            _rectTransform = rectTransform;
        }

        public void MoveOnXAxis(float targetValue, float duration)
        {
            gameObject.SetActive(true);
            var moveTween = _rectTransform.DOAnchorPosX(targetValue, duration);
        }
    }
}