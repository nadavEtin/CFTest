﻿//using UnityEngine;

//namespace Assets.UI.GameScene
//{
//    public class UIScoreTargetPanel : MonoBehaviour
//    {

//        [SerializeField] private UISlideAnimation _slideAnimation;
//        [SerializeField] private RectTransform _rectTransform;

//        private void Awake()
//        {
//            Vector2 panelSize = _rectTransform.rect.size;
//            var inViewPos = _rectTransform.anchoredPosition;
//            var outOfViewXPos = inViewPos.x - panelSize.x * 3;
//            var outOfViewPos = new Vector2(outOfViewXPos, inViewPos.y);
//            //_slideAnimation.Init(outOfViewPos, inViewPos);
//        }
//    }
//}