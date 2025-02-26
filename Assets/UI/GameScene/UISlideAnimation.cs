using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Assets.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UISlideAnimation : MonoBehaviour
	{
		//private float _outOfViewPosX, _inViewPosX;
        //private Tween _moveTween;
        private RectTransform _rectTransform;

        public void Init(RectTransform rectTransform)
        {
            //_outOfViewPosX = outOfViewPos;
            //_inViewPosX = inViewPos;
            _rectTransform = rectTransform;
        }

        public void MoveOnXAxis(float targetValue, float duration)
        {
            gameObject.SetActive(true);
            var moveTween = _rectTransform.DOAnchorPosX(targetValue, duration);
        }

  //      public void SlideIntoView()
		//{
  //          if (_moveTween != null && _moveTween.IsPlaying())
  //              _moveTween.Kill();

  //          //_rectTransform.anchoredPosition = new Vector2(_outOfViewPosX, _rectTransform.anchoredPosition.y);
            
  //      }

  //      public void SlideOutOfView()
  //      {
  //          if (_moveTween != null && _moveTween.IsPlaying())
  //              _moveTween.Kill();

  //          //_rectTransform.localPosition = _inViewPosX;
  //          _moveTween = _rectTransform.DOAnchorPosX(_outOfViewPosX, 0.5f);
  //      }
    }
}