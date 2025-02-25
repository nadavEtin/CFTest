using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.UI.GameScene
{
	[RequireComponent(typeof(RectTransform))]
	public class UIGameplayPanel : MonoBehaviour
	{
        [SerializeField] private UISlideAnimation _slideAnimation;
        [SerializeField] private RectTransform _rectTransform;

        private void Awake()
        {
            //_rectTransform = GetComponent<RectTransform>();
            Vector2 worldSize = _rectTransform.rect.size * _rectTransform.lossyScale;
            var inViewPos = _rectTransform.localPosition;
            //var outOfViewPosX = ScreenDisplayInformation.Instance.
            //_slideAnimation.Init();
        }


	}
}