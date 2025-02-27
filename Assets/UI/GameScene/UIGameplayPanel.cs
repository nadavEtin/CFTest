using TMPro;
using UnityEngine;

namespace Assets.UI.GameScene
{
    [RequireComponent(typeof(RectTransform))]
    public class UIGameplayPanel : MonoBehaviour, IUIGameplayPanel
    {
        [SerializeField] private UISlideAnimation _slideAnimation;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TextMeshProUGUI _panelText;
        [Tooltip("Value is multiples of the panel's width." +
            "\nIt's added or subtracted (depending on direction) to the panel's X position to set its hidden pos")]
        [SerializeField] private float _distanceFromStart;
        [SerializeField] private float _moveAnimationDuration;

        private Vector2 _startingPos, _outOfViewPos, _panelSize;

        public TextMeshProUGUI PanelText => _panelText;

        public void Awake()
        {
            _outOfViewPos = Vector2.one;
            _startingPos = _rectTransform.anchoredPosition;
            _panelSize = _rectTransform.rect.size;
            _slideAnimation.Init(_rectTransform);
        }

        public void MoveIntoView()
        {
            _slideAnimation.MoveOnXAxis(_startingPos.x, _moveAnimationDuration);
        }

        public void MoveOutOfView()
        {
            _slideAnimation.MoveOnXAxis(_outOfViewPos.x, _moveAnimationDuration);
        }

        //ui starts outside the camera view
        public void PositionOutOfView(bool leftDirection)
        {
            if(_outOfViewPos == Vector2.one)
            {
                float distanceFromStartPos = _distanceFromStart * _panelSize.x;
                float newXPos = leftDirection ? _rectTransform.anchoredPosition.x - distanceFromStartPos : _rectTransform.anchoredPosition.x + distanceFromStartPos;
                _outOfViewPos = new Vector2(newXPos, _rectTransform.anchoredPosition.y);
            }
            
            _rectTransform.anchoredPosition = _outOfViewPos;
        }
    }
}