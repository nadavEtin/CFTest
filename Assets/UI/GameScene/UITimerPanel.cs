//using UnityEngine;

//namespace Assets.UI.GameScene
//{
//    public class UITimerPanel : MonoBehaviour
//    {

//        [SerializeField] private UISlideAnimation _slideAnimation;
//        [SerializeField] private RectTransform _rectTransform;

//        private void Awake()
//        {
//            Vector2 panelSize = _rectTransform.rect.size;
//            var inViewPosX = _rectTransform.anchoredPosition.x;
//            var outOfViewXPos = inViewPosX + panelSize.x * 1.5f;             
//            //_slideAnimation.Init(outOfViewXPos, inViewPosX);
//            //_rectTransform.anchoredPosition = new Vector2(outOfViewXPos, _rectTransform.anchoredPosition.y);
//            //_slideAnimation.SlideIntoView();
//        }

//        private void GameStart()
//        {
//            //_slideAnimation.SlideIntoView();
//        }
//    }
//}